using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.UpdateServiceAsHealthy;

public class UpdateServiceAsHealthyCommandHandler(
    ILogger<UpdateServiceAsHealthyCommandHandler> logger,
    IServiceRepository repository,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<UpdateServiceAsHealthyCommand>
{
    public async Task Handle(UpdateServiceAsHealthyCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating service as healthy");
        var service = await repository.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Service), request.Id.ToString());
        if (service.Status == ServiceStatus.Healthy)
        {
            throw new InvalidOperationException("Service is already healthy");
        }

        service.Status = ServiceStatus.Healthy;
        await repository.Save(cancellationToken);
    }
}
