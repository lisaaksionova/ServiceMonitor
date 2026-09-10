using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.ServiceChecks.Dtos;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.ServiceChecks.Queries;

public class GetAllServiceChecksQueryHandler(
    IRepositoryManager repository,
    IMapper mapper,
    ILogger<GetAllServiceChecksQueryHandler> logger) : IRequestHandler<GetAllServiceChecksQuery, PagedList<ServiceCheckDto>>
{
    public async Task<PagedList<ServiceCheckDto>> Handle(GetAllServiceChecksQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all service checks");

        var checks = await repository.ServiceCheck.GetPagedListAsync(request.Page, request.PageSize, request.ServiceId,
            cancellationToken);
        var checkDtos = new PagedList<ServiceCheckDto>(
            mapper.Map<List<ServiceCheckDto>>(checks.Items),
            checks.Count,
            checks.CurrentPage,
            request.PageSize);
        return checkDtos;
    }
}
