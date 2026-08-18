using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.UpdateServiceAsUnavailable;

public class UpdateServiceAsUnavailableCommandHandler(
    ILogger<UpdateServiceAsUnavailableCommandHandler> logger,
    IRepositoryManager repository,
    IAuthenticatedUser authenticatedUser,
    IMapper mapper) : IRequestHandler<UpdateServiceAsUnavailableCommand, ServiceDto>
{
    public async Task<ServiceDto> Handle(UpdateServiceAsUnavailableCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating service as unavailable");

        var service = await repository.Service.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken);
        if (service == null)
        {
            logger.LogError("Service {ServiceId} is not found.", request.Id);
            throw new ServiceNotFoundException(request.Id);
        }

        if (service.Status == ServiceStatus.Unavailable)
        {
            logger.LogError("Service {ServiceId} is already unavailable", request.Id);
            throw new InvalidOperationException("Service is already unavailable");
        }

        service.Status = ServiceStatus.Unavailable;
        await repository.Service.Save(cancellationToken);
        return mapper.Map<ServiceDto>(service);
    }
}
