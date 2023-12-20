using EvaExchange.API.Application.Shares.Commands;
using EvaExchange.API.Application.Shares.Queries;
using EvaExchange.API.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaExchange.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/v1/shares")]
public class SharesController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetShares()
    {
        var shares = await mediator.Send(new GetSharesQuery());
        var mapped = shares.Select(share => new ShareResponseModel(share.Id, share.Rate, share.Price));
        return Ok(mapped);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetShare(string id)
    {
        var share = await mediator.Send(new GetShareQuery(id));
        if (share is null)
            return NotFound();

        var mapped = new ShareResponseModel(share.Id, share.Rate, share.Price);
        return Ok(mapped);
    }

    [HttpPost]
    public async Task<IActionResult> CreateShare([FromBody] CreateShareCommand command)
    {
        var share = await mediator.Send(command);
        var mapped = new ShareResponseModel(share.Id, share.Rate, share.Price);
        return CreatedAtAction(nameof(GetShare), new {id = share.Id}, mapped);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateShare([FromRoute] string id, [FromBody] UpdateShareCommand command)
    {
        command.Id = id;
        var share = await mediator.Send(command);
        if (share is null)
            return NotFound();

        var mapped = new ShareResponseModel(share.Id, share.Rate, share.Price);
        return Ok(mapped);
    }
}