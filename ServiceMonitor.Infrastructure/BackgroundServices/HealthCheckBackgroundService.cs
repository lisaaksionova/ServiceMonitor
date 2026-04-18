using System.Net;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Infrastructure.BackgroundServices;

public class HealthCheckBackgroundService(IServiceScopeFactory scopeFactory, IHttpClientFactory httpClientFactory, ILogger<HealthCheckBackgroundService> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var serviceRepository = scope.ServiceProvider.GetRequiredService<IServiceRepository>();
            var incidentRepository = scope.ServiceProvider.GetRequiredService<IIncidentRepository>();
            
            var services = await serviceRepository.GetAllAsync();
            var httpClient = httpClientFactory.CreateClient();

            foreach (var service in services)
            {
                try
                {
                    using var request = new HttpRequestMessage(HttpMethod.Head, service.Endpoint);
                    using var response = await httpClient.SendAsync(request, stoppingToken);

                    logger.LogInformation("Health check {Endpoint}: {StatusCode}", 
                        service.Endpoint, response.StatusCode);

                    if (response.IsSuccessStatusCode)
                    {
                        service.Status = ServiceStatus.Healthy;
                        await serviceRepository.Save();
                    }
                    else if (response.StatusCode == HttpStatusCode.ServiceUnavailable && service.Status != ServiceStatus.Down)
                    {
                        service.Status = ServiceStatus.Down;
                        await serviceRepository.Save();
                        await incidentRepository.CreateAsync(new Incident
                        {
                            ServiceId = service.Id,
                            Date = DateTime.UtcNow,
                            Status = IncidentStatus.Open,
                            Description = $"Service {service.Name} is in state {service.Status}"
                        });
                    }
                    else if(service.Status != ServiceStatus.Degraded)
                    {
                        service.Status = ServiceStatus.Degraded;
                        await  serviceRepository.Save();
                        await incidentRepository.CreateAsync(new Incident
                        {
                            ServiceId = service.Id,
                            Date = DateTime.UtcNow,
                            Status = IncidentStatus.Open,
                            Description = $"Service {service.Name} is in state {service.Status}"
                        });
                    }
                }
                catch (Exception ex)
                {
                    logger.LogWarning(ex, "Health check failed for {Endpoint}", service.Endpoint);

                    if(service.Status != ServiceStatus.Down)
                    {
                        service.Status = ServiceStatus.Down;
                        await serviceRepository.Save();
                        await incidentRepository.CreateAsync(new Incident
                        {
                            ServiceId = service.Id,
                            Date = DateTime.UtcNow,
                            Status = IncidentStatus.Open,
                            Description = $"Service {service.Name} is in state {service.Status}"
                        });
                    }
                }
            }

            await Task.Delay(5000, stoppingToken);
        }
    }
}