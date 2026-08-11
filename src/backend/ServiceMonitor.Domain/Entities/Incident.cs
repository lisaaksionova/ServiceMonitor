using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ServiceMonitor.Domain.Enums;

namespace ServiceMonitor.Domain.Entities;

public class Incident
{
    public Guid Id { get; set; }
    public DateTime Date { get; set; }

    [Required(ErrorMessage = "Description is required.")]
    [MaxLength(200, ErrorMessage = "Description cannot exceed 200 characters.")]
    public string Description { get; set; } = string.Empty;
    public IncidentStatus Status { get; set; }
    public DateTime ResolvedAt { get; set; }

    [ForeignKey(nameof(Service))]
    public Guid ServiceId { get; set; }
    public Service Service { get; set; } = new();
}
