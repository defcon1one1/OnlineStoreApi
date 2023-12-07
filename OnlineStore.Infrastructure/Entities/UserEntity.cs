using OnlineStore.Domain.Models;

namespace OnlineStore.Infrastructure.Entities;
public class UserEntity
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public UserRole Role { get; set; } = UserRole.Customer;
    public User ToUser()
    {
        return new User(Id, Email, PasswordHash, Role);
    }
    public static UserEntity FromUser(User user)
    {
        return new UserEntity
        {
            Id = user.Id,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            Role = user.Role
        };
    }
}
