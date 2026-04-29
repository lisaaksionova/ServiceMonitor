namespace ServiceMonitor.Application.Users;

public interface IUserContext
{
    CurrentUser? GetCurrentUser();
}