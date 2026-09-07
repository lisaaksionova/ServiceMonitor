using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly MonitorDbContext _context;
    private readonly Lazy<IServiceRepository> _serviceRepository;
    private readonly Lazy<IIncidentRepository> _incidentRepository;

    public RepositoryManager(MonitorDbContext context)
    {
        _context = context;
        _serviceRepository = new Lazy<IServiceRepository>(() => new ServiceRepository(_context));
        _incidentRepository = new Lazy<IIncidentRepository>(() => new IncidentRepository(_context));
    }

    public IServiceRepository Service => _serviceRepository.Value;
    public IIncidentRepository Incident => _incidentRepository.Value;

    public async Task SaveAsync(CancellationToken cancellationToken) => await _context.SaveChangesAsync(cancellationToken);
}
