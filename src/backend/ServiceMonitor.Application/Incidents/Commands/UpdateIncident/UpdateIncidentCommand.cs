using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;

namespace ServiceMonitor.Application.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommand(Guid incidentId, string description, string status, string resolvedAt) : IRequest<IncidentDto>
{
    public Guid IncidentId { get; set; } = incidentId;
    public Guid ServiceId { get; set; }
    public string? Description { get; set; } = description;
    public string? Status { get; set; } = status;
    public string? ResolvedAt { get; set; } = resolvedAt;
}
