namespace ServiceMonitor.Application.Interfaces;

public interface IServiceHealthChecker
{
    Task<CheckServiceResult> CheckAsync(Service service, CancellationToken cancellationToken);
}
