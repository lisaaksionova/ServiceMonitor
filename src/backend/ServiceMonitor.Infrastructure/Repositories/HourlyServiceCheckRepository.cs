using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class HourlyServiceCheckRepository(MonitorDbContext context)
    : RepositoryBase<HourlyServiceCheck>(context), IHourlyServiceCheckRepository
{
    public void CreateRange(IEnumerable<HourlyServiceCheck> serviceCheck) =>
        context.HourlyServiceChecks.AddRange(serviceCheck);

    public async Task<PagedList<HourlyServiceCheck>> GetAllFromToPaged(int page, int pageSize, Guid serviceId, DateTime from,
        DateTime to,
        CancellationToken cancellationToken)
    {
        var query = GetAll()
            .AsNoTracking()
            .Where(s => s.ServiceId == serviceId && s.Hour >= from && s.Hour <= to)
            .OrderBy(s => s.Hour);

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<HourlyServiceCheck>(items, count, page, pageSize);
    }

    public async Task<List<HourlyServiceCheck>> GetAllFromTo(DateTime from, DateTime to,
        CancellationToken cancellationToken) =>
        await GetByCondition(s => s.Hour >= from && s.Hour <= to).ToListAsync(cancellationToken);

    public async Task<DateTime?> GetLastCheckedHour(CancellationToken cancellationToken)
    {
        return await GetAll()
            .AsNoTracking()
            .OrderByDescending(h => h.Hour)
            .Select(h => (DateTime?)h.Hour)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task DeleteOlderThanAsync(DateTime hour, CancellationToken cancellationToken) =>
        await context.HourlyServiceChecks.Where(s => s.Hour < hour).ExecuteDeleteAsync(cancellationToken);
}
