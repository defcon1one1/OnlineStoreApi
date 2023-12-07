using OnlineStore.Domain.Models;

namespace OnlineStore.Domain.Repositories;
public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id);
}
