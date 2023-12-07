using OnlineStore.Domain.Models;

namespace OnlineStore.Domain.Repositories;
public interface ITransactionRepository
{
    Task<Transaction?> GetByIdAsync(Guid id);
    Task Add(Transaction transaction);
    Task UpdateAsync(Transaction transaction);
    Task DeleteAsync(Transaction transaction);
}
