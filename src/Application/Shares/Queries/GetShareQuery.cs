using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using MediatR;

namespace EvaExchange.API.Application.Shares.Queries;

public record GetShareQuery(string Id) : IRequest<Share>;

public class GetShareQueryHandler(IShareRepository repository) : IRequestHandler<GetShareQuery, Share>
{
    public Task<Share> Handle(GetShareQuery request, CancellationToken cancellationToken)
    {
        return repository.GetByIdAsync(request.Id);
    }
}