using MediatR;
using ServiceMonitor.Application.Services.Dtos;

namespace ServiceMonitor.Application.Services.Commands.UpdateService;

public class UpdateServiceCommand : IRequest<ServiceDto>
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Endpoint { get; set; }
    public int? CheckIntervalMinutes { get; set; }
}
