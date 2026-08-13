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
    IServiceRepository repository,
    IAuthenticatedUser authenticatedUser,
    IMapper mapper) : IRequestHandler<UpdateServiceAsHealthyCommand, ServiceDto>
{
    public async Task<ServiceDto> Handle(UpdateServiceAsHealthyCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating service as healthy");
        var service = await repository.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken)
            ?? throw new ServiceNotFoundException(request.Id);
        if (service.Status == ServiceStatus.Healthy)
        {
            throw new InvalidOperationException("Service is already healthy");
        }

        service.Status = ServiceStatus.Healthy;
        await repository.Save(cancellationToken);
        return mapper.Map<ServiceDto>(service);
    }
}
