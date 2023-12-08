using Microsoft.EntityFrameworkCore;
using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;
using OnlineStore.Infrastructure.Entities;
using OnlineStore.Infrastructure.Exceptions;

namespace OnlineStore.Infrastructure.DAL.Repositories;
public class ProductRepository(AppDbContext dbContext) : IProductRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<List<Product>> GetAllAsync(string searchPhrase, CancellationToken cancellationToken)
    {
        List<ProductEntity> productEntities
            = await _dbContext.Products.Where(p => p.Name.Contains(searchPhrase, StringComparison.CurrentCultureIgnoreCase)).ToListAsync(cancellationToken: cancellationToken);
        return productEntities.Select(productEntity => productEntity.ToProduct()).ToList();
    }

    public async Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        ProductEntity? productEntity = await GetEntityByIdAsync(id);
        return productEntity?.ToProduct();
    }
    public async Task<Guid> AddAsync(Product product)
    {
        ProductEntity productEntity = ProductEntity.FromProduct(product);
        await _dbContext.Products.AddAsync(productEntity);
        await _dbContext.SaveChangesAsync();
        return productEntity.Id;
    }
    public async Task DeleteAsync(Guid id)
    {
        ProductEntity? productEntity = await GetEntityByIdAsync(id) ?? throw new DatabaseOperationException("Delete product operation failed: product not found.");
        _dbContext.Remove(productEntity);
        await _dbContext.SaveChangesAsync();
    }
    public async Task UpdateAsync(Product newProduct)
    {
        ProductEntity? currentProduct = await GetEntityByIdAsync(newProduct.Id) ?? throw new DatabaseOperationException("Update operation failed: product not found.");
        try
        {
            currentProduct.Price = newProduct.Price;
            currentProduct.Name = newProduct.Name;
            currentProduct.Description = newProduct.Description;
            _dbContext.Products.Update(currentProduct);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new DatabaseOperationException($"Update operation failed: {ex.Message}");
        }
    }

    private async Task<ProductEntity?> GetEntityByIdAsync(Guid id)
    {
        return await _dbContext.Products.FindAsync(id);
    }
}
