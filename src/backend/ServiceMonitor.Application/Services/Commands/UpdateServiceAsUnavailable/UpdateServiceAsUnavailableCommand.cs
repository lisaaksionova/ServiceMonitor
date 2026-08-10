using MediatR;

namespace ServiceMonitor.Application.Services.Commands.UpdateServiceAsUnavailable;

public class UpdateServiceAsUnavailableCommand(Guid id) : IRequest
{
    public Guid Id { get; set; } = id;
}
