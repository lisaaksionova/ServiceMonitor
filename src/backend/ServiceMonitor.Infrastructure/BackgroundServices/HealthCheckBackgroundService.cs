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
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();

            var repository = scope.ServiceProvider.GetRequiredService<IRepositoryManager>();

            var services = await repository.Service.GetServicesForCheck(cancellationToken);
            var httpClient = httpClientFactory.CreateClient();

            foreach (var service in services)
            {
                var oldStatus = service.Status;
                var newStatus = oldStatus;

                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Get, service.Endpoint);
                    using var response = await httpClient.SendAsync(request, cancellationToken);

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

                    if (newStatus != ServiceStatus.Healthy)
                    {
                        await repository.Incident.CreateAsync(
                            new Incident
                            {
                                ServiceId = service.Id,
                                Date = DateTime.UtcNow,
                                Status = IncidentStatus.Open,
                                Description = $"Service {service.Name} changed from {oldStatus} to {newStatus}"
                            }, cancellationToken);
                    }
                    else
                    {
                        var openIncident = await repository.Incident.GetAllOpenAsync(service.Id, cancellationToken);
                        foreach (var incident in openIncident)
                        {
                            incident.Status = IncidentStatus.Resolved;
                        }

                        await repository.Incident.SaveAsync(cancellationToken);
                    }
                }

                service.NextCheckAt = DateTime.UtcNow.AddMinutes(service.CheckIntervalMinutes);
            }

            await repository.Service.Save(cancellationToken);
            await Task.Delay(5000, cancellationToken);
        }
    }

    private static ServiceStatus DetermineStatus(HttpStatusCode statusCode)
    {
        if ((int)statusCode >= 200 && (int)statusCode < 300)
        {

            return ServiceStatus.Healthy;
        }

        if (statusCode == HttpStatusCode.ServiceUnavailable)
        {
            return ServiceStatus.Down;
        }

        return ServiceStatus.Unavailable;
    }
}
