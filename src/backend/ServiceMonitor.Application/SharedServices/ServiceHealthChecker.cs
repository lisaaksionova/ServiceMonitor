using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
using static System.Net.HttpStatusCode;

namespace ServiceMonitor.Application.SharedServices;

public class ServiceHealthChecker(IHttpClientFactory httpClientFactory) : IServiceHealthChecker
{
    public async Task<CheckServiceResult> CheckAsync(Service service, CancellationToken cancellationToken)
    {
        var url = service.Endpoint;
        var now =  DateTime.UtcNow;
        using var client = httpClientFactory.CreateClient();

        var response = await client.GetAsync(url, cancellationToken);

        return response.IsSuccessStatusCode
            ? new CheckServiceResult(true, response.StatusCode)
            : new CheckServiceResult(false, response.StatusCode, service.LastFailureReason);
    }
}
