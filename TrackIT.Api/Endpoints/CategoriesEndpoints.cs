using System;
using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using TrackIT.Api.Data;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;
using TrackIT.Api.Mapping;

namespace TrackIT.Api.Endpoints;

public static class CategoriesEndpoints{
    const string GetCategoryEndpointName = "GetCategory";

    public static RouteGroupBuilder MapCategoriesEndpoints(this WebApplication app)
    {
        
        var group = app.MapGroup("categories").WithParameterValidation().WithTags("Categories");
        group.MapGet("/", (TrackITContext dbContext) => 
            dbContext.Categories
            .Include(category=> category.Type)
            .Select(category => category.toCategorySummaryDto())
            .AsNoTracking()
        );
        
        group.MapGet("/{id}", (int id, TrackITContext dbContext)=> 
            {
                Category? category = dbContext.Categories.Find(id);

                return category is null ? Results.NotFound() : Results.Ok(category.toCategoryDetailsDto());
            }).WithName(GetCategoryEndpointName);
        
        group.MapPost("/", (CreateCategoryDto newCategory, TrackITContext dbContext) => 
            {
                Category category = newCategory.toEntity();

                dbContext.Categories.Add(category);
                dbContext.SaveChanges();

                return Results.CreatedAtRoute(GetCategoryEndpointName, new{id = category.Id}, category.toCategoryDetailsDto());
            });

        group.MapPut("/{id}", (int id, UpdateCategoryDto updatedCategory, TrackITContext dbContext) =>
            {
                var existingCategory = dbContext.Categories.Find(id);
                if(existingCategory is null) return Results.NotFound();

                dbContext.Entry(existingCategory).CurrentValues.SetValues(updatedCategory.toEntity(id));
                dbContext.SaveChanges();
                return Results.NoContent();
            });

        group.MapDelete("/{id}", (int id, TrackITContext dbContext) => 
            {
                dbContext.Categories
                            .Where(category=>category.Id == id)
                            .ExecuteDelete();
                return Results.NoContent();
            });

        return group;
    }

}
