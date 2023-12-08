namespace OnlineStore.Domain.Users.Commands.LoginCommand;
public class LoginResponse
{
    public bool Success { get; set; }
    public Guid? UserId { get; private set; }
    public string? Token { get; private set; }

    public LoginResponse(bool isSuccess)
    {
        Success = isSuccess;
    }
    public LoginResponse(bool isSuccess, Guid? userId, string? token)
    {
        Success = isSuccess;
        UserId = userId;
        Token = token;
    }
}
