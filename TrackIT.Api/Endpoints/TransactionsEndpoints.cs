using System;
using Microsoft.EntityFrameworkCore;
using TrackIT.Api.Data;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;
using TrackIT.Api.Mapping;
namespace TrackIT.Api.Endpoints;

public static class TransactionsEndpoints
{
    const string GetTransactionEndpointName = "GetTransaction";

    public static RouteGroupBuilder MapTransactionsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("transactions").WithParameterValidation().WithTags("Transactions");
    
        group.MapGet("/", (TrackITContext dbContext )=> 
            dbContext.Transactions
            .Include(transaction=> transaction.Category)
            .Select(transaction=>transaction.toTransactionSummaryDto())
            .AsNoTracking()
        );

        group.MapGet("/{id}", (int id, TrackITContext dbContext)=> 
            {
                Transaction? transaction = dbContext.Transactions.Find(id);
                return transaction is null ? Results.NotFound() : Results.Ok(transaction.toTransactionDetailsDto());
            }).WithName(GetTransactionEndpointName);

        group.MapPost("/", (CreateTransactionDto newTransaction, TrackITContext dbContext) => 
            {
                Transaction transaction = newTransaction.toEntity();
                dbContext.Transactions.Add(transaction);
                dbContext.SaveChanges();  
                TransactionDetailsDto transactionDetailsDto = transaction.toTransactionDetailsDto();
                return Results.CreatedAtRoute(GetTransactionEndpointName, new{id = transaction.Id}, transactionDetailsDto);
            });

        group.MapPut("/{id}", (int id, UpdateTransactionDto updatedTransaction, TrackITContext dbContext) => 
            {
            var existingTransaction = dbContext.Transactions.Find(id);
            if(existingTransaction is null) return Results.NotFound();
                
            dbContext.Entry(existingTransaction).CurrentValues.SetValues(updatedTransaction
            .toEntity(id));
            dbContext.SaveChanges();
            return Results.NoContent();
            });

        group.MapDelete("/{id}", (int id, TrackITContext dbContext) => 
            {
            dbContext.Transactions
                        .Where(transaction=>transaction.Id == id)
                        .ExecuteDelete();
            return Results.NoContent();
            });

        return group;
    }
}
