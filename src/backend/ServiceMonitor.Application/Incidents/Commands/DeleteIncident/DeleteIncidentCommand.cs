using MediatR;

namespace ServiceMonitor.Application.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommand(Guid incidentId) : IRequest
{
    public Guid IncidentId { get; set; } = incidentId;
}
