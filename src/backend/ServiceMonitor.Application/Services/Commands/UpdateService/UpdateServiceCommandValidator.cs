using FluentValidation;
using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Application.Services.Commands.UpdateService;

public class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(service => service.Name)
            .NotEmpty()
            .When(x => x.Name != null)
            .WithMessage("Name cannot be empty");

        RuleFor(service => service.Endpoint)
            .NotEmpty()
            .Matches(@"^https?:\/\/[^\s/$.?#][^\s]*$")
            .When(x => x.Endpoint != null)
            .WithMessage("Only correct http/https endpoint are supported");

        RuleFor(service => service.CheckIntervalMinutes)
            .GreaterThan(0)
            .When(x => x.CheckIntervalMinutes != null)
            .WithMessage("Check interval must be greater than zero");
    }
}
