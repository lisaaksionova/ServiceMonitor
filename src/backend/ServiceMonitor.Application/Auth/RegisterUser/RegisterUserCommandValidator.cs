using FluentValidation;

namespace ServiceMonitor.Application.Auth.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
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
