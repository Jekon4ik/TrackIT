using System;
using Microsoft.EntityFrameworkCore;
using TrackIT.Api.Entities;

namespace TrackIT.Api.Data;

public class TrackITContext(DbContextOptions<TrackITContext> options) : DbContext(options)
{
    public DbSet<Category> Category => Set<Category>();
    public DbSet<CategoryType> CategoryTypes => Set<CategoryType>();
    public DbSet<Transaction> Transaction => Set<Transaction>();
}
