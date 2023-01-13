namespace MangaHunter.Contracts.TierList;

public sealed class TierRowDto
{
    public int  Id { get; set; }
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    public string Color { get; set; } = "";
    public Guid Identifier { get; set; } = Guid.NewGuid();
    public List<Guid> MangaIds { get; set; } = new();
}