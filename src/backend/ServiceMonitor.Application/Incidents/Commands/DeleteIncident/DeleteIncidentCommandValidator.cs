using FluentValidation;
using ServiceMonitor.Application.Services.Commands.DeleteService;

namespace ServiceMonitor.Application.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommandValidator : AbstractValidator<DeleteServiceCommand>
{
    public DeleteIncidentCommandValidator()
    {
        RuleFor(i => i.Id)
            .NotNull()
            .WithMessage("Id cannot be null");
    }
}
