using MediatR;

namespace ServiceMonitor.Application.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommand(Guid serviceId, Guid incidentId) : IRequest
{
    public Guid ServiceId { get; set; } = serviceId;
    public Guid IncidentId { get; set; } = incidentId;
}
