using System;

namespace TrackIT.Api.Entities;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public int TypeId { get; set; }
    public CategoryType? Type { get; set; }
}
