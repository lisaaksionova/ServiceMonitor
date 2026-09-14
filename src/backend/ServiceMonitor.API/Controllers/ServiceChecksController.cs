using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Application.ServiceChecks.Dtos;
using ServiceMonitor.Application.ServiceChecks.Queries;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/services/{serviceId:guid}/checks")]
[Authorize]
public class ServiceChecksController(
    IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceCheckDto>>> GetAll([FromRoute] Guid serviceId, CancellationToken cancellationToken,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new GetAllServiceChecksQuery(page, pageSize, serviceId), cancellationToken);

        return Ok(result);
    }
}
