using ErrorOr;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.GetById;

public record GetByIdQuery(string MangadexId, string Username, bool HasMangadex, bool HasMangaUpdates,
    bool HasMangaUpdatesRss) : IRequest<ErrorOr<HunterResultNEW>>;