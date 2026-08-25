using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceMonitor.Domain.Interfaces;
using ServiceMonitor.Infrastructure.BackgroundServices;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MonitorDatabase");
        services.AddDbContextPool<MonitorDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IRepositoryManager, RepositoryManager>();

        services.AddHttpClient();
        services.AddHostedService<HealthCheckBackgroundService>();
    }
}
