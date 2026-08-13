using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler(
    IServiceRepository repository,
    IAuthenticatedUser authenticatedUser,
    ILogger<DeleteServiceCommandHandler> logger) : IRequestHandler<DeleteServiceCommand>
{
    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Removing the service {@ServiceId}", request.Id);
        var service = await repository.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken) ??
                      throw new ServiceNotFoundException(request.Id);
        await repository.Delete(service, cancellationToken);
    }
}
