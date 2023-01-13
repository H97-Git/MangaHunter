using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Application.Common.Interfaces.Services;

using MapsterMapper;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.GetByUsername;

public class GetByUsernameQueryHandler : IRequestHandler<GetByUsernameQuery, ErrorOr<List<HunterResult>>>
{
    private readonly IMangadexService _mangadex;
    private readonly IMangaUpdatesService _mangaUpdates;
    private readonly IHunterRepository _repository;
    private readonly IMapper _mapper;

    public GetByUsernameQueryHandler(IMangadexService mangadex, IMangaUpdatesService mangaUpdates,
        IHunterRepository repository, IMapper mapper)
    {
        _mangadex = mangadex;
        _mangaUpdates = mangaUpdates;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ErrorOr<List<HunterResult>>> Handle(GetByUsernameQuery request,
        CancellationToken cancellationToken)
    {
        var hunters = await _repository.GetByUsername(request.Username);
        if (hunters.Count is 0)
        {
            return new List<HunterResult>();
        }

        var list = _mapper.Map<List<HunterResult>>(hunters);
        switch (request.HasMangadex)
        {
            case true when !request.HasMangaUpdates && !request.HasMangaUpdatesRss:
                await GetMangadex(list);
                return list;
            case true when request.HasMangaUpdates && !request.HasMangaUpdatesRss:
                await GetMangadex(list);
                await GetMangaUpdates(list);
                return list;
            case true when request.HasMangaUpdates & request.HasMangaUpdatesRss:
                await GetMangadex(list);
                await GetMangaUpdates(list);
                await GetRss(list);
                return list;
            case false when request.HasMangaUpdates && !request.HasMangaUpdatesRss:
                await GetMangaUpdates(list);
                return list;
            case false when !request.HasMangaUpdates && request.HasMangaUpdatesRss:
                await GetRss(list);
                return list;
            case false when request.HasMangaUpdates && request.HasMangaUpdatesRss:
                await GetMangaUpdates(list);
                await GetRss(list);
                return list;
            default:
                return list;
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
        {
            return;
        }

        var coverArts = await _mangadex.GetCoverList(mangas.Value.ConvertAll(m => m.MainCoverArtId));
        if (!coverArts.IsError)
        {
            foreach (var hunterResult in list)
            {
                var mangadexId = hunterResult.Hunter.MangadexId;
                var manga = mangas.Value.Find(m => m.Id.Equals(mangadexId));
                var coverArt = coverArts.Value.Find(c => c.MangaId.Equals(mangadexId));
                hunterResult.Mangadex = (manga, coverArt);
            }
        }
    }
}