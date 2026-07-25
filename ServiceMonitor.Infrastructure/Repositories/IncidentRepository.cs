using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public class IncidentRepository(MonitorDbContext context) : IIncidentRepository
{
    public async Task<IEnumerable<Incident>> GetAllAsync(CancellationToken cancellationToken)
    {
        var incidents = await context.Incidents.ToListAsync(cancellationToken);
        return incidents;
    }

    public async Task<Incident?> GetByIdAsync(int id, CancellationToken cancellationToken)
    {
        var incident = await context.Incidents.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
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
