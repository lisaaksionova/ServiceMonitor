using MediatR;

namespace ServiceMonitor.Application.Services.Commands.DeleteService;

public class DeleteServiceCommand(Guid id) : IRequest
{
    public Guid Id { get; set; } = id;
}
