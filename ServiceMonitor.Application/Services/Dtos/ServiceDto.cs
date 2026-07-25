using ServiceMonitor.Application.Incidents.Dtos;

namespace ServiceMonitor.Application.Services.Dtos;

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public List<IncidentDto> Incidents { get; set; } = new();
}
