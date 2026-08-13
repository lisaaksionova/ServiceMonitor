namespace ServiceMonitor.Application.Incidents.Dtos;

public record IncidentDto(Guid Id, Guid ServiceId, string Date, string ResolvedAt, string Description, string Status);
