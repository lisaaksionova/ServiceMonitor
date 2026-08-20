using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class IncidentRepository(MonitorDbContext context) : RepositoryBase<Incident>(context), IIncidentRepository
{
    public async Task<CursorPagedList<Incident>> GetAllPaginatedAsync(Guid serviceId, string cursor, int limit,
        CancellationToken cancellationToken)
    {
        var decodedCursor = Cursor.Decode(cursor);
        var lastId = decodedCursor?.LastId;
        var created = decodedCursor?.CreatedAt;

        var query = GetAll()
            .Where(i => i.ServiceId.Equals(serviceId))
            .OrderByDescending(i => i.Date)
            .ThenBy(i => i.Id)
            .AsQueryable();

        if (created.HasValue)
        {
            query = query.Where(i =>
                i.Date < created.Value ||
                (i.Date == created.Value && i.Id > lastId!.Value));
        }

        var incidents = await query.Take(limit + 1).ToListAsync(cancellationToken);
        var hasMore = incidents.Count > limit;
        if (hasMore)
        {
            incidents.RemoveAt(incidents.Count - 1);
        }

        var lastIncident = incidents.LastOrDefault();
        var nextCursor = hasMore && lastIncident != null
            ? Cursor.Encode(lastIncident.Id, lastIncident.Date)
            : null;

        return new CursorPagedList<Incident>(incidents, nextCursor, hasMore);
    }

    public async Task<List<Incident>> GetAllOpenAsync(Guid serviceId, CancellationToken cancellationToken)
    {
        var openIncidents = await GetByCondition(i => i.ServiceId == serviceId && i.Status == IncidentStatus.Open)
            .ToListAsync(cancellationToken);
        return openIncidents;
    }

    public async Task<Incident?> GetByIdAsync(Guid serviceId, Guid id, CancellationToken cancellationToken)
    {
        var incident = await
            GetByCondition(i => i.ServiceId.Equals(serviceId) && i.Id.Equals(id))
                .SingleOrDefaultAsync(cancellationToken);

        return incident;
    }

    public async Task CreateAsync(Incident incident, CancellationToken cancellationToken)
    {
        Create(incident);
        await SaveAsync(cancellationToken);
    }

    public async Task DeleteAsync(Incident incident, CancellationToken cancellationToken)
    {
        Delete(incident);
        await SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync(Incident incident, CancellationToken cancellationToken)
    {
        Update(incident);
        await SaveAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken) =>
        await context.SaveChangesAsync(cancellationToken);
}
