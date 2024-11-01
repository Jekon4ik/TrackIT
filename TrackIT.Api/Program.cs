using TrackIT.Api.Dtos;
using TrackIT.Api.Data;
using Microsoft.AspNetCore.Mvc;
using System.Transactions;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

const string GetTransactionEndpointName = "GetTransaction";
const string GetCategoryEndpointName = "GetCategory";

app.MapGet("/", ()=> "Welcome to TracIT");

// Get All
app.MapGet("transactions", ()=> TransactionData.transactionsList).WithName(GetTransactionEndpointName);
app.MapGet("categories", ()=> CategoryData.categoriesList).WithName(GetCategoryEndpointName);

// Get by ID  
app.MapGet("transactions/{id}", (int id)=> TransactionData.transactionsList.Find(transaction=>transaction.Id == id));
app.MapGet("categories/{id}", (int id)=> CategoryData.categoriesList.Find(category=>category.Id == id));

// Create new instance 
app.MapPost("transactions", (CreateTransactionDto newTransaction) => 
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

app.MapPost("categories", (CreateCategoryDto newCategory) => 
{
    CategoryDto category = new (
        CategoryData.categoriesList.Count() +1,
        newCategory.Name,
        newCategory.Type
    );
    CategoryData.categoriesList.Add(category);
    return Results.CreatedAtRoute(GetCategoryEndpointName, new{id = category.Id}, category);
});

// Update instance
app.MapPut("transactions/{id}", (int id, UpdateTransactionDto updatedTransaction) => {
    var index = TransactionData.transactionsList.FindIndex(transaction => transaction.Id == id);
    TransactionData.transactionsList[index] = new TransactionDto(
        id,
        updatedTransaction.Amount,
        updatedTransaction.Date,
        updatedTransaction.CategoryId,
        updatedTransaction.Description
    );
    return Results.NoContent();
});

app.MapPut("categories/{id}", (int id, UpdateCategoryDto updatedCategory) =>
{
    var index = CategoryData.categoriesList.FindIndex(category=> category.Id==id);
    CategoryData.categoriesList[index] = new CategoryDto(
        id,
        updatedCategory.Name,
        updatedCategory.Type
    );
    return Results.NoContent();
});


// Delete instance
app.MapDelete("transactions/{id}", (int id) => {
    TransactionData.transactionsList.RemoveAll(transaction => transaction.Id==id);
    
    return Results.NoContent();
});

app.MapDelete("categories/{id}", (int id) => {
    CategoryData.categoriesList.RemoveAll(category => category.Id==id);
    
    return Results.NoContent();
});

app.Run();
