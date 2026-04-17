using MediatR;

namespace ServiceMonitor.Application.Services.Commands.UpdateService;

public class UpdateServiceCommand: IRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Endpoint { get; set; }
    public string Status { get; set; }
}