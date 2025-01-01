using TrackIT.Api.Data;
using TrackIT.Api.Endpoints;
using FluentValidation.AspNetCore;
using TrackIT.Api.Data.Validation;
using FluentValidation;
using System.Reflection;
using TrackIT.Api.Dtos;
using TrackIT.Api.Configuration;
var builder = WebApplication.CreateBuilder(args);

// Додавання логування
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

builder.Services.AddTransient<IValidator<CreateCategoryDto>, CreateCategoryDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateCategoryDto>, UpdateCategoryDtoValidator>();
builder.Services.AddTransient<IValidator<CreateTransactionDto>, CreateTransactionDtoValidator>();
builder.Services.AddTransient<IValidator<UpdateTransactionDto>, UpdateTransactionDtoValidator>();

var configuration = builder.Configuration;

// Доступ до параметрів кешування
var cacheExpiration = configuration.GetValue<int>("CacheSettings:TransactionsExpirationMinutes");
var slidingExpiration = configuration.GetValue<int>("CacheSettings:SlidingExpirationMinutes");
var enableCaching = configuration.GetValue<bool>("CacheSettings:EnableCaching");

// Доступ до функціональних налаштувань
var maxTransactions = configuration.GetValue<int>("ApiSettings:MaxTransactionsPerRequest");

builder.Services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
builder.Services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));


var connString = builder.Configuration.GetConnectionString("TrackIT");

builder.Services.AddSqlite<TrackITContext>(connString);

var app = builder.Build();

app.MapCategoriesEndpoints();

app.MapTransactionsEndpoints();

app.MigrateDb();

app.UseSwagger();
app.UseSwaggerUI();
app.Run();