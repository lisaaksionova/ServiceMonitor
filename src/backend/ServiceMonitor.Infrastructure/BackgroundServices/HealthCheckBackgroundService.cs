using System.Net;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Infrastructure.BackgroundServices;

public class HealthCheckBackgroundService(
    IRepositoryManager repositoryManager,
    IServiceHealthChecker serviceHealthChecker,
    ILogger<HealthCheckBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            var services = await repositoryManager.Service.GetServicesForCheckAsync(cancellationToken);
            foreach (var service in services)
            {
                var result = await serviceHealthChecker.CheckAsync(service, cancellationToken);
                if (result.IsHealthy && service.Status != ServiceStatus.Healthy)
                {
                    var incident = await repositoryManager.Incident.GetOpenAsync(service.Id, cancellationToken);

                    if (incident != null)
                    {
                        incident.Status = IncidentStatus.Resolved;
                        incident.ResolvedAt = DateTime.UtcNow;
                        await repositoryManager.Incident.UpdateAsync(incident, cancellationToken);
                    }

                    service.Status = ServiceStatus.Healthy;
                    service.LastCheckAt = DateTime.UtcNow;
                    service.LastSuccessfulCheckAt = DateTime.UtcNow;
                    service.NextCheckAt = DateTime.UtcNow + TimeSpan.FromMinutes(service.CheckIntervalMinutes);
                    await repositoryManager.Service.UpdateAsync(service, cancellationToken);
                }

                if (!result.IsHealthy && service.Status == ServiceStatus.Healthy)
                {
                    var incident = new Incident
                    {
                        Date = DateTime.UtcNow,
                        Status = IncidentStatus.Open,
                        Description = $"Service failed due to {result.FailureReason} {result.StatusCode}"
                    };

                    await repositoryManager.Incident.CreateAsync(incident, cancellationToken);

                    service.Status = result.StatusCode switch
                    {
                        HttpStatusCode.NotFound => ServiceStatus.Unavailable,
                        HttpStatusCode.InternalServerError => ServiceStatus.Down,
                        HttpStatusCode.RequestTimeout => ServiceStatus.Down,
                        _ => ServiceStatus.Unknown
                    };
                    service.LastCheckAt = DateTime.UtcNow;
                    service.LastSuccessfulCheckAt = DateTime.UtcNow;
                    service.NextCheckAt = DateTime.UtcNow + TimeSpan.FromMinutes(service.CheckIntervalMinutes);
                    await repositoryManager.Service.UpdateAsync(service, cancellationToken);
                }
            }
        }
    }
}
