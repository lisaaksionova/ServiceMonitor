namespace ServiceMonitor.Application.Incidents.Dtos;

public class IncidentDto
{
    public Guid Id { get; set; }
    public string Date { get; set; }
    public string ResolvedAt { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
}
