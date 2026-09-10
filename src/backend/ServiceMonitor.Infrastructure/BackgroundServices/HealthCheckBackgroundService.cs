using System.Diagnostics;
using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Infrastructure.BackgroundServices;

public class HealthCheckBackgroundService(
    IServiceProvider serviceProvider,
    ILogger<HealthCheckBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Starting health check background service");

        using var scope = serviceProvider.CreateScope();
        var repositoryManager = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();
        var serviceHealthChecker = scope.ServiceProvider.GetRequiredService<IServiceHealthChecker>();
        var sw = new Stopwatch();

        while (!cancellationToken.IsCancellationRequested)
        {
            var services = await repositoryManager.Service.GetServicesForCheckAsync(cancellationToken);
            foreach (var service in services)
            {
                logger.LogInformation("Checking service: {@ServiceName}", service.Name);

                var now = DateTime.UtcNow;
                sw.Start();
                var result = await serviceHealthChecker.CheckAsync(service, cancellationToken);
                sw.Stop();
                switch (result.IsHealthy)
                {
                    case true when service.Status != ServiceStatus.Healthy:
                        {
                            logger.LogInformation("Service {@ServiceName} is healthy", service.Name);
                            var incident = await repositoryManager.Incident.GetOpenAsync(service.Id, cancellationToken);

                            if (incident != null)
                            {
                                incident.Status = IncidentStatus.Resolved;
                                incident.ResolvedAt = now;
                                await repositoryManager.Incident.UpdateAsync(incident, cancellationToken);
                            }

                            service.Status = ServiceStatus.Healthy;
                            service.LastSuccessfulCheckAt = now;
                            break;
                        }
                    case false when service.Status == ServiceStatus.Healthy:
                        {
                            var incident = new Incident
                            {
                                Date = now,
                                Status = IncidentStatus.Open,
                                Description = $"Service failed due to {result.FailureReason} {result.StatusCode}"
                            };

                            await repositoryManager.Incident.CreateAsync(incident, cancellationToken);

                            service.Status = CheckServiceStatus(result.StatusCode, cancellationToken);
                            service.LastFailureReason = result.FailureReason;

                            logger.LogInformation("Service {@ServiceName} is {@ServiceStatus} due to {@FailureReason}", service.Name, service.Status, service.LastFailureReason);
                            break;
                        }
                }

                service.LastCheckAt = now;
                service.NextCheckAt = now + TimeSpan.FromMinutes(service.CheckIntervalMinutes);
                await repositoryManager.Service.UpdateAsync(service, cancellationToken);

                var serviceCheck = new ServiceCheck
                {
                    ServiceId = service.Id,
                    CheckedAt = now,
                    Status = service.Status,
                    StatusCode = result.StatusCode,
                    ResponseTimeMs = sw.ElapsedMilliseconds,
                    FailureReason = service.Status == ServiceStatus.Healthy ? null : service.LastFailureReason,
                };

                await repositoryManager.ServiceCheck.CreateAsync(serviceCheck, cancellationToken);
            }
        }
    }

    private ServiceStatus CheckServiceStatus(HttpStatusCode resultStatusCode, CancellationToken cancellationToken)
    {
        return resultStatusCode switch
        {
            HttpStatusCode.NotFound => ServiceStatus.Unavailable,
            HttpStatusCode.InternalServerError => ServiceStatus.Down,
            HttpStatusCode.RequestTimeout => ServiceStatus.Down,
            _ => ServiceStatus.Unknown
        };
    }
}
