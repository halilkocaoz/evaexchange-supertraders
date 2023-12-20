using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using EvaExchange.API.Extensions;
using EvaExchange.API.Infrastructure;
using MediatR;

namespace EvaExchange.API.Application.Shares.Commands;

public record CreateShareCommand(string Id, decimal Rate, decimal Price) : IRequest<Share>;

public class CreateShareCommandHandler(IShareRepository repository, IHttpContextAccessor httpContextAccessor) : IRequestHandler<CreateShareCommand, Share>
{
    public async Task<Share> Handle(CreateShareCommand request, CancellationToken cancellationToken)
    {
        var share = await repository.GetByIdAsync(request.Id);
        if (share != null)
            throw new ApiException(400, $"Share with id {request.Id} already exists.");
        
        var userId = httpContextAccessor.HttpContext!.User.GetUserId();
        share = new Share(request.Id, request.Rate, request.Price, userId);
        await repository.AddAsync(share);
        return share;
    }
}