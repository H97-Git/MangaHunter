namespace MangaHunter.Application.Common.Interfaces.Services;

using ErrorOr;

using MangaDexSharp;

using MangaDexSharpOld.Parameters.Manga;

using MangaHunter.Domain.Common;

public interface IMangadexService
{
    Task<ErrorOr<CoverArtSerializable>> GetCover(Guid mangaId);
    Task<ErrorOr<List<CoverArtSerializable>>> GetCoverList(List<Guid> coverArtIds);
    Task<ErrorOr<MangaDexSharp.MangaDexRoot<Manga>>> GetByGuid(Guid mangaId);
    Task<ErrorOr<List<MangaSerializable>>> GetListByGuids(List<Guid> mangaIds);
    Task<ErrorOr<List<MangaSerializable>>> Search(GetMangaListParameters mangaListParameters);
}