using MediatR;

namespace ServiceMonitor.Application.Services.Commands.DeleteService;

public class DeleteServiceCommand(int id) : IRequest
{
    public int Id { get; set; } = id;
}