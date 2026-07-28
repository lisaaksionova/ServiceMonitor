using MediatR;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Domain.Common;

namespace ServiceMonitor.Application.Services.Queries.GetAllServices;

public class GetAllServicesQuery(int page, int pageSize) : IRequest<PagedList<ServiceDto>>
{
    public int Page { get; } = page < 1 ? 1 : page;

    public int PageSize { get; } =
        pageSize > 100 ? 100 :
        pageSize < 0 ? 10 :
        pageSize;
}
