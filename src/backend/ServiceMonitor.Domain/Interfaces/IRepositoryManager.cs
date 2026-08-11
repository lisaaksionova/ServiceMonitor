namespace ServiceMonitor.Domain.Interfaces;

public interface IRepositoryManager
{
    IServiceRepository Service { get; }
    IIncidentRepository Incident { get; }
    Task SaveAsync(CancellationToken cancellationToken);
}
