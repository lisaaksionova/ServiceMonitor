namespace ServiceMonitor.Application.Interfaces;

public interface IAuthenticationToken
{
    Task<string?> GenerateToken(User user);
}
