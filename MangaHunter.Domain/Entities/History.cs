using System.ComponentModel.DataAnnotations.Schema;

namespace MangaHunter.Domain.Entities;

public class HistoryRow
{
    public int Id { get; set; }
    public int HunterId { get; set; }
    [ForeignKey("HunterId")] public virtual Hunter Hunter { get; set; }
    public int? Vol { get; set; }
    public float? Chapter { get; set; }
    public DateTime? Date { get; set; }
}