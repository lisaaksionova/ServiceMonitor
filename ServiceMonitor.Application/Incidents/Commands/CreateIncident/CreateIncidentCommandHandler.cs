using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Commands.CreateIncident;

public class CreateIncidentCommandHandler(
    IIncidentRepository repository,
    IMapper mapper,
    ILogger<CreateIncidentCommandHandler> logger) : IRequestHandler<CreateIncidentCommand>
{
    public async Task Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Creating incident {@Incident}", request.Description);
        var incident = mapper.Map<Incident>(request);
        await repository.CreateAsync(incident, cancellationToken);
    }
}
