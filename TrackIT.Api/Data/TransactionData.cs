using System;
using TrackIT.Api.Dtos;
namespace TrackIT.Api.Data;

public static class TransactionData
{
    public static List<TransactionDto> transactionsList = [
        new TransactionDto(1, 1500.00m, DateOnly.Parse("2024-01-01"), 1, "Monthly salary for January"), // CategoryId 1
            new TransactionDto(2, 600.00m, DateOnly.Parse("2024-01-05"), 2, "Income from freelance graphic design work"), // CategoryId 2
            
            // Expense transactions
            new TransactionDto(3, 100.00m, DateOnly.Parse("2024-01-10"), 3, "Weekly grocery shopping at SuperMart"), // CategoryId 3
            new TransactionDto(4, 75.00m, DateOnly.Parse("2024-01-12"), 4, "Dinner at Olive Garden with friends"), // CategoryId 4
            new TransactionDto(5, 50.00m, DateOnly.Parse("2024-01-15"), 5, "Movie tickets for the latest blockbuster"), // CategoryId 5
            new TransactionDto(6, 200.00m, DateOnly.Parse("2024-01-17"), 6, "Medical check-up at Health Clinic"), // CategoryId 6
            new TransactionDto(7, 30.00m, DateOnly.Parse("2024-01-20"), 7, "Purchase of office supplies from Office Depot"), // CategoryId 7
            new TransactionDto(8, 90.00m, DateOnly.Parse("2024-01-22"), 8, "Monthly bus pass for public transportation"), // CategoryId 8
            new TransactionDto(9, 45.00m, DateOnly.Parse("2024-01-25"), 9, "Birthday gift for a friend"), // CategoryId 9
            new TransactionDto(10, 120.00m, DateOnly.Parse("2024-01-27"), 10, "Home repair supplies from Home Depot"), // CategoryId 10
            new TransactionDto(11, 60.00m, DateOnly.Parse("2024-01-30"), 4, "Dinner at a local Italian restaurant"), // CategoryId 4
            new TransactionDto(12, 80.00m, DateOnly.Parse("2024-02-01"), 5, "Concert tickets for the weekend"), // CategoryId 5
            new TransactionDto(13, 100.00m, DateOnly.Parse("2024-02-03"), 3, "Groceries for the week, including snacks"), // CategoryId 3
            new TransactionDto(14, 150.00m, DateOnly.Parse("2024-02-05"), 6, "Dental check-up and cleaning at Smile Clinic"), // CategoryId 6
            new TransactionDto(15, 40.00m, DateOnly.Parse("2024-02-10"), 7, "Office chair purchased from IKEA"), // CategoryId 7
            new TransactionDto(16, 30.00m, DateOnly.Parse("2024-02-15"), 8, "Taxi fare to the airport"), // CategoryId 8
            new TransactionDto(17, 25.00m, DateOnly.Parse("2024-02-20"), 9, "Gift card for a favorite restaurant"), // CategoryId 9
            new TransactionDto(18, 90.00m, DateOnly.Parse("2024-02-22"), 10, "New curtains for the living room from Walmart"), // CategoryId 10
            new TransactionDto(19, 55.00m, DateOnly.Parse("2024-02-25"), 4, "Lunch at a local café with colleagues"), // CategoryId 4
            new TransactionDto(20, 70.00m, DateOnly.Parse("2024-02-28"), 5, "Subscription for a streaming service"), // CategoryId 5
    ];
}
