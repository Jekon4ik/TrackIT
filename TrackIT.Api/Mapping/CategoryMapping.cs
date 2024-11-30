using System;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;
namespace TrackIT.Api.Mapping;

public static class CategoryMapping
{
    public static Category toEntity(this CreateCategoryDto category){
        Category newCategory = new Category(){
            Name = category.Name,
            TypeId = category.TypeId
        };
        return newCategory;
    }

    public static Category toEntity(this UpdateCategoryDto category, int id){
        Category newCategory = new Category(){
            Id = id,
            Name = category.Name,
            TypeId = category.TypeId
        };
        return newCategory;
    }

    public static CategoryDetailsDto toCategoryDetailsDto(this Category category){
        CategoryDetailsDto categoryDetailsDto = new CategoryDetailsDto(
            category.Id,
            category.Name,
            category.TypeId
        );
        return categoryDetailsDto;
    }

    public static CategorySummaryDto toCategorySummaryDto(this Category category){
        CategorySummaryDto categorySummaryDto = new CategorySummaryDto(
            category.Id,
            category.Name,
            category.Type!.Name
        );
        return categorySummaryDto;
    }
}
