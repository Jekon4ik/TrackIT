using System;
using Microsoft.EntityFrameworkCore;
using TrackIT.Api.Data;
using TrackIT.Api.Dtos;
using TrackIT.Api.Entities;
using TrackIT.Api.Mapping;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using TrackIT.Api.Configuration;

namespace TrackIT.Api.Endpoints;

public static class TransactionsEndpoints
{
    const string GetTransactionEndpointName = "GetTransaction";
    const string TransactionsCacheKey = "Transactions";

    public static RouteGroupBuilder MapTransactionsEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("transactions").WithParameterValidation().WithTags("Transactions");

        group.MapGet("/", async (
            TrackITContext dbContext,
            IMemoryCache cache,
            IOptions<ApiSettings> apiSettings,
            IOptions<CacheSettings> cacheSettings,
            ILogger<TrackITContext> logger) =>
        {
            if (!cacheSettings.Value.EnableCaching)
            {
                logger.LogInformation("Getting list of transactions.");
                var transactions = await dbContext.Transactions
                    .Include(transaction => transaction.Category)
                    .Select(transaction => transaction.toTransactionSummaryDto())
                    .AsNoTracking()
                    .Take(apiSettings.Value.MaxTransactionsPerRequest)
                    .ToListAsync();

                return transactions;
            }

            List<TransactionSummaryDto> cachedTransactions = new();
            if (!cache.TryGetValue(TransactionsCacheKey, out cachedTransactions))
            {
                logger.LogInformation("Cache miss for transactions. Fetching from database.");
                cachedTransactions = await dbContext.Transactions
                    .Include(transaction => transaction.Category)
                    .Select(transaction => transaction.toTransactionSummaryDto())
                    .AsNoTracking()
                    .Take(apiSettings.Value.MaxTransactionsPerRequest)
                    .ToListAsync();

                var cacheOptions = new MemoryCacheEntryOptions()
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(cacheSettings.Value.TransactionsExpirationMinutes))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(cacheSettings.Value.SlidingExpirationMinutes));

                cache.Set(TransactionsCacheKey, cachedTransactions, cacheOptions);
                logger.LogInformation("Transactions cached successfully.");
            }
            else
            {
                logger.LogInformation("Cache hit for transactions.");
            }

            return cachedTransactions;
        });

        group.MapGet("/{id}", async (
            int id,
            TrackITContext dbContext,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings,
            ILogger<TrackITContext> logger) =>
        {
            if (!cacheSettings.Value.EnableCaching)
            {
                logger.LogInformation("Getting transaction with ID {Id}.", id);
                var transaction = await dbContext.Transactions.FindAsync(id);
                if (transaction is null)
                {
                    logger.LogWarning("Transaction with ID {Id} not found.", id);
                    return Results.NotFound();
                }
                return Results.Ok(transaction.toTransactionDetailsDto());
            }

            TransactionDetailsDto? cachedTransaction = null;
            string cacheKey = $"{GetTransactionEndpointName}_{id}";

            if (!cache.TryGetValue(cacheKey, out cachedTransaction))
            {
                logger.LogInformation("Cache miss for transaction with ID {Id}. Fetching from database.", id);
                var transaction = await dbContext.Transactions.FindAsync(id);

                if (transaction is null)
                {
                    logger.LogWarning("Transaction with ID {Id} not found.", id);
                    return Results.NotFound();
                }

                cachedTransaction = transaction.toTransactionDetailsDto();
                cache.Set(cacheKey, cachedTransaction, TimeSpan.FromMinutes(cacheSettings.Value.TransactionsExpirationMinutes));
                logger.LogInformation("Transaction with ID {Id} cached successfully.", id);
            }
            else
            {
                logger.LogInformation("Cache hit for transaction with ID {Id}.", id);
            }

            return cachedTransaction is not null 
                ? Results.Ok(cachedTransaction) 
                : Results.NotFound();
        });

        group.MapPost("/", (
            CreateTransactionDto newTransaction,
            TrackITContext dbContext,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings,
            ILogger<TrackITContext> logger) =>
        {
            logger.LogInformation("Creating new transaction.");
            var transaction = newTransaction.toEntity();
            dbContext.Transactions.Add(transaction);
            dbContext.SaveChanges();

            if (cacheSettings.Value.EnableCaching)
            {
                cache.Remove(TransactionsCacheKey);
                logger.LogInformation("Cache invalidated after creating a transaction.");
            }

            return Results.CreatedAtRoute(GetTransactionEndpointName, new { id = transaction.Id }, transaction.toTransactionDetailsDto());
        });

        group.MapPut("/{id}", (
            int id,
            UpdateTransactionDto updatedTransaction,
            TrackITContext dbContext,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings,
            ILogger<TrackITContext> logger) =>
        {
            logger.LogInformation("Updating transaction with ID {Id}.", id);
            var existingTransaction = dbContext.Transactions.Find(id);

            if (existingTransaction is null)
            {
                logger.LogWarning("Transaction with ID {Id} not found for update.", id);
                return Results.NotFound();
            }

            dbContext.Entry(existingTransaction).CurrentValues.SetValues(updatedTransaction.toEntity(id));
            dbContext.SaveChanges();

            if (cacheSettings.Value.EnableCaching)
            {
                cache.Remove(TransactionsCacheKey);
                cache.Remove($"{GetTransactionEndpointName}_{id}");
                logger.LogInformation("Cache invalidated after updating transaction with ID {Id}.", id);
            }

            return Results.NoContent();
        });

        group.MapDelete("/{id}", (
            int id,
            TrackITContext dbContext,
            IMemoryCache cache,
            IOptions<CacheSettings> cacheSettings,
            ILogger<TrackITContext> logger) =>
        {
            logger.LogInformation("Deleting transaction with ID {Id}.", id);
            dbContext.Transactions.Where(transaction => transaction.Id == id).ExecuteDelete();

            if (cacheSettings.Value.EnableCaching)
            {
                cache.Remove(TransactionsCacheKey);
                cache.Remove($"{GetTransactionEndpointName}_{id}");
                logger.LogInformation("Cache invalidated after deleting transaction with ID {Id}.", id);
            }

            return Results.NoContent();
        });

        return group;
    }
}
