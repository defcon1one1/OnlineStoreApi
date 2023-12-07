using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Products.Commands.UpdateProduct;
using OnlineStore.Domain.Repositories;
using OnlineStore.Infrastructure.Entities;
using OnlineStore.Infrastructure.Exceptions;

namespace OnlineStore.Infrastructure.DAL.Repositories;
public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Product>> GetAllAsync()
    {
        List<ProductEntity> productEntities = await _dbContext.Products.ToListAsync();
        return productEntities.Select(productEntity => productEntity.ToProduct()).ToList();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        ProductEntity? productEntity = await GetEntityByIdAsync(id);
        return productEntity?.ToProduct();
    }
    public async Task AddAsync(Product product)
    {
        try
        {
            ProductEntity productEntity = ProductEntity.FromProduct(product);
            await _dbContext.Products.AddAsync(productEntity);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationException($"Add product operation failed: {ex.Message}");
        }
    }
    public async Task DeleteAsync(Guid id)
    {
        ProductEntity? productEntity = await GetEntityByIdAsync(id) ?? throw new DatabaseOperationException("Delete product operation failed: product not found.");
        _dbContext.Remove(productEntity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task UpdateAsync(Guid id, UpdateProductData updateData)
    {
        ProductEntity? productEntity = await GetEntityByIdAsync(id) ?? throw new DatabaseOperationException("Update operation failed: product not found.");
        if (updateData is null) throw new DatabaseOperationException("Update operation failed: update data is null.");

        productEntity.Price = updateData.Price;
        productEntity.Description = updateData.Description;
        productEntity.Name = updateData.Name;
        _dbContext.Products.Update(productEntity);
        await _dbContext.SaveChangesAsync();
    }

    private async Task<ProductEntity?> GetEntityByIdAsync(Guid id)
    {
        return await _dbContext.Products.FindAsync(id);
    }
}
