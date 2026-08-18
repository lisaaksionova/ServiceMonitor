using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQueryHandler(
    ILogger<GetIncidentByIdQueryHandler> logger,
    IRepositoryManager repository,
    IMapper mapper,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<GetIncidentByIdQuery, IncidentDto>
{
    public async Task<IncidentDto> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting incident {IncidentId} for service {ServiceId}", request.IncidentId, request.ServiceId);

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
        var incidentDto = mapper.Map<IncidentDto>(incident);

        return incidentDto;
    }
}
