using ErrorOr;

using MangaHunter.Application.Common;
using MangaHunter.Application.Common.Errors;
using MangaHunter.Application.Common.Interfaces.Persistence;
using MangaHunter.Application.Common.Interfaces.Services;
using MangaHunter.Domain.Common;

using MediatR;

namespace MangaHunter.Application.Hunter.Commands.Create;

public class CreateCommandHandler : IRequestHandler<CreateCommand, ErrorOr<Domain.Entities.Hunter>>
{
    private readonly IMangadexService _mangadex;
    private readonly IMangaUpdatesService _mangaUpdates;
    private readonly IHunterRepository _repository;

    public CreateCommandHandler(IMangadexService mangadex, IMangaUpdatesService mangaUpdates,
        IHunterRepository repository)
    {
        _mangadex = mangadex;
        _mangaUpdates = mangaUpdates;
        _repository = repository;
    }

    public async Task<ErrorOr<Domain.Entities.Hunter>> Handle(CreateCommand command,
        CancellationToken cancellationToken)
    {
        var hunter = command.Hunter;
        var username = command.Username;
        if (await _repository.GetByMangaIdNoCaching(hunter.MangadexId, username) is not null)
        {
            return Errors.Hunter.Duplicate;
        }

        hunter.BakaId = await _repository.GetMangaUpdatesId(hunter.MangadexId) ??
                        await GetMangaUpdatesIdUnsafe(hunter.MangadexId);
        return await _repository.Add(hunter);
    }

    private async Task<long?> GetMangaUpdatesIdUnsafe(Guid mangadexId)
    {
        var mangadex = await _mangadex.GetByGuid(mangadexId);
        if (mangadex.IsError)
        {
            return null;
        }

        return null;

        /*ErrorOr<MangaUpdates> mangaUpdates =
            await _mangaUpdates.GetMangaUpdatesUnsafe(mangadex.Value.Links.MangaUpdates);

        return !mangaUpdates.IsError ? mangaUpdates.Value.SeriesId : null;*/
    }
}