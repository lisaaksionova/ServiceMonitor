using MediatR;
using ServiceMonitor.Application.Services.Dtos;

namespace ServiceMonitor.Application.Services.Commands.CreateService;

public class CreateServiceCommand : IRequest<ServiceDto>
{
    public string Name { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public int CheckIntervalMinutes { get; set; }
}
