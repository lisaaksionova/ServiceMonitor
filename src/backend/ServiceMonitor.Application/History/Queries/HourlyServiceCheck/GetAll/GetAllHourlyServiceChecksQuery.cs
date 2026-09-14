using MediatR;
using ServiceMonitor.Application.History.Dtos.HourlyServiceChecks;
using ServiceMonitor.Domain.Common;

namespace ServiceMonitor.Application.History.Queries.HourlyServiceCheck.GetAll;

public class GetAllHourlyServiceChecksQuery(int page, int pageSize, Guid serviceId, DateTime from, DateTime to) : IRequest<PagedList<HourlyServiceCheckDto>>
{
    public int Page { get; } = page < 1 ? 1 : page;

    public int PageSize { get; } =
        pageSize > 100 ? 100 :
        pageSize < 0 ? 10 :
        pageSize;

    public Guid ServiceId { get; set; } = serviceId;

    public DateTime From { get; set; } = from;

    public DateTime To { get; set; } = to;
}
