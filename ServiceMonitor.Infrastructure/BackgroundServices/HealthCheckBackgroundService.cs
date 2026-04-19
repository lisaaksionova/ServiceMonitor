using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Infrastructure.BackgroundServices;

public class HealthCheckBackgroundService(
    IServiceScopeFactory scopeFactory,
    IHttpClientFactory httpClientFactory,
    ILogger<HealthCheckBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var serviceRepository = scope.ServiceProvider.GetRequiredService<IServiceRepository>();
            var incidentRepository = scope.ServiceProvider.GetRequiredService<IIncidentRepository>();

            var services = await serviceRepository.GetServicesForCheck();
            var httpClient = httpClientFactory.CreateClient();

            var now = DateTime.UtcNow;

            foreach (var service in services)
            {
                var oldStatus = service.Status;
                var newStatus = oldStatus;

                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Get, service.Endpoint);
                    using var response = await httpClient.SendAsync(request, stoppingToken);

                    logger.LogInformation(
                        "Health check {Endpoint}: {StatusCode}",
                        service.Endpoint,
                        response.StatusCode);

                    newStatus = DetermineStatus(response.StatusCode);
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Health check failed for {Endpoint}", service.Endpoint);
                    newStatus = ServiceStatus.Down;
                }

                if (oldStatus != newStatus)
                {
                    logger.LogInformation(
                        "Service {ServiceName} status changed: from {OldStatus} to {NewStatus}",
                        service.Name,
                        oldStatus,
                        newStatus);

                    service.Status = newStatus;

                    await incidentRepository.CreateAsync(new Incident
                    {
                        ServiceId = service.Id,
                        Date = now,
                        Status = IncidentStatus.Open,
                        Description = $"Service {service.Name} changed from {oldStatus} to {newStatus}"
                    });
                }

                service.NextCheckAt = now.AddMinutes(service.CheckIntervalMinutes);
            }

            await serviceRepository.Save();
            await Task.Delay(5000, stoppingToken);
        }
    }

    private static ServiceStatus DetermineStatus(HttpStatusCode statusCode)
    {
        if ((int)statusCode >= 200 && (int)statusCode < 300)
            return ServiceStatus.Healthy;

        if (statusCode == HttpStatusCode.ServiceUnavailable)
            return ServiceStatus.Down;

        return ServiceStatus.Degraded;
    }
}