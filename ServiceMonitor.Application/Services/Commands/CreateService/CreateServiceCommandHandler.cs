using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.SharedServices;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.CreateService;

public class CreateServiceCommandHandler(IServiceRepository repository,
    IMapper mapper,
    ILogger<CreateServiceCommandHandler> logger,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<CreateServiceCommand>
{
    public async Task Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating new service {@Service} with endpoint {@Endpoint}", request.Name, request.Endpoint);
        var service = mapper.Map<Service>(request);
        service.Status = ServiceStatus.Healthy;
        service.UserId = authenticatedUser.UserId;
        await repository.CreateAsync(service, cancellationToken);
    }
}