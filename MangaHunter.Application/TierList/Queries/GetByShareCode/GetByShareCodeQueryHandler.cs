using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Application.Common.Interfaces.Services;
using MangaHunter.Application.TierList.Common;

using MediatR;

namespace MangaHunter.Application.TierList.Queries.GetByShareCode;

public class GetByShareCodeQueryHandler : IRequestHandler<GetByShareCodeQuery, ErrorOr<TierListResult>>
{
    private readonly IMangadexService _mangadex;
    private readonly ITierListRepository _repository;

    public GetByShareCodeQueryHandler(IMangadexService mangadex, IMangaUpdatesService mangaUpdates,
        ITierListRepository repository)
    {
        _mangadex = mangadex;
        _repository = repository;
    }

    public async Task<ErrorOr<TierListResult>> Handle(GetByShareCodeQuery request, CancellationToken cancellationToken)
    {
        var tierlist = await _repository.GetByShareCode(request.ShareCode);
        return await TierListGetQuery.Handle(_mangadex, tierlist);
    }
}