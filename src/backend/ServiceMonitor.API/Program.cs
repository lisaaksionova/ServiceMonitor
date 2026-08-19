using Serilog;
using ServiceMonitor.API.Extensions;
using ServiceMonitor.API.Middlewares;
using ServiceMonitor.Application.Extensions;
using ServiceMonitor.Infrastructure.Extensions;
using ServiceMonitor.Infrastructure.Seeders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();

builder.Services.ConfigureCors();
builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<MaintenanceMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseCorrelationId();

//app.ConfigureExceptionHandler(app.Configuration, new Logger<>()); //use correct logger

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseRequestTimeouts();
app.UseRateLimiter();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IMonitorSeeder>();
    await seeder.SeedAsync();
}

app.Run();
