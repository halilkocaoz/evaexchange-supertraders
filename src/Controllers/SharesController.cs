using EvaExchange.API.Application.Shares.Commands;
using EvaExchange.API.Application.Shares.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaExchange.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/v1/[controller]")]
public class SharesController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetShares()
    {
        var shares = await mediator.Send(new GetSharesQuery());
        return Ok(shares);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetShare(string id)
    {
        var share = await mediator.Send(new GetShareQuery(id));
        return share is null ? NotFound() : Ok(share);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateShare([FromBody] CreateShareCommand command)
    {
        var share = await mediator.Send(command);
        return CreatedAtAction(nameof(GetShare), new {id = share.Id}, share);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShare([FromRoute]string id, [FromBody] UpdateShareCommand command)
    {
        command.Id = id;
        var share = await mediator.Send(command);
        return share is null ? NotFound() : Ok(share);
    }
}