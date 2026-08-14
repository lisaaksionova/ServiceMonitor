using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Application.Services.Commands.CreateService;
using ServiceMonitor.Application.Services.Commands.DeleteService;
using ServiceMonitor.Application.Services.Commands.UpdateService;
using ServiceMonitor.Application.Services.Commands.UpdateServiceAsHealthy;
using ServiceMonitor.Application.Services.Commands.UpdateServiceAsUnavailable;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Application.Services.Queries.GetAllServices;
using ServiceMonitor.Application.Services.Queries.GetServiceById;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ServicesController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ServiceDto>> GetById([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var service = await mediator.Send(new GetServiceByIdQuery(id), cancellationToken);
        return Ok(service);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAll(CancellationToken cancellationToken,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var services = await mediator.Send(new GetAllServicesQuery(page, pageSize), cancellationToken);
        return Ok(services);
    }

    [HttpPost]
    public async Task<ActionResult<ServiceDto>> Create([FromBody] CreateServiceCommand command,
        CancellationToken cancellationToken)
    {
        var service = await mediator.Send(command, cancellationToken);

        return CreatedAtAction(nameof(GetById), new { id = service.Id }, service);
    }

    [HttpPut]
    public async Task<ActionResult<ServiceDto>> Update([FromBody] UpdateServiceCommand command,
        CancellationToken cancellationToken)
    {
        var service = await mediator.Send(command, cancellationToken);
        return Ok(service);
    }

    [HttpPatch("healthy/{id:guid}")]
    public async Task<ActionResult<ServiceDto>> UpdateServiceAsHealthy([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var service = await mediator.Send(new UpdateServiceAsHealthyCommand(id), cancellationToken);
        return Ok(service);
    }

    [HttpPatch("unavailable/{id:guid}")]
    public async Task<ActionResult<ServiceDto>> UpdateServiceAsUnavailable([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        var service = await mediator.Send(new UpdateServiceAsUnavailableCommand(id), cancellationToken);
        return Ok(service);
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteServiceCommand(id), cancellationToken);
        return Ok();
    }
}
