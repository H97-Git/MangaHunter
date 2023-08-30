namespace MangaHunter.Application.Hunter;

using Common;
using MangaHunter.Domain.Common;

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
    public MangaDexSharp.MangaDexRoot<MangaDexSharp.Manga> Mangadex { get; set; }
    public MangaUpdates? MangaUpdates { get; set; }
    public RssMangaUpdates? RssMangaUpdates { get; set; }
}