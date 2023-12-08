using System.Security.Cryptography;
using System.Text;

namespace OnlineStore.Domain.Services;
public interface IPasswordHasherService
{
    string GenerateHash(string password);
}
public class PasswordHasherService : IPasswordHasherService
{
    public string GenerateHash(string password)
    {
        byte[] hashedBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        StringBuilder builder = new();
        for (int i = 0; i < hashedBytes.Length; i++)
        {
            builder.Append(hashedBytes[i].ToString("x2"));
        }

        return builder.ToString();
    }
}
