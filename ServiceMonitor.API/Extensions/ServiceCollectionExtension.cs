using FluentValidation.AspNetCore;
using Microsoft.OpenApi;
using ServiceMonitor.API.Middlewares;
using ServiceMonitor.API.Users;
using ServiceMonitor.Application.Users;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.API.Extensions;

public static class ServiceCollectionExtension
{
    public static void AddPresentation(this IServiceCollection services)
    {
        services.AddScoped<ErrorHandlingMiddleware>();
        services.AddFluentValidationAutoValidation();
        services.AddIdentityApiEndpoints<User>()
            .AddEntityFrameworkStores<MonitorDbContext>();
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
        
        services.AddScoped<IUserContext, UserContext>();
        services.AddHttpContextAccessor();
    }
}