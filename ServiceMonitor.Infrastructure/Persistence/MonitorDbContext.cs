using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Infrastructure.Persistence;

public class MonitorDbContext(DbContextOptions<MonitorDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Service> Services { get; set; }
    public DbSet<Incident> Incidents { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Service>()
            .HasMany(s => s.Incidents)
            .WithOne(i => i.Service);
        
        modelBuilder.Entity<Incident>()
            .HasOne(i => i.Service)
            .WithMany(s => s.Incidents);
    }
}