namespace ServiceMonitor.API.Middlewares;

public class MaintenanceMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(HttpContext context,
        IConfiguration configuration)
    {
        var isMaintenanceMode =
            configuration.GetValue<bool>("MaintenanceMode:Enabled");

        if (isMaintenanceMode)
        {
            context.Response.StatusCode =
                StatusCodes.Status503ServiceUnavailable;

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Service is temporarily unavailable."
            });

            return;
        }

        await next(context);
    }
}
