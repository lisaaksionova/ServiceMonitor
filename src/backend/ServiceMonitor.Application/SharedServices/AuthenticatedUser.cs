using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ServiceMonitor.Application.Interfaces;

namespace ServiceMonitor.Application.SharedServices;

public class AuthenticatedUser(
    IHttpContextAccessor httpContextAccessor) : IAuthenticatedUser
{
    public string UserId =>
        httpContextAccessor.HttpContext?
            .User
            .FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException(
            "The current user is not authenticated.");
}
