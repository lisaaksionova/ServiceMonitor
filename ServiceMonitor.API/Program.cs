using ServiceMonitor.API.Extensions;
using ServiceMonitor.API.Middlewares;
using ServiceMonitor.Application.Extensions;
using ServiceMonitor.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddPresentation();

builder.Services.AddOpenApi();

var app = builder.Build();

app.MapControllers();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseMiddleware<ErrorHandlingMiddleware>();

app.Run();