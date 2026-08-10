using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Services.Queries.GetAllServices;

public class GetAllServicesQueryHandler(
    IServiceRepository repository,
    IMapper mapper,
    IAuthenticatedUser authenticatedUser,
    ILogger<GetAllServicesQueryHandler> logger) : IRequestHandler<GetAllServicesQuery, PagedList<ServiceDto>>
{
    public async Task<PagedList<ServiceDto>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all services");
        var services = await repository.GetPagedListAsync(request.Page, request.PageSize, authenticatedUser.UserId,
            cancellationToken);
        var serviceDtos = new PagedList<ServiceDto>(
            mapper.Map<List<ServiceDto>>(services.Items),
            services.Count,
            services.CurrentPage,
            request.PageSize
        );
        return serviceDtos;
    }
}
