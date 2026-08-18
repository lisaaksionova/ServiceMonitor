using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Queries.GetServiceById;

public class GetServiceByIdQueryHandler(
    ILogger<GetServiceByIdQueryHandler> logger,
    IRepositoryManager repository,
    IMapper mapper,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<GetServiceByIdQuery, ServiceDto>
{
    public async Task<ServiceDto> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting service {ServiceId}", request.Id);

        var service = await repository.Service.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken);
        if (service == null)
        {
            logger.LogError("Service {ServiceId} is not found.", request.Id);
            throw new ServiceNotFoundException(request.Id);
        }

        var serviceDto = mapper.Map<ServiceDto>(service);
        return serviceDto;
    }
}
