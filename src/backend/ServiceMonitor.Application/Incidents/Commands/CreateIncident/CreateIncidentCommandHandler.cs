using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Commands.CreateIncident;

public class CreateIncidentCommandHandler(
    IIncidentRepository incidentRepository,
    IMapper mapper,
    ILogger<CreateIncidentCommandHandler> logger) : IRequestHandler<CreateIncidentCommand, IncidentDto>
{
    public async Task<IncidentDto> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating incident {@Incident}", request.Description);
        var openIncidents = await incidentRepository.GetAllOpenAsync(request.ServiceId, cancellationToken);
        if (openIncidents.Any())
            throw new InvalidOperationException("Cannot create new open incident. Resolve previous.");
        var incident = mapper.Map<Incident>(request);
        await incidentRepository.CreateAsync(incident, cancellationToken);
        return mapper.Map<IncidentDto>(incident);
    }
}
