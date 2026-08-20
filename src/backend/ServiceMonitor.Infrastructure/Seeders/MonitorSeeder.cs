using Bogus;
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
    RoleManager<IdentityRole> roleManager) : ISeeder
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

        var faker = new Faker<User>()
            .RuleFor(u => u.UserName, f => f.Internet.UserName())
            .RuleFor(u => u.Email, f => f.Internet.Email())
            .RuleFor(u => u.EmailConfirmed, true);

        var users = faker.Generate(10);

        foreach (var user in users)
        {
            var result = await userManager.CreateAsync(user, "Password123!");

            if (!result.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", result.Errors.Select(e => e.Description)));
            }

            var roleResult = await userManager.AddToRoleAsync(user, UserRoles.User);

            if (!roleResult.Succeeded)
            {
                throw new InvalidOperationException(
                    string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }
        }

        var admin = new User
        {
            UserName = "admin",
            Email = "admin@servicemonitor.com",
            EmailConfirmed = true
        };

        var adminResult = await userManager.CreateAsync(admin, "Password123!");

        if (!adminResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", adminResult.Errors.Select(e => e.Description)));
        }

        var adminRoleResult = await userManager.AddToRoleAsync(admin, UserRoles.Admin);

        if (!adminRoleResult.Succeeded)
        {
            throw new InvalidOperationException(
                string.Join(", ", adminRoleResult.Errors.Select(e => e.Description)));
        }
    }

    private async Task SeedServicesAsync()
    {
        if (await dbContext.Services.AnyAsync())
        {
            return;
        }

        var userIds = await userManager.Users
            .Select(u => u.Id)
            .ToListAsync();

        var now = DateTime.UtcNow;

        var serviceData = new[]
        {
            ("Facebook", "https://www.facebook.com/"),
            ("GitHub", "https://github.com/"),
            ("Google", "https://www.google.com/"),
            ("Microsoft", "https://www.microsoft.com/"),
            ("Stack Overflow", "https://stackoverflow.com/")
        };

        var faker = new Faker<Service>()
            .RuleFor(s => s.Status, f => f.PickRandom<ServiceStatus>())
            .RuleFor(s => s.CheckIntervalMinutes, f => f.PickRandom(5, 10, 15, 30))
            .RuleFor(s => s.LastCheckAt, now)
            .RuleFor(
                s => s.NextCheckAt,
                (_, service) => now.AddMinutes(service.CheckIntervalMinutes))
            .RuleFor(s => s.LastSuccessfulCheckAt, now)
            .RuleFor(s => s.LastFailureReason, f => null)
            .RuleFor(s => s.UserId, f => f.PickRandom(userIds));

        var services = serviceData
            .Select(data =>
            {
                var service = faker.Generate();

                service.Name = data.Item1;
                service.Endpoint = data.Item2;

                return service;
            })
            .ToList();

        dbContext.Services.AddRange(services);

        await dbContext.SaveChangesAsync();

        var incidents = GetIncidents(services.ToArray());

        dbContext.Incidents.AddRange(incidents);

        await dbContext.SaveChangesAsync();
    }

    private static IEnumerable<Incident> GetIncidents(
        IEnumerable<Service> services)
    {
        var servicesList = services.ToList();
        var now = DateTime.UtcNow;

        var faker = new Faker<Incident>()
            .RuleFor(i => i.ServiceId, f => f.PickRandom(servicesList).Id)
            .RuleFor(i => i.Date, f => f.Date.Recent(30, now))
            .RuleFor(
                i => i.Description,
                f => f.PickRandom(
                    "Service was temporarily unavailable.",
                    "Service response time was higher than usual.",
                    "Health check failed.",
                    "Connection timed out.",
                    "Unexpected server error occurred."))
            .RuleFor(i => i.Status, f => f.PickRandom<IncidentStatus>())
            .RuleFor(
                i => i.ResolvedAt,
                (f, incident) => incident.Status == IncidentStatus.Resolved
                    ? incident.Date.AddHours(f.Random.Int(1, 24))
                    : null);

        return faker.Generate(20);
    }
}
