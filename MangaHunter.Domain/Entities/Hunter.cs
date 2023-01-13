using MangaHunter.Domain.Enums;
using MangaHunter.Domain.Primitives;

namespace MangaHunter.Domain.Entities;

public sealed class Hunter : IAuditableEntity
{
    public int Id { get; set; }
    public Guid MangadexId { get; set; }
    public string Username { get; set; } = String.Empty;
    public long? BakaId { get; set; }
    public ReadingStatus Status { get; set; }
    public string? Notes { get; set; }
    public List<HistoryRow> History { get; set; } = new();
    public DateTime CreatedOnUtc { get; set; }
    public DateTime? UpdatedOnUtc { get; set; }
}