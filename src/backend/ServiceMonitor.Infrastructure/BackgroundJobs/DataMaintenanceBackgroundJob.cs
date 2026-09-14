using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.BackgroundJobs;

public class DataMaintenanceBackgroundJob(MonitorDbContext context, ILogger<DataMaintenanceBackgroundJob> logger) : IDataMaintenanceBackgroundJob
{
    public async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Hourly aggregation job started");

        var to = DateTime.UtcNow.Date;
        var from = to.AddDays(-1);

        var serviceChecks = context
            .ServiceChecks
            .AsNoTracking()
            .Where(s => s.CheckedAt >= from && s.CheckedAt < to);

        var hourlyChecks = await GetAggregatedHourlyServiceChecks(serviceChecks, cancellationToken);


        await context.HourlyServiceChecks.AddRangeAsync(
            hourlyChecks,
            cancellationToken);

        await context.SaveChangesAsync(cancellationToken);
    }

    private async Task<IEnumerable<HourlyServiceCheck>> GetAggregatedHourlyServiceChecks(IQueryable<ServiceCheck> query, CancellationToken cancellationToken)
    {
        return await query
            .GroupBy(s => new
            {
                s.ServiceId,
                Hour = new DateTime(
                    s.CheckedAt.Year,
                    s.CheckedAt.Month,
                    s.CheckedAt.Day,
                    s.CheckedAt.Hour,
                    0,
                    0,
                    DateTimeKind.Utc)
            })
            .Select(h => new HourlyServiceCheck
            {
                ServiceId = h.Key.ServiceId,
                Hour = h.Key.Hour,

                TotalChecks = h.Count(),
                SuccessfulChecks = h.Count(s => s.Status == ServiceStatus.Healthy),
                FailedChecks = h.Count(s => s.Status != ServiceStatus.Healthy),

                AverageResponseTimeMs = (int)h.Average(s => s.ResponseTimeMs),
                MinResponseTimeMs = h.Min(s => s.ResponseTimeMs),
                MaxResponseTimeMs = h.Max(s => s.ResponseTimeMs)
            })
            .ToListAsync(cancellationToken);
    }
}
