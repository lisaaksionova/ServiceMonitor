using MediatR;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Domain.Common;

namespace ServiceMonitor.Application.Incidents.Queries.GetAllIncidents;

public class GetAllIncidentsQuery(Guid serviceId, string? cursor, int limit) : IRequest<CursorPagedList<IncidentDto>>
{
    public Guid ServiceId { get; } = serviceId;
    public string Cursor { get; set; } = cursor ?? string.Empty;
    public int Limit { get; set; } = limit;
}
