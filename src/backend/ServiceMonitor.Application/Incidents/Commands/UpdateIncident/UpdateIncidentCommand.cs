using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;

namespace ServiceMonitor.Application.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommand(Guid id, string description, string status, string resolvedAt) : IRequest<IncidentDto>
{
    public Guid Id { get; set; } = id;
    public Guid ServiceId { get; set; }
    public string? Description { get; set; } = description;
    public string? Status { get; set; } = status;
    public string? ResolvedAt { get; set; } = resolvedAt;
}
