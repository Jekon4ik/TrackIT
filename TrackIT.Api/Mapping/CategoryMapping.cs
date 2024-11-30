using System;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;
namespace TrackIT.Api.Mapping;

public static class CategoryMapping
{
    public static Category toEntity(this CreateCategoryDto category){
        return new Category () 
        {
            Name = category.Name,
            TypeId = category.TypeId
        };
    }

    public static CategoryDetailsDto toCategoryDetailsDto(this Category category){
        return new CategoryDetailsDto(
            category.Id,
            category.Name,
            category.Type!.Id
            );
    }

    public static CategorySummaryDto toCategorySummaryDto(this Category category){
        return new(
            category.Id,
            category.Name,
            category.Type!.Name
            );
    }
}
