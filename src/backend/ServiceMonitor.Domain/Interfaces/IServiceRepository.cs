using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(Guid id, string userId, CancellationToken cancellationToken);

    Task<PagedList<Service>> GetPagedListAsync(int page, int pageSize, string userId,
        CancellationToken cancellationToken);

    Task CreateAsync(Service service, CancellationToken cancellationToken);
    Task DeleteAsync(Service service, CancellationToken cancellationToken);
    Task UpdateAsync(Service service, CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
    Task<IEnumerable<Service>> GetServicesForCheck(CancellationToken cancellationToken);
}
