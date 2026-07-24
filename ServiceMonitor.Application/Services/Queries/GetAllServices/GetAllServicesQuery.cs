using MediatR;
using ServiceMonitor.Application.Services.Dtos;

namespace ServiceMonitor.Application.Services.Queries.GetAllServices;

public class GetAllServicesQuery(int page, int pageSize) : IRequest<IEnumerable<ServiceDto>>
{
    public int Page { get; set; } = page < 1 ? 1 : page;
    public int PageSize { get; set; } =
        pageSize > 100 ? 100 :
        pageSize < 0 ? 10 :
        pageSize;
}