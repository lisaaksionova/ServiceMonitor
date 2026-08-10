using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.UpdateServiceAsUnavailable;

public class UpdateServiceAsUnavailableCommandHandler(
    ILogger<UpdateServiceAsUnavailableCommandHandler> logger,
    IServiceRepository repository,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<UpdateServiceAsUnavailableCommand>
{
    public async Task Handle(UpdateServiceAsUnavailableCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating service as unavailable");
        var service = await repository.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken)
                      ?? throw new NotFoundException(nameof(Service), request.Id.ToString());
        if (service.Status == ServiceStatus.Unavailable)
        {
            throw new InvalidOperationException("Service is already unavailable");
        }

        service.Status = ServiceStatus.Unavailable;
        await repository.Save(cancellationToken);
    }
}
