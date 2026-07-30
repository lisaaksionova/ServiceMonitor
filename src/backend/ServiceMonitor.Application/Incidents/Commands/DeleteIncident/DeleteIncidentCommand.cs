using MediatR;

namespace ServiceMonitor.Application.Incidents.Commands.DeleteIncident;

public class DeleteIncidentCommand(int incidentId) : IRequest
{
    public int IncidentId { get; set; } = incidentId;
}
