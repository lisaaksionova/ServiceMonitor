using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Application.Incidents.Commands.CreateIncident;
using ServiceMonitor.Application.Incidents.Commands.DeleteIncident;
using ServiceMonitor.Application.Incidents.Commands.UpdateIncident;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Application.Incidents.Queries.GetAllIncidents;
using ServiceMonitor.Application.Incidents.Queries.GetIncidentById;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/services/{serviceId:guid}/incidents")]
[Authorize]
public class IncidentController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncidentDto>>> GetAll([FromRoute] Guid serviceId, CancellationToken cancellationToken,
        [FromQuery] int limit = 10, [FromQuery] string? cursor = null)
    {
        var incidents = await mediator.Send(new GetAllIncidentsQuery(serviceId, cursor, limit), cancellationToken);
        return Ok(incidents);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<IncidentDto>> GetById([FromRoute] Guid serviceId, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var incident = await mediator.Send(new GetIncidentByIdQuery(serviceId, id), cancellationToken);
        return Ok(incident);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromRoute] Guid serviceId, CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        request.ServiceId = serviceId;
        await mediator.Send(request, cancellationToken);
        return Created();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid serviceId, [FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteIncidentCommand(serviceId, id), cancellationToken);
        return Ok();
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromRoute] Guid serviceId, [FromBody] UpdateIncidentCommand request,
        CancellationToken cancellationToken)
    {
        request.ServiceId = serviceId;
        await mediator.Send(request, cancellationToken);
        return Ok();
    }
}
