using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using ServiceMonitor.API.Middlewares;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Infrastructure.Persistence;
using ServiceMonitor.Infrastructure.Seeders;

namespace ServiceMonitor.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddScoped<IMonitorSeeder, MonitorSeeder>();
        services.AddFluentValidationAutoValidation();

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<MonitorDbContext>()
            .AddDefaultTokenProviders();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme { Type = SecuritySchemeType.Http, Scheme = "Bearer" });

            c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });
        services.AddHttpContextAccessor();
    }
}
