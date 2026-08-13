namespace ServiceMonitor.Domain.Exceptions;

public sealed class IncidentNotFoundException(Guid incidentId)
    : NotFoundException($"Incident with id {incidentId} not found");
