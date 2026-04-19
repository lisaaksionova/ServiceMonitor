using MediatR;

namespace ServiceMonitor.Application.Services.Commands.CreateService;

public class CreateServiceCommand : IRequest
{
    public string Name { get; set; }
    public string Endpoint { get; set; }
    public int CheckIntervalMinutes { get; set; }
}