using AutoMapper;
using MediatR;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler(IServiceRepository repository) : IRequestHandler<DeleteServiceCommand>
{
    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await repository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(Service), request.Id.ToString());
        await repository.Delete(service);
    }
}