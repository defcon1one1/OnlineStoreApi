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
    public bool VerifyLogin(string email, string passwordHash, out Guid id)
    {
        UserEntity? user = _dbContext.Users
            .FirstOrDefault(u => u.Email == email && u.PasswordHash == passwordHash);
        id = user is null ? Guid.Empty : user.Id;
        return user is not null;
    }
}
