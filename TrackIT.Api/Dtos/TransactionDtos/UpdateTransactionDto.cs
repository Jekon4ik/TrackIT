namespace TrackIT.Api.Dtos;

public record class UpdateTransactionDto(
    decimal Amount, 
    DateOnly Date,
    int CategoryId,
    string Description
    );
