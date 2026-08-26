using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ServiceMonitor.Application.Auth.LoginUser;

public class LoginUserCommandHandler(
    UserManager<User> userManager,
    ILogger<LoginUserCommandHandler> logger,
    IAuthenticationToken authenticationToken) : IRequestHandler<LoginUserCommand, string?>
{
    public async Task<string?> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogDebug("Login user {@UserEmail}", request.Email);
        var user = await userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            logger.LogError("User {@UserEmail} not found", request.Email);
            throw new AuthenticationException("Invalid email");
        }

        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            logger.LogError("Invalid password for user {@UserEmail}", request.Email);
            throw new AuthenticationException("Invalid password");
        }
        
        logger.LogDebug("Generating token for user {@UserEmail}", request.Email);
        var token = await authenticationToken.GenerateToken(user);
        return token;
    }
}
