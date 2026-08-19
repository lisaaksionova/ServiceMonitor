using System.Text;
using System.Threading.RateLimiting;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
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
        services.AddSingleton<ErrorHandlingMiddleware>();
        services.AddScoped<IMonitorSeeder, MonitorSeeder>();
        services.AddFluentValidationAutoValidation();

        services.AddIdentity<User, IdentityRole>()
            .AddEntityFrameworkStores<MonitorDbContext>()
            .AddDefaultTokenProviders();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(opts =>
        {
            opts.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme { Type = SecuritySchemeType.Http, Scheme = "Bearer" });

            opts.AddSecurityRequirement(document => new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", document)] = []
            });
        });
        services.AddHttpContextAccessor();

        services.AddRateLimiter(opts =>
        {
            opts.AddFixedWindowLimiter(policyName: "FixedWindowRateLimiter", windowOpts =>
            {
                windowOpts.PermitLimit = 50;
                windowOpts.Window = TimeSpan.FromSeconds(10);
                windowOpts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                windowOpts.QueueLimit = 2;
            });
        });
    }

    public static void ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("CorsPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
    }

    public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(opts =>
    {
        opts.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        opts.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opts =>
    {
        opts.SaveToken = true;
        opts.RequireHttpsMetadata = false;

        opts.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidAudience = configuration["JWT:ValidAudience"],
            ValidIssuer = configuration["JWT:ValidIssuer"],
            ClockSkew = TimeSpan.Zero,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    configuration["JWT:Secret"]!
                )
            )
        };
    });
    }
}
