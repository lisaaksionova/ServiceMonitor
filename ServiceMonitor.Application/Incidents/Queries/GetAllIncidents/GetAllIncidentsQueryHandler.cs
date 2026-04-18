using AutoMapper;
using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Queries.GetAllIncidents;

public class GetAllIncidentsQueryHandler(IIncidentRepository repository,
    IMapper mapper) : IRequestHandler<GetAllIncidentsQuery, IEnumerable<IncidentDto>>
{
    public async Task<IEnumerable<IncidentDto>> Handle(GetAllIncidentsQuery request, CancellationToken cancellationToken)
    {
        var incidents = await repository.GetAllAsync();
        var incidentDtos = mapper.Map<IEnumerable<IncidentDto>>(incidents);
        return incidentDtos;
    }
}