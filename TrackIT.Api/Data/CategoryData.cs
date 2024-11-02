using System;
using TrackIT.Api.Dtos;

namespace TrackIT.Api.Data;

public static class CategoryData
{
    public static List<CategoryDto> categoriesList {get;} = [
    new(
        1,
        "Salary",
        "Income"    
    ),
    new(
        2,
        "Part-time work",
        "Income"
    ),
    new(
        3,
        "Groceries",
        "Expenses"
    ),
    new(
        4,
        "Restaurants",
        "Expenses"
    ),
    new(
        5,
        "Entertainment",
        "Expenses"
    ),
    new(
        6,
        "Health",
        "Expenses"
    ),
    new(
        7,
        "Purchases",
        "Expenses"
    ),
    new(
        8,
        "Transport",
        "Expenses"
    ),
    new(
        9,
        "Gifts",
        "Expenses"
    ),
    new(
        10,
        "Home",
        "Expenses"
    )
];
}
