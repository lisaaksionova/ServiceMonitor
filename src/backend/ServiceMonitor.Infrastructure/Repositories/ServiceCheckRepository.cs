using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class ServiceCheckRepository(MonitorDbContext context) : RepositoryBase<ServiceCheck>(context), IServiceCheckRepository
{
    public async Task CreateAsync(ServiceCheck serviceCheck, CancellationToken cancellationToken)
    {
        Create(serviceCheck);
        await Task.CompletedTask;
    }

    public async Task<PagedList<ServiceCheck>> GetPagedListAsync(int page, int pageSize, Guid serviceId,
        CancellationToken cancellationToken)
    {
        var query = GetAll()
            .AsNoTracking()
            .Where(s => s.ServiceId == serviceId)
            .OrderBy(s => s.CheckedAt);

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return new PagedList<ServiceCheck>(items, count, page, pageSize);
    }

    public async Task DeleteOlderThanAsync(DateTime date, CancellationToken cancellationToken) => await context.ServiceChecks.Where(s => s.CheckedAt < date).ExecuteDeleteAsync(cancellationToken);
}
