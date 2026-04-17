using AutoMapper;
using MediatR;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler(IServiceRepository repository,
    IMapper mapper) : IRequestHandler<UpdateServiceCommand>
{
    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = mapper.Map<Service>(request);
        await repository.UpdateAsync(service);
    }
}