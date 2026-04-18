using AutoMapper;
using MediatR;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Commands.CreateIncident;

public class CreateIncidentCommandHandler(IIncidentRepository repository,
    IMapper mapper) : IRequestHandler<CreateIncidentCommand>
{
    public async Task Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = mapper.Map<Incident>(request);
        await repository.CreateAsync(incident);
    }
}