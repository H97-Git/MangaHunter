using ErrorOr;

using MangaHunter.Domain.Common;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.GetByUsernameWithPaginationQuery;

public record GetByUsernameWithPaginationQuery(string Username, bool HasMangadex, bool HasMangaUpdates,
    bool HasMangaUpdatesRss, PaginationParameters PaginationParameters) :
    IRequest<ErrorOr<HunterResultWithPagination>>;