using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TrackIT.Api.Data;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;
using TrackIT.Api.Mapping;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TrackIT.Api.Configuration;
using Microsoft.Extensions.Logging;

namespace TrackIT.Api.Endpoints;

public static class CategoriesEndpoints
{
    const string GetCategoryEndpointName = "GetCategory";
    const string CategoriesCacheKey = "Categories";

    public static RouteGroupBuilder MapCategoriesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("categories").WithParameterValidation().WithTags("Categories");

        group.MapGet("/", async (
            TrackITContext dbContext,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings,
            ILogger<TrackITContext> logger) =>
        {
            if (!cacheSettings.Value.EnableCaching)
            {
                logger.LogInformation("Getting list of categories.");
                var categories = await dbContext.Categories
                    .Include(category => category.Type)
                    .Select(category => category.toCategorySummaryDto())
                    .AsNoTracking()
                    .ToListAsync();

                return categories;
            }

            List<CategorySummaryDto> cachedCategories = new();
            if (!cache.TryGetValue(CategoriesCacheKey, out cachedCategories))
            {
                logger.LogInformation("Cache miss for categories. Fetching from database.");
                cachedCategories = await dbContext.Categories
                    .Include(category => category.Type)
                    .Select(category => category.toCategorySummaryDto())
                    .AsNoTracking()
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheSettings.Value.CategoriesExpirationMinutes))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(cacheSettings.Value.SlidingExpirationMinutes));

                cache.Set(CategoriesCacheKey, cachedCategories, cacheOptions);
                logger.LogInformation("Categories cached successfully.");
            }
            else
            {
                logger.LogInformation("Cache hit for categories.");
            }

            return cachedCategories;
        });

        group.MapGet("/{id}", async (
            int id,
            TrackITContext dbContext,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings,
            ILogger<TrackITContext> logger) =>
        {
            if (!cacheSettings.Value.EnableCaching)
            {
                logger.LogInformation("Getting category with ID {Id}.", id);
                var category = await dbContext.Categories.FindAsync(id);
                if (category is null)
                {
                    logger.LogWarning("Category with ID {Id} not found.", id);
                    return Results.NotFound();
                }
                return Results.Ok(category.toCategoryDetailsDto());
            }

            CategoryDetailsDto? cachedCategory = null;
            string cacheKey = $"{GetCategoryEndpointName}_{id}";

            if (!cache.TryGetValue(cacheKey, out cachedCategory))
            {
                logger.LogInformation("Cache miss for category with ID {Id}. Fetching from database.", id);
                var category = await dbContext.Categories.FindAsync(id);

                if (category is null)
                {
                    logger.LogWarning("Category with ID {Id} not found.", id);
                    return Results.NotFound();
                }

                cachedCategory = category.toCategoryDetailsDto();
                cache.Set(cacheKey, cachedCategory, TimeSpan.FromMinutes(cacheSettings.Value.CategoriesExpirationMinutes));
                logger.LogInformation("Category with ID {Id} cached successfully.", id);
            }
            else
            {
                logger.LogInformation("Cache hit for category with ID {Id}.", id);
            }

            return cachedCategory is not null 
                ? Results.Ok(cachedCategory) 
                : Results.NotFound();
        });

        group.MapPost("/", (
            CreateCategoryDto newCategory,
            TrackITContext dbContext,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings,
            ILogger<TrackITContext> logger) =>
        {
            logger.LogInformation("Creating new category: {CategoryName}", newCategory.Name);
            var category = newCategory.toEntity();
            dbContext.Categories.Add(category);
            dbContext.SaveChanges();

            if (cacheSettings.Value.EnableCaching)
            {
                cache.Remove(CategoriesCacheKey);
                logger.LogInformation("Cache invalidated after creating a category.");
            }

            return Results.CreatedAtRoute(GetCategoryEndpointName, new { id = category.Id }, category.toCategoryDetailsDto());
        });

        group.MapPut("/{id}", (
            int id,
            UpdateCategoryDto updatedCategory,
            TrackITContext dbContext,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings,
            ILogger<TrackITContext> logger) =>
        {
            logger.LogInformation("Updating category with ID: {Id}", id);
            var existingCategory = dbContext.Categories.Find(id);

            if (existingCategory is null)
            {
                logger.LogWarning("Category with ID {Id} not found for update.", id);
                return Results.NotFound();
            }

            dbContext.Entry(existingCategory).CurrentValues.SetValues(updatedCategory.toEntity(id));
            dbContext.SaveChanges();

            if (cacheSettings.Value.EnableCaching)
            {
                cache.Remove(CategoriesCacheKey);
                cache.Remove($"{GetCategoryEndpointName}_{id}");
                logger.LogInformation("Cache invalidated after updating category with ID {Id}.", id);
            }

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (
            int id,
            TrackITContext dbContext,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings,
            ILogger<TrackITContext> logger) =>
        {
            logger.LogInformation("Deleting category with ID: {Id}", id);
            dbContext.Categories.Where(category => category.Id == id).ExecuteDelete();

            if (cacheSettings.Value.EnableCaching)
            {
                cache.Remove(CategoriesCacheKey);
                cache.Remove($"{GetCategoryEndpointName}_{id}");
                logger.LogInformation("Cache invalidated after deleting category with ID {Id}.", id);
            }

            return Results.NoContent();
        });

        return group;
    }
}
