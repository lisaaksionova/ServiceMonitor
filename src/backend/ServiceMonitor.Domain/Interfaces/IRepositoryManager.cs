namespace ServiceMonitor.Domain.Interfaces;

public interface IRepositoryManager
{
    IServiceRepository Service { get; }
    IIncidentRepository Incident { get; }
    IServiceCheckRepository ServiceCheck { get; }
    IHourlyServiceCheckRepository HourlyServiceCheck { get; }
    Task SaveAsync(CancellationToken cancellationToken);
}
