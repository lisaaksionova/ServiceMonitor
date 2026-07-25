using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Application.SharedServices;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Queries.GetAllServices;

public class GetAllServicesQueryHandler(
    IServiceRepository repository,
    IMapper mapper,
    IAuthenticatedUser authenticatedUser,
    ILogger<GetAllServicesQueryHandler> logger) : IRequestHandler<GetAllServicesQuery, IEnumerable<ServiceDto>>
{
    public async Task<IEnumerable<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all services");
        var services = await repository.GetAllAsync(request.Page, request.PageSize, authenticatedUser.UserId,
            cancellationToken);
        var serviceDtos = mapper.Map<IEnumerable<ServiceDto>>(services);
        return serviceDtos;
    }
}
