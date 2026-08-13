using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommandHandler(IRepositoryManager repository,
    ILogger<DeleteIncidentCommandHandler> logger,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<DeleteIncidentCommand>
{
    public async Task Handle(DeleteIncidentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Deleting incident {IncidentId}", request.IncidentId);
        var service =
            await repository.Service.GetByIdAsync(request.ServiceId, authenticatedUser.UserId, cancellationToken)
            ?? throw new ServiceNotFoundException(request.ServiceId);
        var incident =
            await repository.Incident.GetByIdAsync(service.Id, request.IncidentId, cancellationToken)
            ?? throw new IncidentNotFoundException(request.IncidentId);
        await repository.Incident.DeleteAsync(incident, cancellationToken);
    }
}
