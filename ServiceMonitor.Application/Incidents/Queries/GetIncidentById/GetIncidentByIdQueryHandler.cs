using AutoMapper;
using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQueryHandler(IIncidentRepository repository,
    IMapper mapper) : IRequestHandler<GetIncidentByIdQuery, IncidentDto>
{
    public async Task<IncidentDto> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    {
        var incident = await repository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(Incident), request.Id.ToString());
        var incidentDto = mapper.Map<IncidentDto>(incident);
        return incidentDto;
    }
}