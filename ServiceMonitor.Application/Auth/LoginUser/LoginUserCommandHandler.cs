using System.IdentityModel.Tokens.Jwt;
using System.Security.Authentication;
using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using ServiceMonitor.Domain.Entities;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace ServiceMonitor.Application.Auth.LoginUser;

public class LoginUserCommandHandler(UserManager<User> userManager,
    IConfiguration configuration,
    ILogger<LoginUserCommandHandler> logger) : IRequestHandler<LoginUserCommand, string>
{
    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogDebug("Login user {@UserEmail}", request.Email);
        var user = await userManager.FindByEmailAsync(request.Email);
        
        if(user == null)
            throw new AuthenticationException("Invalid email");
        if(!await userManager.CheckPasswordAsync(user, request.Password))
            throw new AuthenticationException("Invalid password");
        
        var userRoles = await userManager.GetRolesAsync(user);
        var authClaims = new List<Claim>
        {
            new Claim(
                ClaimTypes.NameIdentifier,
                user.Id),

            new Claim(
                ClaimTypes.Name,
                user.UserName!),

            new Claim(
                ClaimTypes.Email,
                user.Email!),

            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()),
        };
        
        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));
        
        logger.LogDebug("Generating token for user {@UserEmail}", request.Email);
        string token = GenerateToken(authClaims);
        return token;
    }

    private string GenerateToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration["JWT:Secret"]!));
        var tokenExpireMinutes = Convert.ToInt64(configuration["JWT:ExpirationMinutes"]!);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddMinutes(tokenExpireMinutes),
            Issuer = configuration["JWT:ValidIssuer"],
            Audience = configuration["JWT:ValidAudience"],
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}