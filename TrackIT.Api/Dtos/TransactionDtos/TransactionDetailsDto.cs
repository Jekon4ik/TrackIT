namespace TrackIT.Api.Dtos;

public record class TransactionDetailsDto(
    int Id, 
    decimal Amount, 
    DateOnly Date,
    int CategoryId,
    string Description);
