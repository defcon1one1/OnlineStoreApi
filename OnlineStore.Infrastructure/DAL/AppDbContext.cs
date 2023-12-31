﻿using Microsoft.EntityFrameworkCore;
using OnlineStore.Infrastructure.Entities;
using System.Reflection;

namespace OnlineStore.Infrastructure.DAL;

public class AppDbContext : DbContext
{
    public DbSet<TransactionEntity> Transactions { get; set; }
    public DbSet<ProductEntity> Products { get; set; }
    public DbSet<UserEntity> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    public AppDbContext()
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseInMemoryDatabase("OnlineStoreDb");
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}