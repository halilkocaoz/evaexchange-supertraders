using EvaExchange.API.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaExchange.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/v1/me")]
public class MeController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var user = await mediator.Send(new GetMeQuery());
        return Ok(user);
    }
    
    [HttpGet("shares")]
    public async Task<IActionResult> GetShares()
    {
        return Ok();
    }
    
    [HttpGet("portfolio")]
    public async Task<IActionResult> GetPortfolio()
    {
        return Ok();
    }
}