namespace ServiceMonitor.Application.Services.Dtos;

public class ServiceDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Endpoint { get; set; }
    public string Status { get; set; }
}