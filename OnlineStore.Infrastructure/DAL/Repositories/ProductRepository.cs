﻿using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Infrastructure.DAL.Repositories;
public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public Task<Product?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }
}
