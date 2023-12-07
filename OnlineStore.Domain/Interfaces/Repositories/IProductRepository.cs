using OnlineStore.Domain.Models;
using OnlineStore.Domain.Products.Commands.UpdateProduct;

namespace OnlineStore.Domain.Repositories;
public interface IProductRepository
{
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken);
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> AddAsync(Product product);
    Task UpdateAsync(Guid id, UpdateProductData updateData);
    Task DeleteAsync(Guid id);
}
