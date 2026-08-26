using System.Security.Claims;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Application.Interfaces;

public interface IAuthenticationToken
{
    Task<string?> GenerateToken(User user);
}
