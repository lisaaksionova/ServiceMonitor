using AutoMapper;
using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Queries.GetAllIncidents;

public class GetAllIncidentsQueryHandler(
    IIncidentRepository repository,
    IMapper mapper) : IRequestHandler<GetAllIncidentsQuery, CursorPagedList<IncidentDto>>
{
    public async Task<CursorPagedList<IncidentDto>> Handle(GetAllIncidentsQuery request,
        CancellationToken cancellationToken)
    {
        var incidents = await repository.GetAllPaginatedAsync(request.Cursor, request.Limit, cancellationToken);
        var incidentDtos = new CursorPagedList<IncidentDto>(
            mapper.Map<List<IncidentDto>>(incidents.Items),
            incidents.NextCursor,
            incidents.HasMore);
        return incidentDtos;
    }
}
