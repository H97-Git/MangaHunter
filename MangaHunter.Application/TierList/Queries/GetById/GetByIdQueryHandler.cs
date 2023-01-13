using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Application.Common.Interfaces.Services;
using MangaHunter.Application.TierList.Common;

using MediatR;

namespace MangaHunter.Application.TierList.Queries.GetById;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ErrorOr<TierListResult>>
{
    private readonly IMangadexService _mangadex;
    private readonly ITierListRepository _repository;

    public GetByIdQueryHandler(IMangadexService mangadex, ITierListRepository repository)
    {
        _mangadex = mangadex;
        _repository = repository;
    }

    public async Task<ErrorOr<TierListResult>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var tierlist = await _repository.GetById(request.TierListId);
        return await TierListGetQuery.Handle(_mangadex, tierlist);
    }
}