using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.SharedServices;

public class HourlyServiceCheckAggregator(IRepositoryManager repository) : IHourlyServiceCheckAggregator
{
    public async Task AggregateAsync(CancellationToken cancellationToken)
    {
        var to = DateTime.UtcNow;
        var from = to.AddDays(-1);

        var serviceChecks = repository.ServiceCheck
            .GetAllByDate(from, to, cancellationToken);

        var hourlyChecks = await serviceChecks
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


        repository.HourlyServiceCheck.CreateRange(hourlyChecks);

        await repository.SaveAsync(cancellationToken);
    }
}
