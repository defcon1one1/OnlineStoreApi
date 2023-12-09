using FluentValidation;

namespace OnlineStore.Domain.Users.Commands.LoginCommand;
internal class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(c => c.LoginRequest.Email).EmailAddress().WithMessage("The email address you provided is not a valid email address.");
    }
}
