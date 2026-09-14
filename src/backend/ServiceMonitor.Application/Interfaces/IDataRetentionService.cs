namespace ServiceMonitor.Application.Interfaces;

public interface IDataRetentionService
{
    Task RemoveStaleServiceChecks(DateTime olderThan, CancellationToken cancellationToken);
}
