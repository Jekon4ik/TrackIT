using System.ComponentModel.DataAnnotations;

namespace TrackIT.Api.Dtos;

public record class CreateCategoryDto(
    [Required] [StringLength(20)]string Name,
    [Range(1, 2, ErrorMessage = "There is no Type with such ID")]
    int TypeId
);