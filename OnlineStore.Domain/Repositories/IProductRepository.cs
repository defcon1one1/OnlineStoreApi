using OnlineStore.Domain.Models;
using OnlineStore.Domain.Products.Commands.UpdateProduct;

namespace OnlineStore.Domain.Repositories;
public interface IProductRepository
{
    Task<List<Product>> GetAllAsync();
    Task<Product?> GetByIdAsync(Guid id);
    Task UpdateAsync(Guid id, UpdateProductData updateData);
    Task DeleteAsync(Guid id);
}
