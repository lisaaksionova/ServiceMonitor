using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using ServiceMonitor.Application.Interfaces;
using ServiceMonitor.Domain.Entities;
using ServiceMonitor.Domain.Enums;
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
        logger.LogInformation("Updating incident {@Incident}", request.Description);
        var openIncidents = await incidentRepository.GetAllOpenAsync(request.ServiceId, cancellationToken);
        if (openIncidents.Any() && Enum.TryParse<IncidentStatus>(
                                    request.Status,
                                    ignoreCase: true,
                                    out var status)
                                && status == IncidentStatus.Open)
            throw new InvalidOperationException("Cannot create new open incident. Resolve previous.");
        var incident = await incidentRepository.GetByIdAsync(request.ServiceId, request.Id, authenticatedUser.UserId, cancellationToken)
            ?? throw new NotFoundException(nameof(Incident), request.Id.ToString());
        mapper.Map(request, incident);
        await incidentRepository.SaveAsync(cancellationToken);
    }
}
