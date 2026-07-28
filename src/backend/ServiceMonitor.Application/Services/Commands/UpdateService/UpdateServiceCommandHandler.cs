using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Commands.UpdateService;

public class UpdateServiceCommandHandler(
    IServiceRepository repository,
    IMapper mapper,
    IAuthenticatedUser authenticatedUser,
    ILogger<UpdateServiceCommandHandler> logger) : IRequestHandler<UpdateServiceCommand>
{
    public async Task Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Updating {@Service} with id {@ServiceId}", nameof(Service), request.Id);
        var service = await repository.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken) ??
                      throw new NotFoundException(nameof(Service), request.Id.ToString());
        mapper.Map(request, service);
        await repository.Save(cancellationToken);
    }
}
