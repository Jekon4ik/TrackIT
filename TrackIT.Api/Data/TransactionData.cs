using System;
using TrackIT.Api.Dtos;
namespace TrackIT.Api.Data;

public static class TransactionData
{
    public static List<TransactionSummaryDto> transactionsList = [
        new TransactionSummaryDto(1, 1500.00m, DateOnly.Parse("2024-01-01"), "text", "Monthly salary for January"), // CategoryId 1
            new TransactionSummaryDto(2, 600.00m, DateOnly.Parse("2024-01-05"), "text", "Income from freelance graphic design work"), // CategoryId 2
            
            // Expense transactions
            new TransactionSummaryDto(3, 100.00m, DateOnly.Parse("2024-01-10"), "text", "Weekly grocery shopping at SuperMart"), // CategoryId 3
            new TransactionSummaryDto(4, 75.00m, DateOnly.Parse("2024-01-12"), "text", "Dinner at Olive Garden with friends"), // CategoryId 4
            new TransactionSummaryDto(5, 50.00m, DateOnly.Parse("2024-01-15"), "text", "Movie tickets for the latest blockbuster"), // CategoryId 5
            new TransactionSummaryDto(6, 200.00m, DateOnly.Parse("2024-01-17"), "text", "Medical check-up at Health Clinic"), // CategoryId 6
            new TransactionSummaryDto(7, 30.00m, DateOnly.Parse("2024-01-20"), "text", "Purchase of office supplies from Office Depot"), // CategoryId 7
            new TransactionSummaryDto(8, 90.00m, DateOnly.Parse("2024-01-22"), "text", "Monthly bus pass for public transportation"), // CategoryId 8
            new TransactionSummaryDto(9, 45.00m, DateOnly.Parse("2024-01-25"), "text", "Birthday gift for a friend"), // CategoryId 9
            new TransactionSummaryDto(10, 120.00m, DateOnly.Parse("2024-01-27"), "text", "Home repair supplies from Home Depot"), // CategoryId 10
    ];
}
