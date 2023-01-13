using ErrorOr;

using MangaHunter.Application.Common.Interfaces.Services;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.Search;

public class SearchQueryHandler : IRequestHandler<SearchQuery, ErrorOr<List<HunterResult>>>
{
    private readonly IMangadexService _mangadex;

    public SearchQueryHandler(IMangadexService mangadex)
    {
        _mangadex = mangadex;
    }

    public async Task<ErrorOr<List<HunterResult>>> Handle(SearchQuery request, CancellationToken cancellationToken)
    {
        var mangas = await _mangadex.Search(request.Parameters);
        if (mangas.IsError)
        {
            return mangas.FirstError;
        }

        var coverArts = await _mangadex.GetCoverList(mangas.Value.ConvertAll(x => x.MainCoverArtId));
        if (coverArts.IsError)
        {
            return coverArts.FirstError;
        }

        return mangas.Value
            .ConvertAll(manga =>
            {
                var hunter = new Domain.Entities.Hunter {MangadexId = manga.Id};
                var coverArt = coverArts.Value.Find(x => x.MangaId.Equals(manga.Id));
                var mangadex = (manga, coverArt);
                return new HunterResult {Hunter = hunter, Mangadex = mangadex};
            });
    }
}