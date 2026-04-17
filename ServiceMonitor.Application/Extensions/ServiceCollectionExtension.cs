using Microsoft.Extensions.DependencyInjection;

namespace ServiceMonitor.Application.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        var applicationAssembly = typeof(ServiceCollectionExtension).Assembly;
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(applicationAssembly));
        services.AddAutoMapper(cfg => { }, applicationAssembly);
    }
}