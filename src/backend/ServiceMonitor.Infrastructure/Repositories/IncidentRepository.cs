using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class IncidentRepository(MonitorDbContext context) : IIncidentRepository
{
    public async Task<CursorPagedList<Incident>> GetAllPaginatedAsync(Guid serviceId, string cursor, int limit, string userId,
        CancellationToken cancellationToken)
    {
        var decodedCursor = Cursor.Decode(cursor);
        var lastId = decodedCursor?.LastId;
        var created = decodedCursor?.CreatedAt;

        var query = context.Incidents
            .Where(i => i.ServiceId == serviceId)
            .Include(i => i.Service)
            .AsNoTrackingWithIdentityResolution()
            .Where(i => i.Service.UserId == userId)
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
        var openIncidents = await context.Incidents
            .Where(i => i.ServiceId == serviceId && i.Status == IncidentStatus.Open)
            .ToListAsync(cancellationToken);
        return openIncidents;
    }

    public async Task<Incident?> GetByIdAsync(Guid serviceId, Guid id, string userId, CancellationToken cancellationToken)
    {
        var incident = await context.Incidents
            .Where(i => i.ServiceId == serviceId)
            .Include(i => i.Service)
            .FirstOrDefaultAsync(x => x.Id == id && x.Service.UserId == userId, cancellationToken);
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
