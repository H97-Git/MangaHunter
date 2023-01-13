using ErrorOr;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.GetByUsername;

public record GetByUsernameQuery
(string Username, bool HasMangadex, bool HasMangaUpdates,
    bool HasMangaUpdatesRss) : IRequest<ErrorOr<List<HunterResult>>>;