using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.SharedServices;

public class DataRetentionService(IRepositoryManager repository,
    ILogger<DataRetentionService> logger) : IDataRetentionService
{
    public async Task RemoveStaleServiceChecks(DateTime olderThan, CancellationToken cancellationToken)
    {
        logger.LogInformation("Removing stale service checks");

        await repository.ServiceCheck.DeleteOlderThanAsync(olderThan, cancellationToken);
    }
}
