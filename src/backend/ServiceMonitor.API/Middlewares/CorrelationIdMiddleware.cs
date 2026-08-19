namespace ServiceMonitor.API.Middlewares;

public class CorrelationIdMiddleware(RequestDelegate next,
    ILogger<CorrelationIdMiddleware> logger)
{
    private const string CorrelationIdHeaderName = "X-Correlation-Id";

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId = context.Request.Headers[CorrelationIdHeaderName].FirstOrDefault()?.Trim();
        var generated = false;

        if (string.IsNullOrWhiteSpace(correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            generated = true;
        }

        context.Request.Headers[CorrelationIdHeaderName] = correlationId;
        context.Response.Headers[CorrelationIdHeaderName] = correlationId;

        if (generated)
        {
            logger.LogDebug("Generated CorrelationId {CorrelationId} for request path {RequestPath}", correlationId, context.Request.Path);
        }
        await next(context);
    }
}
