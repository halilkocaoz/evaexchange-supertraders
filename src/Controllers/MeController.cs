using EvaExchange.API.Application.Users.Queries;
using EvaExchange.API.Models;
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
        var userShares = await mediator.Send(new GetMeSharesQuery());
        var mappedShares = userShares.Select(share => new ShareResponseModel(share.Id, share.Rate, share.Price));
        var mappedUser = new UserResponseModel(user.Email, user.FullName, user.Balance);
        return Ok(new {user = mappedUser, shares = mappedShares});
    }

    [HttpGet("shares")]
    public async Task<IActionResult> GetShares()
    {
        var shares = await mediator.Send(new GetMeSharesQuery());
        var mapped = shares.Select(share => new ShareResponseModel(share.Id, share.Rate, share.Price));
        return Ok(mapped);
    }

    [HttpGet("portfolio")]
    public async Task<IActionResult> GetPortfolio()
    {
        var portfolio = await mediator.Send(new GetMePortfolioQuery());
        var mapped = portfolio.Select(x => new UserShareResponseModel(x.Share.Id,
            x.Rate,
            x.Share.Price,
            x.Rate * x.Share.Price));
        return Ok(mapped);
    }
}