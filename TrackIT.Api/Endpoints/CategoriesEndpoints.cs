using System;
using TrackIT.Api.Data;
using TrackIT.Api.Dtos;

namespace TrackIT.Api.Endpoints;

public static class CategoriesEndpoints{
    const string GetCategoryEndpointName = "GetCategory";

    public static RouteGroupBuilder MapCategoriesEndpoints(this WebApplication app)
    {
        
        var group = app.MapGroup("categories").WithParameterValidation().WithTags("Categories");
        group.MapGet("/", ()=> CategoryData.categoriesList);
        
        group.MapGet("/{id}", (int id)=> 
            {
                CategoryDto? category = CategoryData.categoriesList.Find(category=>category.Id == id);

                return category is null ? Results.NotFound() : Results.Ok(category);
            }).WithName(GetCategoryEndpointName);
        
        group.MapPost("/", (CreateCategoryDto newCategory) => 
            {
                CategoryDto category = new (
                    CategoryData.categoriesList.Count() +1,
                    newCategory.Name,
                    newCategory.Type
                );
                CategoryData.categoriesList.Add(category);
                return Results.CreatedAtRoute(GetCategoryEndpointName, new{id = category.Id}, category);
            });

        group.MapPut("/{id}", (int id, UpdateCategoryDto updatedCategory) =>
            {
                var index = CategoryData.categoriesList.FindIndex(category=> category.Id==id);
                if(index == -1) return Results.NotFound();

                CategoryData.categoriesList[index] = new CategoryDto(
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
