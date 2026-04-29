using Microsoft.AspNetCore.Identity;

namespace ServiceMonitor.Domain.Entities;

public class User : IdentityUser
{
    public List<Service> Services { get; set; } = new();
}