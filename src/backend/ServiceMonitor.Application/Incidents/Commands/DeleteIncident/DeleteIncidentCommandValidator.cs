using FluentValidation;
using ServiceMonitor.Application.Services.Commands.DeleteService;

namespace ServiceMonitor.Application.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommandValidator : AbstractValidator<DeleteServiceCommand>
{
    public DeleteIncidentCommandValidator()
    {
        RuleFor(i => i.Id)
            .NotNull()
            .WithMessage("IncidentId cannot be null");
    }
}
