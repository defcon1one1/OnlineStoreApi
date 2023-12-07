using OnlineStore.Domain.Models;

namespace OnlineStore.Domain.Repositories;
public interface ITransactionRepository
{
    Task<List<Transaction>> GetAllAsync(CancellationToken cancellationToken);
    Task<Transaction?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<Guid> AddAsync(Transaction transaction);
    Task RejectAsync(Guid id);
    Task AcceptAsync(Guid id);
    Task ReviseAsync(Guid id, decimal price);
}
