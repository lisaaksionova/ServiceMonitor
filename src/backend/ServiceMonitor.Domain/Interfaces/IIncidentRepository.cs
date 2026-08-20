using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IIncidentRepository
{
    Task<CursorPagedList<Incident>> GetAllPaginatedAsync(Guid serviceId, string cursor, int limit, CancellationToken cancellationToken);
    Task<List<Incident>> GetAllOpenAsync(Guid serviceId, CancellationToken cancellationToken);
    Task<Incident?> GetByIdAsync(Guid serviceId, Guid id, CancellationToken cancellationToken);
    Task CreateAsync(Incident incident, CancellationToken cancellationToken);
    Task DeleteAsync(Incident incident, CancellationToken cancellationToken);
    Task UpdateAsync(Incident incident, CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
}
