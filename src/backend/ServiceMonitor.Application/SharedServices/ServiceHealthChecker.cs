using System.Net.Sockets;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Common;
using ServiceMonitor.Domain.Entities;
using static System.Net.HttpStatusCode;

namespace ServiceMonitor.Application.SharedServices;

public class ServiceHealthChecker(IHttpClientFactory httpClientFactory) : IServiceHealthChecker
{
    public async Task<CheckServiceResult> CheckAsync(Service service, CancellationToken cancellationToken)
    {
        var url = service.Endpoint;
        using var client = httpClientFactory.CreateClient();

        HttpResponseMessage response;
        try
        {
            response = await client.GetAsync(url, cancellationToken);
        }
        catch (HttpRequestException e) when (e.InnerException is SocketException sx)
        {
            switch (sx.SocketErrorCode)
            {
                case SocketError.HostNotFound:
                    return new CheckServiceResult(
                        false,
                        NotFound,
                        "Host not found");

                case SocketError.ConnectionRefused:
                    return new CheckServiceResult(
                        false,
                        ServiceUnavailable,
                        "Connection refused");

                case SocketError.HostUnreachable:
                    return new CheckServiceResult(
                        false,
                        ServiceUnavailable,
                        "Host unreachable");
                default:
                    return new CheckServiceResult(false, InternalServerError, e.Message);

            }
        }
        catch (OperationCanceledException e) when (!cancellationToken.IsCancellationRequested)
        {
            return new CheckServiceResult(false, RequestTimeout, e.Message);
        }

        return response.IsSuccessStatusCode
            ? new CheckServiceResult(true, response.StatusCode)
            : new CheckServiceResult(false, response.StatusCode, response.ReasonPhrase);
    }
}
