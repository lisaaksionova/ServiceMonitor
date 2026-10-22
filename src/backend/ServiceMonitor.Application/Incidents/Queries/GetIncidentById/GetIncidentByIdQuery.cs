namespace ServiceMonitor.Application.Incidents.Queries.GetIncidentById;

public class GetIncidentByIdQuery(Guid serviceId, Guid incidentId) : IRequest<IncidentDto>
{
    public Guid ServiceId { get; set; } = serviceId;
    public Guid IncidentId { get; set; } = incidentId;
}
