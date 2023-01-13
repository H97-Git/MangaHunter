using ErrorOr;

namespace MangaHunter.Application.Common.Interfaces.Services;

public interface IMangaUpdatesService
{
    Task<ErrorOr<MangaUpdates>> GetMangaUpdatesUnsafe(string? linksMangaUpdates);
    Task<ErrorOr<MangaUpdates>> GetMangaUpdatesSafe(string? seriesId);
    Task<ErrorOr<RssMangaUpdates>> GetRss(string? seriesId);
    Task<ErrorOr<TodayRss>> GetTodayRss();
}