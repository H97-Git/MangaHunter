using MangaHunter.Contracts.Common;
using MangaHunter.Contracts.Mangadex.Models.Manga;

namespace MangaHunter.Contracts.Hunter;

public class HunterResponse
{
    public HunterDto? HunterDto { get; set; }
    public MangadexDto? MangadexDto { get; set; }
    public MangaUpdatesDto? MangaUpdatesDto { get; set; }
    public RssMangaUpdatesDto? RssMangaUpdatesDto { get; set; }
}

public class HunterResponseNew
{
    public HunterDto? HunterDto { get; set; }
    public MangaDto? MangadexDto { get; set; }
    public MangaUpdatesDto? MangaUpdatesDto { get; set; }
    public RssMangaUpdatesDto? RssMangaUpdatesDto { get; set; }
}