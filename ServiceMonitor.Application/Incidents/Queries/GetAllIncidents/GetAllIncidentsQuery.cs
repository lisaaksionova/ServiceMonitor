using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;

namespace ServiceMonitor.Application.Incidents.Queries.GetAllIncidents;

public class GetAllIncidentsQuery : IRequest<IEnumerable<IncidentDto>>
{
    
}