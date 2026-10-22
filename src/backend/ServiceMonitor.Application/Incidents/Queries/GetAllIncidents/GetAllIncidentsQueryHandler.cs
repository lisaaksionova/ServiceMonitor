namespace ServiceMonitor.Application.Incidents.Queries.GetAllIncidents;

public class GetAllIncidentsQueryHandler(
    ILogger<GetAllIncidentsQueryHandler> logger,
    IRepositoryManager repository,
    IMapper mapper,
    IAuthenticatedUser authenticatedUser) : IRequestHandler<GetAllIncidentsQuery, CursorPagedList<IncidentDto>>
{
    public async Task<CursorPagedList<IncidentDto>> Handle(GetAllIncidentsQuery request,
        CancellationToken cancellationToken)
    {
        logger.LogInformation("Getting all incidents for service {ServiceId}", request.ServiceId);

        var service =
            await repository.Service.GetByIdAsync(request.ServiceId, authenticatedUser.UserId, cancellationToken);
        if (service == null)
        {
            logger.LogError("Service {ServiceId} is not found.", request.ServiceId);
            throw new ServiceNotFoundException(request.ServiceId);
        }

        var incidents =
            await repository.Incident.GetAllPaginatedAsync(service.Id, request.Cursor, request.Limit,
                cancellationToken);
        var incidentDtos = new CursorPagedList<IncidentDto>(
            mapper.Map<List<IncidentDto>>(incidents.Items),
            incidents.NextCursor,
            incidents.HasMore);
        return incidentDtos;
    }
}
