using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Application.Incidents.Commands.CreateIncident;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Application.Incidents.Queries.GetAllIncidents;
using ServiceMonitor.Application.Incidents.Queries.GetIncidentById;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/incidents")]
[Authorize]
public class IncidentController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncidentDto>>> GetAll(CancellationToken cancellationToken,
        [FromQuery] int limit = 10, [FromQuery] string? cursor = null)
    {
        var incidents = await mediator.Send(new GetAllIncidentsQuery(), cancellationToken);
        return Ok(incidents);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IncidentDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var incident = await mediator.Send(new GetIncidentByIdQuery(id), cancellationToken);
        return Ok(incident);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateIncidentCommand request, CancellationToken cancellationToken)
    {
        await mediator.Send(request, cancellationToken);
        return Created();
    }
}