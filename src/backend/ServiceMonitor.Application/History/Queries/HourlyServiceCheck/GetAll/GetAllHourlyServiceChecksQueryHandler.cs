using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.History.Dtos.HourlyServiceChecks;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.History.Queries.HourlyServiceCheck.GetAll;

public class GetAllHourlyServiceChecksQueryHandler(
    IRepositoryManager repository,
    IMapper mapper,
    ILogger<GetAllHourlyServiceChecksQueryHandler> logger)
    : IRequestHandler<GetAllHourlyServiceChecksQuery, PagedList<HourlyServiceCheckDto>>
{
    public async Task<PagedList<HourlyServiceCheckDto>> Handle(GetAllHourlyServiceChecksQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting hourly service checks from {@From} on {@To}", request.From, request.To);

        var checks = await repository.HourlyServiceCheck.GetAllFromTo(request.Page, request.PageSize, request.ServiceId,
            request.From, request.To, cancellationToken);

        var checksDto = new PagedList<HourlyServiceCheckDto>(
            mapper.Map<List<HourlyServiceCheckDto>>(checks.Items),
            checks.Count,
            checks.CurrentPage,
            request.PageSize);

        return checksDto;
    }
}
