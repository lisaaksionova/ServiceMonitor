using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;

namespace ServiceMonitor.Infrastructure.BackgroundJobs;

public class DataMaintenanceBackgroundJob(
    IHourlyServiceCheckAggregator hourlyServiceCheckAggregator,
    IDataRetentionService dataRetentionService,
    ILogger<DataMaintenanceBackgroundJob> logger) : IDataMaintenanceBackgroundJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Data maintenance job started");

        await hourlyServiceCheckAggregator.AggregateAsync(cancellationToken);

        await dataRetentionService.RemoveStaleServiceChecks(DateTime.UtcNow.AddDays(-7), cancellationToken);

        logger.LogInformation("Hourly aggregation job completed");
    }
}
