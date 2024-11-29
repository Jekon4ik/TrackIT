using System;
using Microsoft.EntityFrameworkCore;
using TrackIT.Api.Entities;

namespace TrackIT.Api.Data;

public class TrackITContext(DbContextOptions<TrackITContext> options) : DbContext(options)
{
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<CategoryType> CategoryTypes => Set<CategoryType>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CategoryType>().HasData(
            new {Id = 1, Name = "Outcome"},
            new {Id = 2, Name = "Income" }
        );
    }
}
