using System.ComponentModel.DataAnnotations;

namespace TrackIT.Api.Dtos;

public record class CreateCategoryDto(
    [Required] [StringLength(20)]string Name,
    int TypeId
);