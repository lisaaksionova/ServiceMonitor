using MediatR;
using Microsoft.AspNetCore.Http.Timeouts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using ServiceMonitor.Application.Auth.LoginUser;
using ServiceMonitor.Application.Auth.RegisterUser;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("ConcurrencyLimiter")]
[RequestTimeout("FiveSecondRequestTimeout")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserCommand command,
        CancellationToken cancellationToken)
    {
        var token = await mediator.Send(command, cancellationToken);

        return Ok("ACCESS_TOKEN: " + token);
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        await mediator.Send(command, cancellationToken);

        return Created();
    }
}
