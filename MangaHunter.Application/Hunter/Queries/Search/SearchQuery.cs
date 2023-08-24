using ErrorOr;

using MangaDexSharpOld.Parameters.Manga;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.Search;

public record SearchQuery(GetMangaListParameters Parameters):IRequest<ErrorOr<List<HunterResult>>>;