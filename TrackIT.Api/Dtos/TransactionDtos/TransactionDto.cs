namespace TrackIT.Api.Dtos;

public record class TransactionDto(
    int Id, 
    decimal Amount, 
    DateOnly Date,
    int CategoryId,
    string Description);
