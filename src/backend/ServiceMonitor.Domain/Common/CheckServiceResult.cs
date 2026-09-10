using System.Net;

namespace ServiceMonitor.Domain.Common;

public record CheckServiceResult(bool IsHealthy, HttpStatusCode StatusCode, string? FailureReason = null);
