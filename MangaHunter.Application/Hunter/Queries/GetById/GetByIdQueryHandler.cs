using ErrorOr;

using MangaHunter.Application.Common.Errors;
using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Application.Common.Interfaces.Services;
using MangaHunter.Domain.Common;

using MediatR;


namespace MangaHunter.Application.Hunter.Queries.GetById;

public class GetByIdQueryHandler : IRequestHandler<GetByIdQuery, ErrorOr<HunterResultNEW>>
{
    private readonly IMangadexService _mangadex;
    private readonly IMangaUpdatesService _mangaUpdates;
    private readonly IHunterRepository _repository;

    public GetByIdQueryHandler(IMangadexService mangadex, IMangaUpdatesService mangaUpdates,
        IHunterRepository repository)
    {
        _mangadex = mangadex;
        _mangaUpdates = mangaUpdates;
        _repository = repository;
    }

    public async Task<ErrorOr<HunterResultNEW>> Handle(GetByIdQuery request, CancellationToken cancellationToken)
    {
        var hunter = await _repository.GetByMangaId(new Guid(request.MangadexId), request.Username);
        if (hunter is null)
        {
            return Errors.Hunter.NotFound;
        }

        var result = new HunterResultNEW {Hunter = hunter};
        await GetMangadex(hunter, result);
        return result;
        /*
         switch (request.HasMangadex)
        {
            case true when !request.HasMangaUpdates && !request.HasMangaUpdatesRss:
                await GetMangadex(hunter, result);
                return result;
            case true when request.HasMangaUpdates && !request.HasMangaUpdatesRss:
                await GetMangadex(hunter, result);
                await GetMangaUpdates(hunter, result);
                return result;
            case true when request.HasMangaUpdates & request.HasMangaUpdatesRss:
                await GetMangadex(hunter, result);
                await GetMangaUpdates(hunter, result);
                await GetRss(hunter, result);
                return result;
            case false when request.HasMangaUpdates && !request.HasMangaUpdatesRss:
                await GetMangaUpdates(hunter, result);
                return result;
            case false when !request.HasMangaUpdates && request.HasMangaUpdatesRss:
                await GetRss(hunter, result);
                return result;
            case false when request.HasMangaUpdates && request.HasMangaUpdatesRss:
                await GetMangaUpdates(hunter, result);
                await GetRss(hunter, result);
                return result;
            default:
                return result;
        }*/
    }

    private async Task GetRss(Domain.Entities.Hunter hunter, HunterResult result)
    {
        string? seriesId = hunter.BakaId.ToString();
        var rssMangaUpdates = await _mangaUpdates.GetRss(seriesId);
        if (!rssMangaUpdates.IsError)
        {
            result.RssMangaUpdates = rssMangaUpdates.Value;
        }
    }

    private async Task GetMangaUpdates(Domain.Entities.Hunter hunter, HunterResult result)
    {
        string? seriesId = hunter.BakaId.ToString();
        var mangaUpdates = await _mangaUpdates.GetMangaUpdatesSafe(seriesId);
        if (!mangaUpdates.IsError)
        {
            result.MangaUpdates = mangaUpdates.Value;
        }
    }

    private async Task GetMangadex(Domain.Entities.Hunter hunter, HunterResultNEW result)
    {
        var manga = await _mangadex.GetByGuid(hunter.MangadexId);
        if (manga.IsError)
        {
            return;
        }

        // var coverArt = await _mangadex.GetCover(manga.Value.MainCoverArtId);
        // if (!coverArt.IsError)
        // {
        //     mangadex.Item2 = coverArt.Value;
        // }

        result.Mangadex = manga.Value;
    }
}