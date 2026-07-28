using MediatR;

namespace ServiceMonitor.Application.Services.Commands.CreateService;

public class CreateServiceCommand : IRequest
{
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public int CheckIntervalMinutes { get; set; }
}
