using System;
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
    
        group.MapGet("/", ()=> TransactionData.transactionsList);

        group.MapGet("/{id}", (int id, TrackITContext dbContext)=> 
            {
                Transaction? transaction = dbContext.Transactions.Find(id);
                if(transaction == null) System.Console.WriteLine("bebra");
                return transaction is null ? Results.NotFound() : Results.Ok(transaction.toTransactionDetailsDto());
            }).WithName(GetTransactionEndpointName);

        group.MapPost("/", (CreateTransactionDto newTransaction, TrackITContext dbContext) => 
            {
                Transaction transaction = newTransaction.toEntity();

                dbContext.Transactions.Add(transaction);
                dbContext.SaveChanges();        

            return Results.CreatedAtRoute(GetTransactionEndpointName, new{id = transaction.Id}, transaction.toTransactionDetailsDto());
            });

        group.MapPut("/{id}", (int id, UpdateTransactionDto updatedTransaction) => 
            {
            var index = TransactionData.transactionsList.FindIndex(transaction => transaction.Id == id);
                
            if(index == -1) return Results.NotFound();
                
            TransactionData.transactionsList[index] = new TransactionSummaryDto(
                id,
                updatedTransaction.Amount,
                updatedTransaction.Date,
                updatedTransaction.Category,
                updatedTransaction.Description
                );
            return Results.NoContent();
            });

        group.MapDelete("/{id}", (int id) => 
            {
            TransactionData.transactionsList.RemoveAll(transaction => transaction.Id==id);   
            return Results.NoContent();
            });

        return group;
    }
}
