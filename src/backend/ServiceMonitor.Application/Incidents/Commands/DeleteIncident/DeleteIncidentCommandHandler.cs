using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommandHandler(IIncidentRepository incidentRepository,
    ILogger<DeleteIncidentCommandHandler> logger,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<DeleteIncidentCommand>
{
    public async Task Handle(DeleteIncidentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting incident {IncidentId}", request.IncidentId);
        var incident = await incidentRepository.GetByIdAsync(request.ServiceId, request.IncidentId, authenticatedUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Incident), request.IncidentId.ToString());
        await incidentRepository.DeleteAsync(incident, cancellationToken);
    }
}
