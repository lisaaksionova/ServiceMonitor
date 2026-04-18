using ServiceMonitor.API.Middlewares;

namespace ServiceMonitor.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<ErrorHandlingMiddleware>();
    }
}