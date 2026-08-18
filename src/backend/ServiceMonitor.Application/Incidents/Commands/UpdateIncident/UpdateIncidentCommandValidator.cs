using FluentValidation;
using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Application.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommandValidator : AbstractValidator<UpdateIncidentCommand>
{
    public UpdateIncidentCommandValidator()
    {
        RuleFor(x => x.IncidentId)
            .NotEmpty().WithMessage("IncidentId is required");
        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required")
            .When(x => x.Description != null)
            .MinimumLength(100).WithMessage("Description must be at least 100 characters long")
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
        RuleFor(x => x.Status)
            .NotEmpty().WithMessage("Status is required")
            .When(x => x.Status != null)
            .Must(status => status == null ||
                             Enum.TryParse<IncidentStatus>(status, true, out _))
                .WithMessage("Status should only be in [Open, Investigation, Closed]");
    }
}
