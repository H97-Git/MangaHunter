using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Application.Common.Interfaces.Services;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.GetByUsernameWithPaginationQuery;

public class
    GetByUsernameWithPaginationQueryHandler : IRequestHandler<GetByUsernameWithPaginationQuery,
        ErrorOr<HunterResultWithPagination>>
{
    private readonly IMangadexService _mangadex;
    private readonly IMangaUpdatesService _mangaUpdates;
    private readonly IHunterRepository _repository;

    public GetByUsernameWithPaginationQueryHandler(IMangadexService mangadex, IMangaUpdatesService mangaUpdates,
        IHunterRepository repository)
    {
        _mangadex = mangadex;
        _mangaUpdates = mangaUpdates;
        _repository = repository;
    }

    public async Task<ErrorOr<HunterResultWithPagination>> Handle(GetByUsernameWithPaginationQuery request,
        CancellationToken cancellationToken)
    {
        var hunterListResponse =
            await _repository.GetByUsernameWithPagination(request.Username, request.PaginationParameters);
        if (hunterListResponse.Total is 0)
        {
            return hunterListResponse;
        }

        switch (request.HasMangadex)
        {
            case true when !request.HasMangaUpdates && !request.HasMangaUpdatesRss:
                await GetMangadex(hunterListResponse.List);
                return hunterListResponse;
            case true when request.HasMangaUpdates && !request.HasMangaUpdatesRss:
                await GetMangadex(hunterListResponse.List);
                await GetMangaUpdates(hunterListResponse.List);
                return hunterListResponse;
            case true when request.HasMangaUpdates & request.HasMangaUpdatesRss:
                await GetMangadex(hunterListResponse.List);
                await GetMangaUpdates(hunterListResponse.List);
                await GetRss(hunterListResponse.List);
                return hunterListResponse;
            case false when request.HasMangaUpdates && !request.HasMangaUpdatesRss:
                await GetMangaUpdates(hunterListResponse.List);
                return hunterListResponse;
            case false when !request.HasMangaUpdates && request.HasMangaUpdatesRss:
                await GetRss(hunterListResponse.List);
                return hunterListResponse;
            case false when request.HasMangaUpdates && request.HasMangaUpdatesRss:
                await GetMangaUpdates(hunterListResponse.List);
                await GetRss(hunterListResponse.List);
                return hunterListResponse;
            default:
                return hunterListResponse;
        }
    }

    private async Task GetRss(List<HunterResult> list)
    {
        foreach (var result in list)
        {
            await Task.Delay(200);
            var rssMangaUpdates = await _mangaUpdates.GetRss(result.Hunter.BakaId.ToString());
            if (!rssMangaUpdates.IsError)
            {
                result.RssMangaUpdates = rssMangaUpdates.Value;
            }
        }
    }

    private async Task GetMangaUpdates(List<HunterResult> list)
    {
        foreach (var result in list)
        {
            await Task.Delay(200);
            var mangaUpdates = await _mangaUpdates.GetMangaUpdatesSafe(result.Hunter.BakaId.ToString());
            if (!mangaUpdates.IsError)
            {
                result.MangaUpdates = mangaUpdates.Value;
            }
        }
    }

    private async Task GetMangadex(List<HunterResult> list)
    {
        var mangas = await _mangadex.GetListByGuids(list.ConvertAll(h => h.Hunter.MangadexId));
        if (mangas.IsError)
            return;

        var coverArts = await _mangadex.GetCoverList(mangas.Value.ConvertAll(m => m.MainCoverArtId));
        if (coverArts.IsError)
            return;

        foreach (var hunterResult in list)
        {
            var mangadexId = hunterResult.Hunter.MangadexId;
            var manga = mangas.Value.Find(m => m.Id.Equals(mangadexId));
            var coverArt = coverArts.Value.Find(c => c.MangaId.Equals(mangadexId));
            hunterResult.Mangadex = (manga, coverArt);
        }
    }
}