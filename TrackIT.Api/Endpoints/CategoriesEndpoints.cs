using System;
using System.Data.Common;
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
        group.MapGet("/", ()=> CategoryData.categoriesList);
        
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

        group.MapPut("/{id}", (int id, UpdateCategoryDto updatedCategory) =>
            {
                var index = CategoryData.categoriesList.FindIndex(category=> category.Id==id);
                if(index == -1) return Results.NotFound();

                CategoryData.categoriesList[index] = new CategorySummaryDto(
                    id,
                    updatedCategory.Name,
                    updatedCategory.Type
                );
                return Results.NoContent();
            });

        group.MapDelete("/{id}", (int id) => 
            {
                CategoryData.categoriesList.RemoveAll(category => category.Id==id);
                return Results.NoContent();
            });

        return group;
    }

}
