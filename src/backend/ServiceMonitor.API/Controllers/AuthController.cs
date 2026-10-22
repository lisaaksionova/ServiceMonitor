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
    public async Task<ActionResult<string>> Register([FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        var token = await mediator.Send(command, cancellationToken);

        return Ok("ACCESS_TOKEN: " + token);
    }
}
