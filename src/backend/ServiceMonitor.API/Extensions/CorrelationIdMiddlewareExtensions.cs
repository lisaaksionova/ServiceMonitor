using ServiceMonitor.API.Middlewares;

namespace ServiceMonitor.API.Extensions;

public static class CorrelationIdMiddlewareExtensions
{
    public static IApplicationBuilder UseCorrelationId(this IApplicationBuilder builder) => builder.UseMiddleware<CorrelationIdMiddleware>();
}
