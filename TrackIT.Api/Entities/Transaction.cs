using System;

namespace TrackIT.Api.Entities;

public class Transaction
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public DateOnly Date { get; set; }
    public int CategoryId { get; set; }
    public Category? Category { get; set; }
    public string? Description { get; set; }
}
