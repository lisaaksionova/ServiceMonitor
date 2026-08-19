using System.Text;
using System.Text.Json;
using System.Threading.RateLimiting;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using ServiceMonitor.API.Middlewares;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Infrastructure.Persistence;
using ServiceMonitor.Infrastructure.Seeders;

namespace ServiceMonitor.API.Extensions;

public static class ServiceCollectionExtensions
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
            opts.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            opts.AddFixedWindowLimiter(policyName: "FixedWindowRateLimiter", windowOpts =>
            {
                windowOpts.PermitLimit = 50;
                windowOpts.Window = TimeSpan.FromSeconds(10);
                windowOpts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                windowOpts.QueueLimit = 2;
            });

            opts.AddSlidingWindowLimiter(policyName: "SlidingWindowRateLimiter", windowOpts =>
            {
                windowOpts.PermitLimit = 50;
                windowOpts.Window = TimeSpan.FromSeconds(10);
                windowOpts.SegmentsPerWindow = 4;
                windowOpts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                windowOpts.QueueLimit = 2;
            });

            opts.AddConcurrencyLimiter(policyName: "ConcurrencyLimiter", limiterOpts =>
            {
                limiterOpts.PermitLimit = 50;
                limiterOpts.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                limiterOpts.QueueLimit = 2;
            });
        });

        services.AddRequestTimeouts(opts =>
        {
            opts.AddPolicy(policyName: "FiveSecondRequestTimeout", new RequestTimeoutPolicy
            {
                Timeout = TimeSpan.FromSeconds(5),
                TimeoutStatusCode = StatusCodes.Status503ServiceUnavailable,
                WriteTimeoutResponse = async (HttpContext context) =>
                {
                    context.Response.ContentType = "application/json";
                    var timeoutErrorResponse = new
                    {
                        ErrorMessage = "Request timeout error occurred",
                        StatusCode = StatusCodes.Status503ServiceUnavailable
                    };
                    var jsonResponse = JsonSerializer.Serialize(timeoutErrorResponse);
                    await context.Response.WriteAsync(jsonResponse);
                }
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
