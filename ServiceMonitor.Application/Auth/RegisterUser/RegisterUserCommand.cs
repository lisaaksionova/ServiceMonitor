using MediatR;

namespace ServiceMonitor.Application.Auth.RegisterUser;

public record RegisterUserCommand : IRequest
{
    public required string Email { get; init; } =  string.Empty;
    public required string Password { get; init; } =  string.Empty;
}