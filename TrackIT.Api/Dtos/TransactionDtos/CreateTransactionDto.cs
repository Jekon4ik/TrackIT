namespace TrackIT.Api.Dtos;

public record class CreateTransactionDto(
    decimal Amount, 
    DateOnly Date,
    int CategoryId,
    string Description);
