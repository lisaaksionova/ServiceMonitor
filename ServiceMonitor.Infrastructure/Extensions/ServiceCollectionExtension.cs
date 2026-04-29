using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.BackgroundServices;
using ServiceMonitor.Infrastructure.Persistence;
using ServiceMonitor.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ServiceMonitor.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MonitorDatabase");
        services.AddDbContext<MonitorDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IServiceRepository, ServiceRepository>();
        services.AddScoped<IIncidentRepository, IncidentRepository>();

        services.AddHttpClient();
        services.AddHostedService<HealthCheckBackgroundService>();
    }
}