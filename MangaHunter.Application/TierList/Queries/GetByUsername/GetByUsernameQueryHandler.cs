using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Persistence;

using MediatR;

namespace MangaHunter.Application.TierList.Queries.GetByUsername;

public class GetByUsernameQueryHandler : IRequestHandler<GetByUsernameQuery, ErrorOr<List<Domain.Entities.TierList>>>
{
    private readonly ITierListRepository _repository;

    public GetByUsernameQueryHandler(ITierListRepository repository)
    {
        _repository = repository;
    }

    public async Task<ErrorOr<List<Domain.Entities.TierList>>> Handle(GetByUsernameQuery request,
        CancellationToken cancellationToken)
    {
        return await _repository.GetByUsername(request.Username);
    }
}