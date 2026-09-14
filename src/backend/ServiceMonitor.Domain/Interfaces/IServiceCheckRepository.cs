using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IServiceCheckRepository
{
    Task CreateAsync(ServiceCheck serviceCheck, CancellationToken  cancellationToken);

    Task<PagedList<ServiceCheck>> GetPagedListAsync(int page, int pageSize, Guid serviceId,
        CancellationToken cancellationToken);

    Task DeleteOlderThanAsync(DateTime date, CancellationToken cancellationToken);
}
