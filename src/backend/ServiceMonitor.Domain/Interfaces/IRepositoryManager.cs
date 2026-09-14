namespace ServiceMonitor.Domain.Interfaces;

public interface IRepositoryManager
{
    IServiceRepository Service { get; }
    IIncidentRepository Incident { get; }
    IServiceCheckRepository ServiceCheck { get;  }
    Task SaveAsync(CancellationToken cancellationToken);
}
