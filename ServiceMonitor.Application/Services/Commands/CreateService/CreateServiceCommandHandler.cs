using AutoMapper;
using MediatR;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.CreateService;

public class CreateServiceCommandHandler(IServiceRepository repository,
    IMapper mapper) : IRequestHandler<CreateServiceCommand>
{
    public async Task Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = mapper.Map<Service>(request);
        service.Status = ServiceStatus.Healthy;
        await repository.CreateAsync(service);
    }
}