using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.DeleteService;

public class DeleteServiceCommandHandler(
    IRepositoryManager repository,
    IAuthenticatedUser authenticatedUser,
    ILogger<DeleteServiceCommandHandler> logger) : IRequestHandler<DeleteServiceCommand>
{
    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Removing the service {@ServiceId}", request.Id);

        var service = await repository.Service.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken);
        if (service == null)
        {
            logger.LogError("Service {ServiceId} is not found.", request.Id);
            throw new ServiceNotFoundException(request.Id);
        }
        await repository.Service.Delete(service, cancellationToken);
    }
}
