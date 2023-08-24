using MangaHunter.Application.Common;
using MangaHunter.Domain.Common;

namespace MangaHunter.Application.Hunter;

public class HunterResult
{
    public Domain.Entities.Hunter Hunter { get; set; }
    public (MangaSerializable?, CoverArtSerializable?) Mangadex { get; set; }
    public MangaUpdates? MangaUpdates { get; set; }
    public RssMangaUpdates? RssMangaUpdates { get; set; }
}

public class HunterResultNEW
{
    public Domain.Entities.Hunter Hunter { get; set; }
    public MangaDexSharp.Manga? Mangadex { get; set; }
    public MangaUpdates? MangaUpdates { get; set; }
    public RssMangaUpdates? RssMangaUpdates { get; set; }
}