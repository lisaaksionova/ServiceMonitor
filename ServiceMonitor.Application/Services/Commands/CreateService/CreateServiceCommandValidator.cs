using FluentValidation;

namespace ServiceMonitor.Application.Services.Commands.CreateService;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(service => service.Name)
            .NotNull()
            .NotEmpty()
            .WithMessage("Name is required");
        RuleFor(service => service.Endpoint)
            .NotNull()
            .NotEmpty()
            .WithMessage("Endpoint is required");
        RuleFor(service => service.Endpoint)
            .Matches(@"^https?:\/\/[^\s/$.?#].[^\s]*$")
            .WithMessage("Only correct http/https endpoint are supported");
        
    }
}