using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.CreateService;

public class CreateServiceCommandHandler(
    IRepositoryManager repository,
    IMapper mapper,
    ILogger<CreateServiceCommandHandler> logger,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<CreateServiceCommand, ServiceDto>
{
    public async Task<ServiceDto> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new service {@Service} with endpoint {@Endpoint}", request.Name,
            request.Endpoint);

        var service = mapper.Map<Service>(request);
        service.Status = ServiceStatus.Healthy;
        service.UserId = authenticatedUser.UserId;

        await repository.Service.CreateAsync(service, cancellationToken);
        return mapper.Map<ServiceDto>(service);
    }
}
