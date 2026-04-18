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
    
        RuleFor(service => service.Status)
            .NotEmpty()
            .When(x => x.Status != null)
            .Must(status => status == null ||
                            Enum.TryParse<ServiceStatus>(status, true, out _))
            .WithMessage("Status should only be in [Healthy, Degraded, Down]");
    }
}