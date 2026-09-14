using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class HourlyServiceCheckRepository(MonitorDbContext context) : RepositoryBase<HourlyServiceCheck>(context), IHourlyServiceCheckRepository
{
    public async Task CreateAsync(HourlyServiceCheck serviceCheck, CancellationToken cancellationToken)
    {
        Create(serviceCheck);
        await Task.CompletedTask;
    }

    public async Task<PagedList<HourlyServiceCheck>> GetPagedListAsync(int page, int pageSize, Guid serviceId,
        CancellationToken cancellationToken)
    {
        var query = GetAll()
            .AsNoTracking()
            .Where(s => s.ServiceId == serviceId)
            .OrderBy(s => s.Hour);

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<HourlyServiceCheck>(items, count, page, pageSize);
    }

    public async Task DeleteOlderThanAsync(DateTime hour, CancellationToken cancellationToken) => await context.HourlyServiceChecks.Where(s => s.Hour < hour).ExecuteDeleteAsync(cancellationToken);
}
