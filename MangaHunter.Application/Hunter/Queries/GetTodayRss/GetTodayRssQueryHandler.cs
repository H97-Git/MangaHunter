using ErrorOr;

using MangaHunter.Application.Common;
using MangaHunter.Application.Common.Interfaces.Services;

using MediatR;

namespace MangaHunter.Application.Hunter.Queries.GetTodayRss;

public class GetTodayRssQueryHandler : IRequestHandler<GetTodayRssQuery, ErrorOr<TodayRss>>
{
    private readonly IMangaUpdatesService _mangaUpdates;

    public GetTodayRssQueryHandler(IMangaUpdatesService mangaUpdates)
    {
        _mangaUpdates = mangaUpdates;
    }

    public Task<ErrorOr<TodayRss>> Handle(GetTodayRssQuery request, CancellationToken cancellationToken)
        => _mangaUpdates.GetTodayRss();
    // var hunters = await _repository.GetByUsername(request.Username);
    // todayRss.Value.Results = hunters
    //     .Select(hunter => todayRss.Value.Results.Find(x => x.Record.Id.Equals(hunter.BakaId)))
    //     .Where(result => result is not null)
    //     .ToList();
}