using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class ServiceRepository(MonitorDbContext context) : IServiceRepository
{
    public async Task<Service?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var service = await context.Services.Include(s => s.Incidents).FirstOrDefaultAsync(s => s.Id == id,  cancellationToken);
        return service;
    }

    public async Task<IEnumerable<Service>> GetAllAsync(CancellationToken cancellationToken)
    {
        var services = await context.Services.Include(s => s.Incidents).ToListAsync(cancellationToken);
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

    public async Task Save(CancellationToken cancellationToken)
    {
        await context.SaveChangesAsync(cancellationToken);
    }
}