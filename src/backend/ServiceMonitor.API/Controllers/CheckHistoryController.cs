using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Application.History.Dtos.HourlyServiceChecks;
using ServiceMonitor.Application.History.Queries.HourlyServiceCheck.GetAll;
using ServiceMonitor.Application.ServiceChecks.Dtos;
using ServiceMonitor.Application.ServiceChecks.Queries;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/services/{serviceId:guid}/history")]
[Authorize]
public class CheckHistoryController(
    IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ServiceCheckDto>>> GetAllServiceChecks([FromRoute] Guid serviceId,
        CancellationToken cancellationToken,
        [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new GetAllServiceChecksQuery(page, pageSize, serviceId), cancellationToken);

        return Ok(result);
    }

    [HttpGet("hourly")]
    public async Task<ActionResult<IEnumerable<HourlyServiceCheckDto>>> GetAllHourlyServiceChecks(
        [FromRoute] Guid serviceId, [FromQuery] DateTime from, [FromQuery] DateTime to,
        CancellationToken cancellationToken, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var result = await mediator.Send(new GetAllHourlyServiceChecksQuery(page, pageSize, serviceId, from, to),
            cancellationToken);

        return Ok(result);
    }
}
