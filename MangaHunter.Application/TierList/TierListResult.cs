using MangaHunter.Domain.Common;

namespace MangaHunter.Application.TierList;

public class TierListResult
{
    public Domain.Entities.TierList TierList { get; set; }

    public List<MangaSerializable> Mangas { get; set; } = new();
    public List<CoverArtSerializable> CoverArts { get; set; } = new();
}