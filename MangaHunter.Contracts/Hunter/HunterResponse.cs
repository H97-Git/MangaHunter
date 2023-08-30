namespace MangaHunter.Contracts.Hunter;

using Mangadex.Models.Base;
using Mangadex.Models.Manga;

using Common;

public class HunterResponse
{
    public HunterDto? HunterDto { get; set; }
    public MangadexOldDto? MangadexDto { get; set; }
    public MangaUpdatesDto? MangaUpdatesDto { get; set; }
    public RssMangaUpdatesDto? RssMangaUpdatesDto { get; set; }
}

public class HunterResponseNew
{
    public HunterDto? HunterDto { get; set; }
    public MangaDexRoot<Manga> MangadexDto { get; set; }
    public MangaUpdatesDto? MangaUpdatesDto { get; set; }
    public RssMangaUpdatesDto? RssMangaUpdatesDto { get; set; }
}