using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Exceptions;
using ServiceMonitor.Domain.Interfaces;

namespace ServiceMonitor.Application.Incidents.Commands.UpdateIncident;

public class UpdateIncidentCommandHandler(IIncidentRepository incidentRepository,
    ILogger<UpdateIncidentCommandHandler> logger,
    IAuthenticatedUser authenticatedUser,
    IMapper mapper) : IRequestHandler<UpdateIncidentCommand>
{
    public async Task Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
    {
        var incident = await incidentRepository.GetByIdAsync(request.Id, authenticatedUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Incident), request.Id.ToString());
        mapper.Map(request, incident);
        await incidentRepository.SaveAsync(cancellationToken);
    }
}
