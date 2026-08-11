namespace ServiceMonitor.Application.Services.Dtos;

public record ServiceDto(
    Guid Id,
    string Name,
    string Endpoint,
    string Status,
    string NextCheckAt,
    string LastCheckAt,
    string LastSuccessfulCheckAt);
