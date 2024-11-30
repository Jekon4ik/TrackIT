using System;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;

namespace TrackIT.Api.Mapping;

public static class TransactionMapping
{
    public static Transaction toEntity(this CreateTransactionDto transaction){
         return new Transaction(){
                    Amount = transaction.Amount,
                    Date = transaction.Date,
                    CategoryId = transaction.CategoryId,
                    Description = transaction.Description
                };
    }

    public static TransactionDetailsDto toTransactionDetailsDto(this Transaction transaction){
        return new TransactionDetailsDto(
                    transaction.Id,
                    transaction.Amount,
                    transaction.Date,
                    transaction.Category!.Id,
                    transaction.Description!
                ); 
    }

    public static TransactionSummaryDto toTransactionSummaryDto(this Transaction transaction){
        return new TransactionSummaryDto(
                    transaction.Id,
                    transaction.Amount,
                    transaction.Date,
                    transaction.Category!.Name,
                    transaction.Description!
                );
    }
}
