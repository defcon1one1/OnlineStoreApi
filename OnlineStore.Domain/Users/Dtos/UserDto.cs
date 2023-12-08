using OnlineStore.Domain.Models;

namespace OnlineStore.Domain.Users.Dtos;
public class UserDto
{
    public Guid Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public List<Transaction> Transactions = [];
    public static UserDto FromUser(User? user)
    {
        if (user is not null) return new UserDto() { Id = user.Id, Email = user.Email };
        else return new UserDto() { Id = Guid.Empty, Email = string.Empty, Transactions = [] };
    }
}
