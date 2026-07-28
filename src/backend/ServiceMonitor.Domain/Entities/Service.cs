using System.ComponentModel.DataAnnotations;
using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Domain.Entities;

public class Service
{
    public int Id { get; set; }

    [MaxLength(200)] public string Name { get; set; } = string.Empty;

    [MaxLength(500)] public string Endpoint { get; set; } = string.Empty;

    public ServiceStatus Status { get; set; }
    public int CheckIntervalMinutes { get; set; }
    public DateTime NextCheckAt { get; set; }

    public List<Incident> Incidents { get; set; } = new();

    public string UserId { get; set; } = null!;
    public User User { get; set; } = null!;
}
