using System.ComponentModel.DataAnnotations;

namespace TrackIT.Api.Dtos;

public record class UpdateCategoryDto(
    [Required] [StringLength(20)]string Name,
    [Range(1, int.MaxValue, ErrorMessage = "TypeId must be a positive integer.")]
    int TypeId
);
