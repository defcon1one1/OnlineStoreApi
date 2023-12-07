using OnlineStore.Domain.Models;

namespace OnlineStore.Domain.Repositories;
public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id);
}
