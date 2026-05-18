using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Application.Auth.LoginUser;
using ServiceMonitor.Application.Auth.RegisterUser;

namespace ServiceMonitor.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginUserCommand command)
    {
        var token = await mediator.Send(command);
        
        return Ok("ACCESS_TOKEN: " + token);
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserCommand command)
    {
        await mediator.Send(command);
        
        return Created();
    }
}