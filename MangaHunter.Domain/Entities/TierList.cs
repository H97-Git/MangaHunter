using MangaHunter.Domain.Primitives;

namespace MangaHunter.Domain.Entities;

public class TierList : IAuditableEntity
{
    public int Id { get; set; }
    public string Title { get; set; } = String.Empty;
    public string CreatorName { get; set; } = String.Empty;
    public string UserName { get; set; } = String.Empty;
    public ICollection<TierRow> Tiers { get; set; } = new List<TierRow>();
    public ICollection<Guid> UnlistedMangaIds { get; set; } = new List<Guid>();
    public string ShareCode { get; set; } = String.Empty;
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
    public List<Guid> AllIdsInTierList => GetAllIdsFromTierList();

    private List<Guid> GetAllIdsFromTierList()
    {
        List<Guid> mangadexIds = new();
        var mangaIdsInTiers = this.Tiers.SelectMany(row => row.MangaIds).ToList();
        if (mangaIdsInTiers is not {Count: 0})
        {
            mangadexIds.AddRange(mangaIdsInTiers);
        }

        if (this.UnlistedMangaIds is {Count: 0})
        {
            return mangadexIds;
        }

        mangadexIds.AddRange(this.UnlistedMangaIds);

        return mangadexIds;
    }
}