
namespace OnlineStore.Domain.Models;
public class User(Guid id, string email, string passwordHash, UserRole role)
{
    public Guid Id { get; private set; } = id;
    public string Email { get; private set; } = email;
    public string PasswordHash { get; private set; } = passwordHash;
    public UserRole Role { get; private set; } = role;
}

public enum UserRole
{
    Customer,
    Employee,
    Admin
}
