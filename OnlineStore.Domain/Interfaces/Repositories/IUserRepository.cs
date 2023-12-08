using OnlineStore.Domain.Models;

namespace OnlineStore.Domain.Repositories;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    bool VerifyLogin(string email, string passwordHash, out Guid id);
}
