using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class ServiceRepository(MonitorDbContext context) : RepositoryBase<Service>(context), IServiceRepository
{
    public async Task<Service?> GetByIdAsync(Guid id, string userId, CancellationToken cancellationToken)
        => await GetByCondition(s => s.Id.Equals(id) && s.UserId.Equals(userId))
            .SingleOrDefaultAsync(cancellationToken);

    public async Task<PagedList<Service>> GetPagedListAsync(int page, int pageSize, string userId,
        CancellationToken cancellationToken)
    {
        var query = GetAll()
            .Where(s => s.UserId == userId)
            .OrderBy(s => s.Name);

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Include(s => s.Incidents)
            .ToListAsync(cancellationToken);

        return new PagedList<Service>(items, count, page, pageSize);
    }

    public async Task<IEnumerable<Service>> GetServicesForCheck(CancellationToken cancellationToken)
    {
        var services = await GetByCondition(s => s.NextCheckAt <= DateTime.UtcNow)
            .Take(50)
            .ToListAsync(cancellationToken);
        return services;
    }

    public async Task CreateAsync(Service service, CancellationToken cancellationToken)
    {
        Create(service);
        await SaveAsync(cancellationToken);
    }

    public async Task DeleteAsync(Service service, CancellationToken cancellationToken)
    {
        Delete(service);
        await SaveAsync(cancellationToken);
    }

    public async Task UpdateAsync(Service service, CancellationToken cancellationToken)
    {
        Update(service);
        await SaveAsync(cancellationToken);
    }

    public async Task SaveAsync(CancellationToken cancellationToken) => await context.SaveChangesAsync(cancellationToken);
}
