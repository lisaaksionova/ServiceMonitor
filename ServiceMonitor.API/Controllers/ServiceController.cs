using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Application.Services.Dtos;
using ServiceMonitor.Application.Services.Queries.GetServiceById;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/services")]
public class ServiceController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<ServiceDto>> GetById(int id)
    {
        var service = await mediator.Send(new GetServiceByIdQuery(id));
        return  Ok(service);
    }
}