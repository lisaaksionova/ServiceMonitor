using MediatR;
using ServiceMonitor.Application.Services.Dtos;

namespace ServiceMonitor.Application.Services.Commands.UpdateServiceAsHealthy;

public class UpdateServiceAsHealthyCommand(Guid id) : IRequest<ServiceDto>
{
    public Guid Id { get; set; } = id;
}
