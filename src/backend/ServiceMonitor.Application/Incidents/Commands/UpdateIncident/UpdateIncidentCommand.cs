using MediatR;

namespace ServiceMonitor.Application.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommand(int id, string description, string status) : IRequest
{
    public int Id { get; set; } = id;
    public string? Description { get; set; } = description;
    public string? Status { get; set; } =  status;
}
