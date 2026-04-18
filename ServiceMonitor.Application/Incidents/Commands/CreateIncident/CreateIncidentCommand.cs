using MediatR;

namespace ServiceMonitor.Application.Incidents.Commands.CreateIncident;

public class CreateIncidentCommand : IRequest
{
    public string Description { get; set; } = string.Empty;
    
    public int ServiceId { get; set; }
}