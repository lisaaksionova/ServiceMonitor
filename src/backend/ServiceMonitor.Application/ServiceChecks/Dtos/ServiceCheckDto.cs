namespace ServiceMonitor.Application.ServiceChecks.Dtos;

public class ServiceCheckDto
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public string CheckedAt { get; set; } = null!;
    public string Status { get; set; } = null!;
    public int? StatusCode { get; set; }
    public double ResponseTimeSeconds { get; set; }
    public string? FailureReason { get; set; }
}
