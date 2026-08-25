using FluentValidation;

namespace ServiceMonitor.Application.Auth.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Email is required");

        RuleFor(u => u.Password)
            .NotEmpty()
            .WithMessage("Password is required");
    }
}
