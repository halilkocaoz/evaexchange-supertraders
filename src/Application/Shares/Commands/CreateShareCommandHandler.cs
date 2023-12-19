using System.ComponentModel.DataAnnotations;
using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using MediatR;

namespace EvaExchange.API.Application.Shares.Commands;

public record CreateShareCommand : IRequest<Share>
{
    public string Id { get; set; }
    public decimal Rate { get; set; }
    public decimal Price { get; set; }
}

public class CreateShareCommandHandler(IShareRepository repository) : IRequestHandler<CreateShareCommand, Share>
{
    public async Task<Share> Handle(CreateShareCommand request, CancellationToken cancellationToken)
    {
        var share = new Share(request.Id, request.Rate, request.Price);
        await repository.AddAsync(share);
        return share;
    }
}