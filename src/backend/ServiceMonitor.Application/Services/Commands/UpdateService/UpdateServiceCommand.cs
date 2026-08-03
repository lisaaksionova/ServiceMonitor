using MediatR;

namespace ServiceMonitor.Application.Services.Commands.UpdateService;

public class UpdateServiceCommand : IRequest
{
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Endpoint { get; set; }
    public string? Status { get; set; }
    public int? CheckIntervalMinutes { get; set; }
}
