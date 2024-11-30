using System.ComponentModel.DataAnnotations;

namespace TrackIT.Api.Dtos;

public record class CreateTransactionDto(
    [Required] [Range(1,int.MaxValue, ErrorMessage = "Amount must be a positive integer.")]
    decimal Amount, 
    DateOnly Date,
    [Required] [Range(1, int.MaxValue, ErrorMessage = "CategoryId must be a positive integer.")]
    int CategoryId,
    string Description);
