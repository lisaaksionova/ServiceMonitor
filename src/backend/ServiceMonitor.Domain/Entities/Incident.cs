using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Domain.Entities;

public class Incident
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public IncidentStatus Status { get; set; }
    public DateTime ResolvedAt { get; set; }

    public Guid ServiceId { get; set; }
    public Service Service { get; set; }
}
