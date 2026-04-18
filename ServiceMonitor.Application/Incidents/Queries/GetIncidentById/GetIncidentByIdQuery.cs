using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;

namespace ServiceMonitor.Application.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQuery(int id) : IRequest<IncidentDto>
{
    public int Id { get; set; } = id;
}