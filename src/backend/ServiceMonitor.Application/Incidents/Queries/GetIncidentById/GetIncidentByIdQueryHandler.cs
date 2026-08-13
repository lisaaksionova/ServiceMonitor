using AutoMapper;
using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQueryHandler(
    IRepositoryManager repository,
    IMapper mapper,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<GetIncidentByIdQuery, IncidentDto>
{
    public async Task<IncidentDto> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        var service =
            await repository.Service.GetByIdAsync(request.ServiceId, authenticatedUser.UserId, cancellationToken)
            ?? throw new ServiceNotFoundException(request.ServiceId);
        var incident =
            await repository.Incident.GetByIdAsync(service.Id, request.Id, cancellationToken)
            ?? throw new IncidentNotFoundException(request.Id);
        var incidentDto = mapper.Map<IncidentDto>(incident);

        return incidentDto;
    }
}
