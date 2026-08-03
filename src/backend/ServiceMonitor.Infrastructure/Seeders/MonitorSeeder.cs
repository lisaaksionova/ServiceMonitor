using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Constants;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Seeders;

public class MonitorSeeder(
    MonitorDbContext dbContext,
    UserManager<User> userManager,
    RoleManager<IdentityRole> roleManager) : IMonitorSeeder
{
    public async Task SeedAsync()
    {
        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        await SeedRolesAsync();
        await SeedUsersAsync();
        await SeedServicesAsync();
    }

    private async Task SeedRolesAsync()
    {
        if (await roleManager.Roles.AnyAsync())
        {
            return;
        }

        var roles = GetRoles();

        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
    }

    private static IEnumerable<IdentityRole> GetRoles()
    {
        return
        [
            new IdentityRole(UserRoles.User) { NormalizedName = UserRoles.User.ToUpperInvariant() },

            new IdentityRole(UserRoles.Admin) { NormalizedName = UserRoles.Admin.ToUpperInvariant() }
        ];
    }

    private async Task SeedUsersAsync()
    {
        if (await userManager.Users.AnyAsync())
        {
            return;
        }

        var admin = new User
        {
            UserName = "admin@servicemonitor.com", Email = "admin@servicemonitor.com", EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, "Admin123!");

        if (!result.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(error => error.Description)));
        }

        await userManager.AddToRoleAsync(admin, UserRoles.Admin);
    }

    private async Task SeedServicesAsync()
    {
        if (await dbContext.Services.AnyAsync())
        {
            return;
        }

        var admin = await userManager.FindByEmailAsync("admin@servicemonitor.com");

        if (admin is null)
        {
            throw new InvalidOperationException("Admin user was not found.");
        }

        var facebook = new Service
        {
            Name = "Facebook",
            Endpoint = "https://www.facebook.com/",
            Status = ServiceStatus.Unknown,
            CheckIntervalMinutes = 5,
            NextCheckAt = DateTime.UtcNow.AddMinutes(5),
            UserId = admin.Id
        };

        var github = new Service
        {
            Name = "GitHub",
            Endpoint = "https://github.com/",
            Status = ServiceStatus.Unknown,
            CheckIntervalMinutes = 10,
            NextCheckAt = DateTime.UtcNow.AddMinutes(10),
            UserId = admin.Id
        };

        dbContext.Services.AddRange(facebook, github);

        await dbContext.SaveChangesAsync();

        var incidents = GetIncidents(facebook, github);

        dbContext.Incidents.AddRange(incidents);

        await dbContext.SaveChangesAsync();
    }

    private static IEnumerable<Incident> GetIncidents(
        Service facebook,
        Service github)
    {
        return
        [
            new Incident
            {
                ServiceId = facebook.Id,
                Date = DateTime.UtcNow.AddDays(-2),
                Description = "Facebook was temporarily unavailable.",
                Status = IncidentStatus.Resolved,
                ResolvedAt = DateTime.UtcNow.AddDays(-1),
            },

            new Incident
            {
                ServiceId = github.Id,
                Date = DateTime.UtcNow.AddHours(-5),
                Description = "GitHub response time was higher than usual.",
                Status = IncidentStatus.Resolved,
                ResolvedAt = DateTime.UtcNow.AddDays(-1)
            }
        ];
    }
}
