using FluentValidation;

namespace ServiceMonitor.Application.Incidents.Commands.CreateIncident;

public class CreateIncidentCommandValidator : AbstractValidator<CreateIncidentCommand>
{
    public CreateIncidentCommandValidator()
    {
        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(100)
            .MaximumLength(1000)
            .WithMessage("Description is required.");
        RuleFor(x => x.ServiceId)
            .NotNull()
            .WithMessage("ServiceId is required.");
    }
}
