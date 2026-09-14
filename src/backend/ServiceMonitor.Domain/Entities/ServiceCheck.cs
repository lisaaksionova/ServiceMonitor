using System.Net;
using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Domain.Entities;

public class ServiceCheck
{
    public Guid Id { get; set; }

    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = null!;

    public DateTime CheckedAt { get; set; }

    public ServiceStatus Status { get; set; }

    public HttpStatusCode? StatusCode { get; set; }

    public long ResponseTimeMs { get; set; }

    public string? FailureReason { get; set; }
}
