using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Domain.Entities;

public class Service
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "Service name is required.")]
    [MaxLength(100, ErrorMessage = "Service name cannot exceed 100 characters.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Endpoint is required.")]
    [MaxLength(100, ErrorMessage = "Endpoint cannot exceed 100 characters.")]
    public string Endpoint { get; set; } = string.Empty;

    public ServiceStatus Status { get; set; }
    public int CheckIntervalMinutes { get; set; }
    public DateTime NextCheckAt { get; set; }
    public DateTime LastCheckAt { get; set; }
    public DateTime LastSuccessfulCheckAt { get; set; }
    [MaxLength(200, ErrorMessage = "Failure reason cannot exceed 200 characters.")]
    public string? LastFailureReason { get; set; } = string.Empty;

    public List<Incident> Incidents { get; set; } = null!;

    [ForeignKey(nameof(User))]
    public string UserId { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}
