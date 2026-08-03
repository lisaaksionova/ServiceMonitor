using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IIncidentRepository
{
    Task<CursorPagedList<Incident>> GetAllPaginatedAsync(string cursor, int limit, string userId, CancellationToken cancellationToken);
    Task<Incident?> GetByIdAsync(Guid id, string userId, CancellationToken cancellationToken);
    Task CreateAsync(Incident incident, CancellationToken cancellationToken);
    Task DeleteAsync(Incident incident, CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
}
