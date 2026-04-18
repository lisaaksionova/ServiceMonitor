using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class IncidentRepository(MonitorDbContext context) : IIncidentRepository
{
    public async Task<IEnumerable<Incident>> GetAllAsync()
    {
        var incidents = await context.Incidents.ToListAsync();
        return incidents;
    }

    public async Task<Incident?> GetByIdAsync(int id)
    {
        var incident = await context.Incidents.FirstOrDefaultAsync(x => x.Id == id);
        return incident;
    }

    public async Task CreateAsync(Incident incident)
    {
        await context.Incidents.AddAsync(incident);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Incident incident)
    {
        context.Incidents.Remove(incident);
        await context.SaveChangesAsync();
    }

    public async Task SaveAsync()
    {
        await context.SaveChangesAsync();
    }
}