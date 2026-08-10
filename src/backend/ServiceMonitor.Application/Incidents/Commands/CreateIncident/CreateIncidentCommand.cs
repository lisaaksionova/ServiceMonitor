using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;

namespace ServiceMonitor.Application.Incidents.Commands.CreateIncident;

public class CreateIncidentCommand : IRequest<IncidentDto>
{
    public string Description { get; set; } = string.Empty;

    public Guid ServiceId { get; set; }
}
