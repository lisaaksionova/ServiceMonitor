using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Domain.Interfaces;

public interface IIncidentRepository
{
    Task<IEnumerable<Incident>> GetAllAsync(CancellationToken cancellationToken);
    Task<CursorPagedList<Incident>> GetAllPaginatedAsync(string cursor, int limit, CancellationToken cancellationToken);
    Task<Incident?> GetByIdAsync(int id, CancellationToken cancellationToken);
    Task CreateAsync(Incident incident, CancellationToken cancellationToken);
    Task DeleteAsync(Incident incident, CancellationToken cancellationToken);
    Task SaveAsync(CancellationToken cancellationToken);
}
