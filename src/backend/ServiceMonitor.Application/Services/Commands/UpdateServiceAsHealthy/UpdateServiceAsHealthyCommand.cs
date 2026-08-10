using MediatR;

namespace ServiceMonitor.Application.Services.Commands.UpdateServiceAsHealthy;

public class UpdateServiceAsHealthyCommand(Guid id) : IRequest
{
    public Guid Id { get; set; } = id;
}
