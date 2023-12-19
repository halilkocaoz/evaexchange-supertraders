using EvaExchange.API.Application.Users.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EvaExchange.API.Controllers;
[Route("api/v1")]
public class UsersController(IMediator mediator) : ControllerBase
{
    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] SignInCommand command)
    {
        var token = await mediator.Send(command);
        return token == null ? BadRequest("Invalid credentials.") : Ok(token);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] SignUpCommand command)
    {
        var token = await mediator.Send(command);
        return Ok(token);
    }
}