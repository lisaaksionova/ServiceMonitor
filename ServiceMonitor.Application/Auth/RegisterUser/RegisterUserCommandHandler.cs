using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Domain.Constants;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Application.Auth.RegisterUser;

public class RegisterUserCommandHandler(
    UserManager<User> userManager,
    Logger<RegisterUserCommandHandler> logger) : IRequestHandler<RegisterUserCommand>
{
    public async Task Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        logger.LogDebug("Creating user {@UserEmail}", request.Email);
        var userExists = await userManager.FindByEmailAsync(request.Email);
        if (userExists != null)
        {
            throw new InvalidOperationException("User already exists"); //handle in extension
        }

        User user = new() { Email = request.Email, UserName = request.Email.Split('@')[0] };

        var createResult = await userManager.CreateAsync(user, request.Password);
        if (!createResult.Succeeded)
        {
            throw new InvalidOperationException($"Failed to create user: {createResult.Errors.First().Description}");
        }

        await userManager.AddToRoleAsync(user, UserRoles.User);
    }
}
