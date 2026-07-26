using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IServiceRepository
{
    Task<Service?> GetByIdAsync(int id, string userId, CancellationToken cancellationToken);

    Task<PagedList<Service>> GetPagedListAsync(int page, int pageSize, string userId,
        CancellationToken cancellationToken);

    Task CreateAsync(Service service, CancellationToken cancellationToken);
    Task Delete(Service service, CancellationToken cancellationToken);
    Task Save(CancellationToken cancellationToken);
    Task<IEnumerable<Service>> GetAllAsync(string userId, CancellationToken cancellationToken);
    Task<IEnumerable<Service>> GetServicesForCheck(CancellationToken cancellationToken);
}
