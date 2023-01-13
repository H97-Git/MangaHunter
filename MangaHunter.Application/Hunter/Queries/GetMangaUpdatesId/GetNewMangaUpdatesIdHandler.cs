using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Application.Common.Interfaces.Services;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.GetMangaUpdatesId;

public class GetNewMangaUpdatesIdHandler : IRequestHandler<GetNewMangaUpdatesIdQuery, ErrorOr<long?>>
{
    private readonly IHunterRepository _repository;
    private readonly IMangadexService _mangadex;
    private readonly IMangaUpdatesService _mangaUpdates;

    public GetNewMangaUpdatesIdHandler(IHunterRepository repository, IMangadexService mangadex,
        IMangaUpdatesService mangaUpdates)
    {
        _repository = repository;
        _mangadex = mangadex;
        _mangaUpdates = mangaUpdates;
    }

    public async Task<ErrorOr<long?>> Handle(GetNewMangaUpdatesIdQuery request, CancellationToken cancellationToken)
    {
        var mangadexId = new Guid(request.MangadexId);
        
        var mangaUpdatesIdFromRepo = await _repository.GetMangaUpdatesId(mangadexId);
        if (mangaUpdatesIdFromRepo is not null && mangaUpdatesIdFromRepo != 0)
        {
            return  mangaUpdatesIdFromRepo;
        }

        var manga = await _mangadex.GetByGuid(mangadexId);
        if (manga.IsError)
        {
            return manga.FirstError;
        }

        var mangaUpdates = await _mangaUpdates.GetMangaUpdatesUnsafe(manga.Value.Links.MangaUpdates);

        if (mangaUpdates.IsError)
        {
            return mangaUpdates.FirstError;
        }

        return mangaUpdates.Value.SeriesId;

    }
}