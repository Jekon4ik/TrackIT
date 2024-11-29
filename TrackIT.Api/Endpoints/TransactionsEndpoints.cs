using System;
using TrackIT.Api.Data;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;
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

        group.MapPost("/", (CreateTransactionDto newTransaction, TrackITContext dbContext) => 
            {
                Transaction transaction= new(){
                    Amount = newTransaction.Amount,
                    Date = newTransaction.Date,
                    CategoryId = newTransaction.CategoryId,
                    Category = dbContext.Categories.Find(newTransaction.CategoryId),
                    Description = newTransaction.Description
                };
                dbContext.Transactions.Add(transaction);
                dbContext.SaveChanges();    

                TransactionDto transactionDto = new(
                    transaction.Id,
                    transaction.Amount,
                    transaction.Date,
                    transaction.Category!.Id,
                    transaction.Description
                );      

            return Results.CreatedAtRoute(GetTransactionEndpointName, new{id = transaction.Id}, transactionDto);
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
