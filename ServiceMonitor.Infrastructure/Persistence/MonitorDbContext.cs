using Microsoft.EntityFrameworkCore;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Infrastructure.Persistence;

public class MonitorDbContext(DbContextOptions<MonitorDbContext> options) : DbContext(options)
{
    public DbSet<Service> Services { get; set; }
}