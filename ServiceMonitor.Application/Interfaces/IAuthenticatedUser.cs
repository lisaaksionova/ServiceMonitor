namespace ServiceMonitor.Application.SharedServices;

public interface IAuthenticatedUser
{
    string UserId { get; }
}