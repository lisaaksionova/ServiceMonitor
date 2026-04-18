using AutoMapper;
using MediatR;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler(IServiceRepository repository,
    IMapper mapper) : IRequestHandler<UpdateServiceCommand>
{
    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await repository.GetByIdAsync(request.Id) ?? throw new NotFoundException(nameof(Service), request.Id.ToString());
        mapper.Map(request, service);
        await repository.Save();
    }
}