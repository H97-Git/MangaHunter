namespace MangaHunter.Contracts.TierList;

public class TierListDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string CreatorName { get; set; }
    public string UserName { get; set; }
    public List<TierRowDto> Tiers { get; set; } = new();
    public List<Guid> UnlistedMangaIds { get; set; } = new();

    public string ShareCode { get; set; } = String.Empty;
    public DateTime CreatedOnUtc { get; init; }
    public DateTime? UpdatedOnUtc { get; set; }
}