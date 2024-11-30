using TrackIT.Api.Data;
using TrackIT.Api.Endpoints;
using FluentValidation.AspNetCore;
using TrackIT.Api.Data.Validation;
using FluentValidation;
using System.Reflection;
using TrackIT.Api.Dtos;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddTransient<IValidator<CreateCategoryDto>, CreateCategoryDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateCategoryDto>, UpdateCategoryDtoValidator>();
builder.Services.AddTransient<IValidator<CreateTransactionDto>, CreateTransactionDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateTransactionDto>, UpdateTransactionDtoValidator>();

var connString = builder.Configuration.GetConnectionString("TrackIT");

builder.Services.AddSqlite<TrackITContext>(connString);


var app = builder.Build();


app.MapCategoriesEndpoints();

app.MapTransactionsEndpoints();

app.MigrateDb();

app.UseSwagger();
app.UseSwaggerUI();
app.Run();