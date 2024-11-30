namespace TrackIT.Api.Dtos;

public record class TransactionSummaryDto(
    int Id, 
    decimal Amount, 
    DateOnly Date,
    string Category,
    string Description);
