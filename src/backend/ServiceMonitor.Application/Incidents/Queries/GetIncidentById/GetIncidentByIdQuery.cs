using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;

namespace ServiceMonitor.Application.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQuery(Guid id) : IRequest<IncidentDto>
{
    public Guid Id { get; set; } = id;
}
