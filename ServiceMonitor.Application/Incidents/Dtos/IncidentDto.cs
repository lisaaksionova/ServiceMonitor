namespace ServiceMonitor.Application.Incidents.Dtos;

public class IncidentDto
{
    public int Id { get; set; }
    public string Date { get; set; }
    public string Description { get; set; }
    public string Status { get; set; }
}