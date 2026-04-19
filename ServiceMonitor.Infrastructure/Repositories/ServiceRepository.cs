using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class ServiceRepository(MonitorDbContext context) : IServiceRepository
{
    public async Task<Service?> GetByIdAsync(int id)
    {
        var service = await context.Services.Include(s => s.Incidents).FirstOrDefaultAsync(s => s.Id == id);
        return service;
    }

    public async Task<IEnumerable<Service>> GetAllAsync()
    {
        var services = await context.Services.Include(s => s.Incidents).ToListAsync();
        return services;
    }

    public async Task<IEnumerable<Service>> GetServicesForCheck()
    {
        var services = await context.Services
            .Where(s => s.NextCheckAt <= DateTime.UtcNow)
            .Take(50)
            .ToListAsync();
        return services;
    }

    public async Task CreateAsync(Service service)
    {
        await context.Services.AddAsync(service);
        await context.SaveChangesAsync();
    }

    public async Task Delete(Service service)
    {
        context.Services.Remove(service);
        await context.SaveChangesAsync();
    }

    public async Task Save()
    {
        await context.SaveChangesAsync();
    }
}