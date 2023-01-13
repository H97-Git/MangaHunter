using ErrorOr;

using MangaHunter.Application.Common;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.GetTodayRss;

public record GetTodayRssQuery() : IRequest<ErrorOr<TodayRss>>;