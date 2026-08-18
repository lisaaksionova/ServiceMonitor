using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;

namespace ServiceMonitor.Application.SharedServices;

public class AuthenticatedUser(
    IHttpContextAccessor httpContextAccessor,
    ILogger<AuthenticatedUser> logger) : IAuthenticatedUser
{
    public string UserId
    {
        get
        {
            var user = httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            if (user != null)
            {
                return user;
            }

            logger.LogError("User {User} is not authenticated", httpContextAccessor.HttpContext?.User.Identity?.Name);
            throw new UnauthorizedAccessException("The current user is not authenticated.");

        }
    }
}
