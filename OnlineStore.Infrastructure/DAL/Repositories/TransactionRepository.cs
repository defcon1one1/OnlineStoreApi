using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;

namespace OnlineStore.Infrastructure.DAL.Repositories;
public class TransactionRepository(AppDbContext dbContext) : ITransactionRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Transaction?> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task Add(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public async Task DeleteAsync(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public async Task UpdateAsync(Transaction transaction)
    {
        throw new NotImplementedException();
    }
}
