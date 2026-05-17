using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi;
using ServiceMonitor.API.Middlewares;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddFluentValidationAutoValidation();
        services.AddIdentityCore<User>()
            .AddEntityFrameworkStores<MonitorDbContext>()
            .AddApiEndpoints();
        services.AddAuthentication().AddBearerToken(IdentityConstants.BearerScheme);
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
            });
            
            c.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });
        services.AddHttpContextAccessor();
    }
}