using MediatR;

namespace ServiceMonitor.Application.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommand(Guid id, string description, string status, string resolvedAt) : IRequest
{
    public Guid Id { get; set; } = id;
    public Guid ServiceId { get; set; }
    public string? Description { get; set; } = description;
    public string? Status { get; set; } = status;
    public string? ResolvedAt { get; set; } = resolvedAt;
}
