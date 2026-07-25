using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Application.Services.Commands.CreateService;
using ServiceMonitor.Application.Services.Commands.DeleteService;
using ServiceMonitor.Application.Services.Commands.UpdateService;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Application.Services.Queries.GetAllServices;
using ServiceMonitor.Application.Services.Queries.GetServiceById;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/services")]
[Authorize]
public class ServiceController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceDto>> GetById([FromRoute] int id, CancellationToken cancellationToken)
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
    public async Task<IActionResult> Create([FromBody] CreateServiceCommand command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return Created();
    }

    [HttpPatch]
    public async Task<IActionResult> Update([FromBody] UpdateServiceCommand command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        await mediator.Send(new DeleteServiceCommand(id), cancellationToken);
        return Ok();
    }
}
