using AutoMapper;
using MediatR;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler(IServiceRepository repository) : IRequestHandler<DeleteServiceCommand>
{
    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        await repository.Delete(request.Id);
    }
}