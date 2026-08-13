namespace ServiceMonitor.Domain.Exceptions;

public sealed class ServiceNotFoundException(Guid serviceId)
    : NotFoundException($"Service with id {serviceId} not found");
