using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class ServiceRepository(MonitorDbContext context) : IServiceRepository
{
    public async Task<Service?> GetByIdAsync(int id)
    {
        var service = await context.Services.FirstOrDefaultAsync(s => s.Id == id);
        return service;
    }

    public async Task CreateAsync(Service service)
    {
        await context.Services.AddAsync(service);
        await context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Service service)
    {
        var serviceToUpdate = await context.Services.FirstOrDefaultAsync(s => s.Id == service.Id);
        serviceToUpdate.Name = service.Name;
        serviceToUpdate.Endpoint = service.Endpoint;
        serviceToUpdate.Status = service.Status;
        await context.SaveChangesAsync();
    }

    public async Task Delete(int id)
    {
        var service = await context.Services.FirstOrDefaultAsync(s => s.Id == id);
        context.Services.Remove(service);
        await context.SaveChangesAsync();
    }
}