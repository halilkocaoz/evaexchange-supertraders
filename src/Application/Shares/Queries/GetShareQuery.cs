using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using MediatR;

namespace EvaExchange.API.Application.Shares.Queries;

public record GetShareQuery(string Id) : IRequest<Share>;

public class GetShareQueryHandler(IShareRepository repository) : IRequestHandler<GetShareQuery, Share>
{
    public async Task<Share> Handle(GetShareQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetByIdAsync(request.Id);
    }
}