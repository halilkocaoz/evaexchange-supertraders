using EvaExchange.API.Data.Entities;
using EvaExchange.API.Data.Repositories;
using MediatR;

namespace EvaExchange.API.Application.Shares.Queries;

public record GetSharesQuery : IRequest<IEnumerable<Share>>;

public class GetSharesQueryHandler(IShareRepository repository) : IRequestHandler<GetSharesQuery, IEnumerable<Share>>
{
    public async Task<IEnumerable<Share>> Handle(GetSharesQuery request, CancellationToken cancellationToken)
    {
        return await repository.GetAllAsync();
    }
}

