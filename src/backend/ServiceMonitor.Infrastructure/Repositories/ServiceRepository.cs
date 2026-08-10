using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class ServiceRepository(MonitorDbContext context) : IServiceRepository
{
    public async Task<Service?> GetByIdAsync(Guid id, string userId, CancellationToken cancellationToken)
    {
        var service = await context.Services
            .Include(s => s.Incidents)
            .FirstOrDefaultAsync(s => s.Id == id && s.UserId == userId, cancellationToken);
        return service;
    }

    public async Task<PagedList<Service>> GetPagedListAsync(int page, int pageSize, string userId,
        CancellationToken cancellationToken)
    {
        var query = context.Services.Include(s => s.Incidents).AsNoTrackingWithIdentityResolution();
        var count = await query.CountAsync(cancellationToken);
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).Where(s => s.UserId == userId)
            .ToListAsync(cancellationToken);
        return new PagedList<Service>(items, count, page, pageSize);
    }

    public async Task<IEnumerable<Service>> GetAllAsync(string userId,
        CancellationToken cancellationToken)
    {
        var services = await context.Services.Include(s => s.Incidents).Where(s => s.UserId == userId)
            .ToListAsync(cancellationToken);

        return services;
    }

    public async Task<IEnumerable<Service>> GetServicesForCheck(CancellationToken cancellationToken)
    {
        var services = await context.Services
            .Where(s => s.NextCheckAt <= DateTime.UtcNow)
            .Take(50)
            .ToListAsync(cancellationToken);
        return services;
    }

    public async Task CreateAsync(Service service, CancellationToken cancellationToken)
    {
        await context.Services.AddAsync(service, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(Service service, CancellationToken cancellationToken)
    {
        context.Services.Remove(service);
        await context.SaveChangesAsync(cancellationToken);
    }

    public async Task Save(CancellationToken cancellationToken) => await context.SaveChangesAsync(cancellationToken);
}
