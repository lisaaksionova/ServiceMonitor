using MediatR;
using ServiceMonitor.Application.ServiceChecks.Dtos;
using ServiceMonitor.Domain.Common;

namespace ServiceMonitor.Application.ServiceChecks.Queries;

public class GetAllServiceChecksQuery(int page, int pageSize, Guid serviceId) : IRequest<PagedList<ServiceCheckDto>>
{
    public int Page { get; } = page < 1 ? 1 : page;

    public int PageSize { get; } =
        pageSize > 100 ? 100 :
        pageSize < 0 ? 10 :
        pageSize;

    public Guid ServiceId { get; set; } = serviceId;
}
