using MangaHunter.Contracts.Common;

namespace MangaHunter.Contracts.Hunter;

public class HunterResponse
{
    public HunterDto? HunterDto { get; set; }
    public MangadexDto? MangadexDto { get; set; }
    public MangaUpdatesDto? MangaUpdatesDto { get; set; }
    public RssMangaUpdatesDto? RssMangaUpdatesDto { get; set; }
}


