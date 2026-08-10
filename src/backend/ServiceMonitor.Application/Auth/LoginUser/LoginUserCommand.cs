using MediatR;

namespace ServiceMonitor.Application.Auth.LoginUser;

public record LoginUserCommand : IRequest<string>
{
    public required string Email { get; init; } = string.Empty;
    public required string Password { get; init; } = string.Empty;
}
