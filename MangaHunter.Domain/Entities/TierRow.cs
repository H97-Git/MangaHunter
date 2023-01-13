using System.ComponentModel.DataAnnotations.Schema;

namespace MangaHunter.Domain.Entities;

public sealed class TierRow
{
    public int Id { get; set; }
    public int TierListId { get; set; }
    [ForeignKey("TierListId")] public TierList TierList { get; set; }
    public string Title { get; set; } = "";
    public string SubTitle { get; set; } = "";
    public string Color { get; set; } = "";
    public Guid Identifier { get; set; } = Guid.NewGuid();
    public ICollection<Guid> MangaIds { get; set; } = new List<Guid>();
}