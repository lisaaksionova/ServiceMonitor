using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;

namespace ServiceMonitor.Application.Interfaces;

public interface IServiceHealthChecker
{
    Task<CheckServiceResult> CheckAsync(Service service, CancellationToken cancellationToken);
}
