using OnlineStore.Domain.Models;
using OnlineStore.Domain.Repositories;
using OnlineStore.Infrastructure.Entities;

namespace OnlineStore.Infrastructure.DAL.Repositories;
public class UserRepository(AppDbContext dbContext) : IUserRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<User?> GetByIdAsync(Guid id)
    {
        UserEntity? userEntity = await _dbContext.Users.FindAsync(id);
        return userEntity?.ToUser();
    }
}
