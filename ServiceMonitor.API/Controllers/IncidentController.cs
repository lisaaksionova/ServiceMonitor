using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Application.Incidents.Commands.CreateIncident;
using ServiceMonitor.Application.Incidents.Dtos;
using ServiceMonitor.Application.Incidents.Queries.GetAllIncidents;
using ServiceMonitor.Application.Incidents.Queries.GetIncidentById;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/incidents")]
public class IncidentController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<IncidentDto>>> GetAll()
    {
        var incidents = await mediator.Send(new GetAllIncidentsQuery());
        return Ok(incidents);
    }
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<IncidentDto>> GetById(int id)
    {
        var incident = await mediator.Send(new GetIncidentByIdQuery(id));
        return Ok(incident);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateIncidentCommand request)
    {
        await mediator.Send(request);
        return Created();
    }
}