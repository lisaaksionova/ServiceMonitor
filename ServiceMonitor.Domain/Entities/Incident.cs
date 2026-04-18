using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Domain.Entities;

public class Incident
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Description { get; set; } = string.Empty;
    public IncidentStatus Status { get; set; }
    
    public int ServiceId { get; set; }
    public Service Service { get; set; }
}