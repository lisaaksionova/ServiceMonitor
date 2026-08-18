using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommandHandler(IRepositoryManager repository,
    ILogger<UpdateIncidentCommandHandler> logger,
    IAuthenticatedUser authenticatedUser,
    IMapper mapper) : IRequestHandler<UpdateIncidentCommand, IncidentDto>
{
    public async Task<IncidentDto> Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating incident {@Incident}", request.Description);

        var openIncidents = await repository.Incident.GetAllOpenAsync(request.ServiceId, cancellationToken);
        if (openIncidents.Any() && Enum.TryParse<IncidentStatus>(
                                    request.Status,
                                    ignoreCase: true,
                                    out var status)
                                && status == IncidentStatus.Open)
        {
            logger.LogError("Incident {IncidentId} is already opened", request.IncidentId);
            throw new InvalidOperationException("Cannot create new open incident. Resolve previous.");
        }
        var service = await repository.Service.GetByIdAsync(request.ServiceId, authenticatedUser.UserId, cancellationToken);
        if (service == null)
        {
            logger.LogError("Service {ServiceId} for incident {IncidentId} is not found.", request.ServiceId, request.IncidentId);
            throw new ServiceNotFoundException(request.ServiceId);
        }
        var incident = await repository.Incident.GetByIdAsync(service.Id, request.IncidentId, cancellationToken);
        if (incident == null)
        {
            logger.LogError("Incident {IncidentId} is not found.", request.IncidentId);
            throw new IncidentNotFoundException(request.IncidentId);
        }
        mapper.Map(request, incident);
        await repository.Incident.SaveAsync(cancellationToken);
        return mapper.Map<IncidentDto>(incident);
    }
}
