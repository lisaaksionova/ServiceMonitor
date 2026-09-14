using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IHourlyServiceCheckRepository
{
    Task CreateAsync(HourlyServiceCheck serviceCheck, CancellationToken  cancellationToken);

    Task<PagedList<HourlyServiceCheck>> GetPagedListAsync(int page, int pageSize, Guid serviceId,
        CancellationToken cancellationToken);

    Task DeleteOlderThanAsync(DateTime hour, CancellationToken cancellationToken);
}
