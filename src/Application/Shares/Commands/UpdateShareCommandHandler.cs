using System.Text.Json.Serialization;
using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using MediatR;

namespace EvaExchange.API.Application.Shares.Commands;

public class UpdateShareCommand : IRequest<Share>
{
    [JsonIgnore]
    public string Id { get; set; }
    public decimal Price { get; set; }
}


public class UpdateShareCommandHandler(IShareRepository shareRepository) : IRequestHandler<UpdateShareCommand, Share>
{
    public async Task<Share> Handle(UpdateShareCommand request, CancellationToken cancellationToken)
    {
        var share = await shareRepository.GetByIdAsync(request.Id);
        if (share is not null)
        {
            share.Update(request.Price);
            await shareRepository.UpdateAsync(share);
        }
        
        return share;
    }
}
