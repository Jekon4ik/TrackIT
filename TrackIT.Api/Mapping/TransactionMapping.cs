using System;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;

namespace TrackIT.Api.Mapping;

public static class TransactionMapping
{
    public static Transaction toEntity(this CreateTransactionDto transaction){
        Transaction newTransaction = new Transaction(){
            Amount = transaction.Amount,
            Date = transaction.Date,
            CategoryId = transaction.CategoryId,
            Description = transaction.Description
        };  
        return newTransaction;    
    }

    public static Transaction toEntity(this UpdateTransactionDto transaction, int id){
        Transaction newTransaction = new Transaction(){
            Id = id,
            Amount = transaction.Amount,
            Date = transaction.Date,
            CategoryId = transaction.CategoryId,
            Description = transaction.Description
        };  
        return newTransaction;    
    }

    public static TransactionDetailsDto toTransactionDetailsDto(this Transaction transaction){
        TransactionDetailsDto transactionDetailsDto = new(
            transaction.Id,
            transaction.Amount,
            transaction.Date,
            transaction.CategoryId,
            transaction.Description!
        );
        return transactionDetailsDto;      
    }

    public static TransactionSummaryDto toTransactionSummaryDto(this Transaction transaction){
        TransactionSummaryDto transactionSummaryDto = new(
            transaction.Id,
            transaction.Amount,
            transaction.Date,
            transaction.Category!.Name,
            transaction.Description!
        );
        return transactionSummaryDto;  
    }
}
