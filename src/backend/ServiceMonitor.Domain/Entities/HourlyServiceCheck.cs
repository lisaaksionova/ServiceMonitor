using System.ComponentModel.DataAnnotations;

namespace ServiceMonitor.Domain.Entities;

public class HourlyServiceCheck
{
    public Guid Id { get; set; }
    public Guid ServiceId { get; set; }
    public DateTime Hour { get; set; }
    public int TotalChecks { get; set; }
    public int SuccessfulChecks { get; set; }
    public int FailedChecks { get; set; }
    public double AverageResponseTimeMs { get; set; }
    public double MinResponseTimeMs { get; set; }
    public double MaxResponseTimeMs { get; set; }
}
