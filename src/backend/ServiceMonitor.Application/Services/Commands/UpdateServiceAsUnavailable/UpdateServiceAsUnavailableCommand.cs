using MediatR;
using ServiceMonitor.Application.Services.Dtos;

namespace ServiceMonitor.Application.Services.Commands.UpdateServiceAsUnavailable;

public class UpdateServiceAsUnavailableCommand(Guid id) : IRequest<ServiceDto>
{
    public Guid Id { get; set; } = id;
}
