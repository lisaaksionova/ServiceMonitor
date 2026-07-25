using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Constants;
using ServiceMonitor.Infrastructure.Persistence;

namespace ServiceMonitor.Infrastructure.Seeders;

public class MonitorSeeder(MonitorDbContext dbContext) : IMonitorSeeder
{
    public async Task SeedAsync()
    {
        if ((await dbContext.Database.GetPendingMigrationsAsync()).Any())
        {
            await dbContext.Database.MigrateAsync();
        }

        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Roles.Any())
            {
                var roles = GetRoles();
                dbContext.Roles.AddRange(roles);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles =
        [
            new(UserRoles.User) { NormalizedName = UserRoles.User.ToUpper() },
            new(UserRoles.Admin) { NormalizedName = UserRoles.Admin.ToUpper() }
        ];

        return roles;
    }
}
