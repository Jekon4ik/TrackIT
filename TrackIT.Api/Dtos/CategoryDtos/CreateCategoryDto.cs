using System.ComponentModel.DataAnnotations;

namespace TrackIT.Api.Dtos;

public record class CreateCategoryDto(
    [Required] [StringLength(20)]string Name,
    [Required] [StringLength(8)]string Type
);