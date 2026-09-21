using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Application.ServiceChecks.Dtos;
using ServiceMonitor.Application.ServiceChecks.Queries;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.History.Queries.ServiceCheck.GetAll;

public class GetAllServiceChecksQueryHandler(
    IRepositoryManager repository,
    IMapper mapper,
    ILogger<GetAllServiceChecksQueryHandler> logger,
    IAuthenticatedUser authenticatedUser)
    : IRequestHandler<GetAllServiceChecksQuery, PagedList<ServiceCheckDto>>
{
    public async Task<PagedList<ServiceCheckDto>> Handle(GetAllServiceChecksQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all service checks");


        var service =
            await repository.Service.GetByIdAsync(request.ServiceId, authenticatedUser.UserId, cancellationToken);
        if (service == null)
        {
            logger.LogError("Service {ServiceId} is not found.", request.ServiceId);
            throw new ServiceNotFoundException(request.ServiceId);
        }

        var checks = await repository.ServiceCheck.GetPagedListAsync(request.Page, request.PageSize, service.Id,
            cancellationToken);
        var checkDtos = new PagedList<ServiceCheckDto>(
            mapper.Map<List<ServiceCheckDto>>(checks.Items),
            checks.Count,
            checks.CurrentPage,
            request.PageSize);
        return checkDtos;
    }
}
