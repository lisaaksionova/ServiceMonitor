using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class IncidentRepository(MonitorDbContext context) : IIncidentRepository
{
    public async Task<IEnumerable<Incident>> GetAllAsync(CancellationToken cancellationToken)
    {
        var incidents = await context.Incidents.ToListAsync(cancellationToken);
        return incidents;
    }

    public async Task<CursorPagedList<Incident>> GetAllPaginatedAsync(string cursor, int limit,
        CancellationToken cancellationToken)
    {
        var decodedCursor = Cursor.Decode(cursor);
        var lastId = decodedCursor?.LastId;
        var created = decodedCursor?.CreatedAt;

        var query = context.Incidents
            .AsNoTracking()
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

    public async Task<Incident?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var incident = await context.Incidents.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        return incident;
    }

    public async Task CreateAsync(Incident incident, CancellationToken cancellationToken)
    {
        await context.Incidents.AddAsync(incident, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Incident incident, CancellationToken cancellationToken)
    {
        context.Incidents.Remove(incident);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken) =>
        await context.SaveChangesAsync(cancellationToken);
}
