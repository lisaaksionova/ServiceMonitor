using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.UpdateServiceAsHealthy;

public class UpdateServiceAsHealthyCommandHandler(
    ILogger<UpdateServiceAsHealthyCommandHandler> logger,
    IRepositoryManager repository,
    IAuthenticatedUser authenticatedUser,
    IMapper mapper) : IRequestHandler<UpdateServiceAsHealthyCommand, ServiceDto>
{
    public async Task<ServiceDto> Handle(UpdateServiceAsHealthyCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating service {ServiceId} as healthy", request.Id);

        var service = await repository.Service.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken);
        if (service == null)
        {
            logger.LogError("Service {ServiceId} is not found.", request.Id);
            throw new ServiceNotFoundException(request.Id);
        }

        if (service.Status == ServiceStatus.Healthy)
        {
            logger.LogError("Service {ServiceId} is already healthy", request.Id);
            throw new InvalidOperationException("Service is already healthy");
        }
        service.Status = ServiceStatus.Healthy;

        await repository.Service.Save(cancellationToken);
        return mapper.Map<ServiceDto>(service);
    }
}
