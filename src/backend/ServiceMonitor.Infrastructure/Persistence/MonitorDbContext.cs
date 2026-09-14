using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Infrastructure.Persistence;

public class MonitorDbContext(DbContextOptions<MonitorDbContext> options) : IdentityDbContext<User>(options)
{
    public DbSet<Service> Services { get; set; }
    public DbSet<Incident> Incidents { get; set; }
    public DbSet<ServiceCheck> ServiceChecks { get; set; }
    public DbSet<HourlyServiceCheck> HourlyServiceChecks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Service>()
            .HasMany(s => s.Incidents)
            .WithOne(i => i.Service);

        modelBuilder.Entity<Incident>()
            .HasOne(i => i.Service)
            .WithMany(s => s.Incidents);

        modelBuilder.Entity<ServiceCheck>()
            .HasIndex(x => new { x.ServiceId, x.CheckedAt });

        modelBuilder.Entity<ServiceCheck>()
            .Property(x => x.CheckedAt)
            .HasColumnType("timestamp with time zone");

        modelBuilder.Entity<HourlyServiceCheck>()
            .HasIndex(x => new { x.ServiceId, x.Hour })
            .IsUnique();

        modelBuilder.Entity<HourlyServiceCheck>()
            .Property(x => x.Hour)
            .HasColumnType("timestamp with time zone");
    }
}
