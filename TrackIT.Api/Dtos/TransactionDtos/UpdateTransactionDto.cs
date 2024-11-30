using System.ComponentModel.DataAnnotations;
namespace TrackIT.Api.Dtos;

public record class UpdateTransactionDto(
    [Required] [Range(1,999999)]decimal Amount, 
    DateOnly Date,
    [Required]string Category,
    string Description
    );
