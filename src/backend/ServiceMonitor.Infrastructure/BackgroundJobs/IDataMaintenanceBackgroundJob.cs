namespace ServiceMonitor.Infrastructure.BackgroundJobs;

public interface IDataMaintenanceBackgroundJob
{
    Task ExecuteAsync(CancellationToken cancellationToken);
}
