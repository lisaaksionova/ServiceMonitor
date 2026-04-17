using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Application.Services.Commands.CreateService;
using ServiceMonitor.Application.Services.Commands.DeleteService;
using ServiceMonitor.Application.Services.Commands.UpdateService;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Application.Services.Queries.GetServiceById;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/services")]
public class ServiceController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ServiceDto>> GetById([FromRoute] int id)
    {
        var service = await mediator.Send(new GetServiceByIdQuery(id));
        return  Ok(service);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateServiceCommand command)
    {
        await mediator.Send(command);
        
        return Created();
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromBody] UpdateServiceCommand command)
    {
        await mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        await mediator.Send(new DeleteServiceCommand(id));
        return Ok();
    }
}