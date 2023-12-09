using OnlineStore.Domain.Models;
using OnlineStore.Domain.Interfaces.Repositories;
using OnlineStore.Infrastructure.Entities;

namespace OnlineStore.Infrastructure.DAL.Repositories;
public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        UserEntity? userEntity = await _dbContext.Users.FindAsync([id], cancellationToken: cancellationToken);
        return userEntity?.ToUser();
    }
    public bool VerifyLogin(string email, string passwordHash, out Guid id)
    {
        UserEntity? user = _dbContext.Users
            .FirstOrDefault(u => u.Email == email && u.PasswordHash == passwordHash);
        id = user is null ? Guid.Empty : user.Id;
        return user is not null;
    }

    public List<Transaction> GetUserTransactions(Guid id)
    {
        List<TransactionEntity> transactionEntities = [.. _dbContext.Transactions.Where(t => t.UserId == id)];
        return transactionEntities.Select(t => t.ToTransaction()).ToList();
    }
}
