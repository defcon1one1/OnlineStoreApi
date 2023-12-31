﻿using OnlineStore.Domain.Models;

namespace OnlineStore.Domain.Interfaces.Repositories;
public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(string searchPhrase, CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> AddAsync(Product product);
    Task UpdateAsync(Product product);
    Task DeleteAsync(Guid id);
    Task<bool> NameExistsAsync(string name);
}
