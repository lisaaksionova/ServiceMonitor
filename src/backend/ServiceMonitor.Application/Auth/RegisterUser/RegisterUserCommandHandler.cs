using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Constants;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Application.Auth.RegisterUser;

public class RegisterUserCommandHandler(
    UserManager<User> userManager,
    ILogger<RegisterUserCommandHandler> logger,
    IAuthenticationToken authenticationToken) : IRequestHandler<RegisterUserCommand, string?>
{
    public async Task<string?> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogDebug("Creating user {@UserEmail}", request.Email);
        var userExists = await userManager.FindByEmailAsync(request.Email);
        if (userExists != null)
        {
            logger.LogWarning("User {@UserEmail} is already registered", request.Email);
            throw new InvalidOperationException("User already exists"); //handle in extension
        }

        var user = new User
        {
            Email = request.Email,
            UserName = request.Email.Split('@')[0],
            SecurityStamp = Guid.NewGuid().ToString()
        };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            logger.LogError("User {@UserEmail} creation failed", request.Email);
            throw new InvalidOperationException($"Failed to create user: {createResult.Errors.First().Description}");
        }

        await userManager.AddToRoleAsync(user, UserRoles.User);

        logger.LogDebug("Generating token for user {@UserEmail}", request.Email);
        var token = await authenticationToken.GenerateToken(user);
        return token;
    }
}
