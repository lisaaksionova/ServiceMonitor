using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;

namespace ServiceMonitor.Application.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQuery(Guid serviceId, Guid id) : IRequest<IncidentDto>
{
    public Guid ServiceId { get; set; } = serviceId;
    public Guid Id { get; set; } = id;
}
