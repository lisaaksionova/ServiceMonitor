namespace ServiceMonitor.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtension).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssembly));
        services.AddAutoMapper(cfg => { }, applicationAssembly);
        services.AddValidatorsFromAssembly(applicationAssembly);
        services.AddScoped<IAuthenticatedUser, AuthenticatedUser>();
        services.AddScoped<IAuthenticationToken, AuthenticationToken>();
        services.AddScoped<IServiceHealthChecker, ServiceHealthChecker>();
        services.AddScoped<IHourlyServiceCheckAggregator, HourlyServiceCheckAggregator>();
        services.AddScoped<IDataRetentionService, DataRetentionService>();
        services.AddHttpContextAccessor();
    }
}
