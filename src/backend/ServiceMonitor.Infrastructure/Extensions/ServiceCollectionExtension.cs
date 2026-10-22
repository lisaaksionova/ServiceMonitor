namespace ServiceMonitor.Infrastructure.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("MonitorDatabase");
        services.AddDbContext<MonitorDbContext>(options => options.UseNpgsql(connectionString));

        services.AddScoped<IRepositoryManager, RepositoryManager>();
        services.AddScoped<IDataMaintenanceBackgroundJob, DataMaintenanceBackgroundJob>();

        services.AddHttpClient();
        services.AddHostedService<HealthCheckBackgroundService>();

        services.AddHangfire(x => x.UsePostgreSqlStorage(configuration.GetConnectionString("MonitorDatabase")));
        services.AddHangfireServer();
    }
}
