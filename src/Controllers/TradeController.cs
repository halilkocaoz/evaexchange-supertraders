using EvaExchange.API.Application.Trades.Commands;
using EvaExchange.API.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EvaExchange.API.Controllers;

[Authorize(AuthenticationSchemes = "Bearer")]
[Route("api/v1/trade")]
public class TradeController(ISender mediatr) : ControllerBase
{
    [HttpPost("buy")]
    public async Task<IActionResult> Buy([FromBody] BuyCommand command)
    {
        var userShare = await mediatr.Send(command);
        var mapped = new UserShareResponseModel(userShare.ShareId,
            userShare.Rate,
            userShare.Share.Price,
            userShare.Rate * userShare.Share.Price);
        return Ok(mapped);
    }
    
    [HttpPost("sell")]
    public async Task<IActionResult> Sell([FromBody] SellCommand command)
    {
        var userShare = await mediatr.Send(command);
        var mapped = new UserShareResponseModel(userShare.ShareId,
            userShare.Rate,
            userShare.Share.Price,
            userShare.Rate * userShare.Share.Price);
        return Ok(mapped);
    }
}