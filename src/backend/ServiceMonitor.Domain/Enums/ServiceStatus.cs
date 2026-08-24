namespace ServiceMonitor.Domain.Enums;

public enum ServiceStatus
{
    Healthy, // 2xx-3xx status codes
    Unavailable, // 4xx
    Down, // 5xx + no response/exception/timeout
    Unknown // not checked yet
}
