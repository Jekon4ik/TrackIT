using System;
using TrackIT.Api.Data;
using TrackIT.Api.Dtos;
namespace TrackIT.Api.Endpoints;

public static class TransactionsEndpoints
{
    const string GetTransactionEndpointName = "GetTransaction";

    public static RouteGroupBuilder MapTransactionsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("transactions").WithParameterValidation().WithTags("Transactions");
    
        group.MapGet("/", ()=> TransactionData.transactionsList);

        group.MapGet("/{id}", (int id)=> 
            {
            TransactionDto? transaction = TransactionData.transactionsList.Find(transaction=>transaction.Id == id);  
            return transaction is null ? Results.NotFound() : Results.Ok(transaction);
            }).WithName(GetTransactionEndpointName);

        group.MapPost("/", (CreateTransactionDto newTransaction) => 
            {
            TransactionDto transaction = new (
                TransactionData.transactionsList.Count() +1,
                newTransaction.Amount,
                newTransaction.Date,
                newTransaction.CategoryId,
                newTransaction.Description
                );
            TransactionData.transactionsList.Add(transaction);

            return Results.CreatedAtRoute(GetTransactionEndpointName, new{id = transaction.Id}, transaction);
            });

        group.MapPut("/{id}", (int id, UpdateTransactionDto updatedTransaction) => 
            {
            var index = TransactionData.transactionsList.FindIndex(transaction => transaction.Id == id);
                
            if(index == -1) return Results.NotFound();
                
            TransactionData.transactionsList[index] = new TransactionDto(
                id,
                updatedTransaction.Amount,
                updatedTransaction.Date,
                updatedTransaction.CategoryId,
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
