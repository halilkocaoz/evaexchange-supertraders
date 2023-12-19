using System.Text.Json.Serialization;
using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using EvaExchange.API.Extensions;
using MediatR;

namespace EvaExchange.API.Application.Shares.Commands;

public class UpdateShareCommand : IRequest<Share>
{
    [JsonIgnore]
    public string Id { get; set; }
    public decimal Price { get; set; }
}


public class UpdateShareCommandHandler(IShareRepository shareRepository, IHttpContextAccessor httpContextAccessor) : IRequestHandler<UpdateShareCommand, Share>
{
    public async Task<Share> Handle(UpdateShareCommand request, CancellationToken cancellationToken)
    {
        var share = await shareRepository.GetByIdAsync(request.Id);
        if (share is not null)
        {
            var userId = httpContextAccessor.HttpContext!.User.GetUserId();
            share.Update(request.Price, userId);
            await shareRepository.UpdateAsync(share);
        }
        
        return share;
    }
}
