using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.History.Dtos.HourlyServiceChecks;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.History.Queries.HourlyServiceCheck.GetAll;

public class GetAllHourlyServiceChecksQueryHandler(
    IRepositoryManager repository,
    IMapper mapper,
    ILogger<GetAllHourlyServiceChecksQueryHandler> logger,
    IAuthenticatedUser authenticatedUser)
    : IRequestHandler<GetAllHourlyServiceChecksQuery, PagedList<HourlyServiceCheckDto>>
{
    public async Task<PagedList<HourlyServiceCheckDto>> Handle(GetAllHourlyServiceChecksQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting hourly service checks from {@From} on {@To}", request.From, request.To);

        var service =
            await repository.Service.GetByIdAsync(request.ServiceId, authenticatedUser.UserId, cancellationToken);
        if (service == null)
        {
            logger.LogError("Service {ServiceId} is not found.", request.ServiceId);
            throw new ServiceNotFoundException(request.ServiceId);
        }

        var checks = await repository.HourlyServiceCheck.GetAllFromToPaged(request.Page, request.PageSize, service.Id,
            request.From, request.To, cancellationToken);

        var checksDto = new PagedList<HourlyServiceCheckDto>(
            mapper.Map<List<HourlyServiceCheckDto>>(checks.Items),
            checks.Count,
            checks.CurrentPage,
            request.PageSize);

        return checksDto;
    }
}
