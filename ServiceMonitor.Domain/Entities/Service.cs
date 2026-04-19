using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Domain.Entities;

public class Service
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public ServiceStatus Status { get; set; }
    public int CheckIntervalMinutes { get; set; }
    public DateTime NextCheckAt { get; set; }

    public List<Incident> Incidents { get; set; } = new();
}