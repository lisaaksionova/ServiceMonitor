using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Application.SharedServices;

public class AuthenticationToken(IConfiguration configuration, UserManager<User> userManager) : IAuthenticationToken
{
    public async Task<string?> GenerateToken(User user)
    {
        var secret = configuration["Jwt:Secret"];
        var issuer = configuration["Jwt:ValidIssuer"];
        var audience = configuration["Jwt:ValidAudience"];
        var expireTime = configuration["Jwt:ExpirationMinutes"];

        if (string.IsNullOrEmpty(secret) || string.IsNullOrEmpty(audience) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(expireTime))
        {
            throw new ApplicationException("Missing configuration");
        }

        var authClaims = new List<Claim>
        {
            new(
                ClaimTypes.NameIdentifier,
                user.Id),
            new(
                ClaimTypes.Name,
                user.UserName!),
            new(
                ClaimTypes.Email,
                user.Email!),
            new(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString())
        };

        var userRoles = await userManager.GetRolesAsync(user);
        authClaims.AddRange(userRoles.Select(role => new Claim(ClaimTypes.Role, role)));

        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(authClaims),
            Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(expireTime)),
            Issuer = issuer,
            Audience = audience,
            SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
        };

        var securityToken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(securityToken);
        return token;
    }
}
