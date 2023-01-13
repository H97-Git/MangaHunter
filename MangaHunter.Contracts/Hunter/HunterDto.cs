namespace MangaHunter.Contracts.Hunter;

public class HunterDto
{
    public int Id { get; set; }
    public string Username { get; set; }
    public Guid MangadexId { get; set; }
    public long BakaId { get; set; }
    public ReadingStatusDto Status { get; set; }
    public string? Notes { get; set; }
    public List<HistoryRowDto> History { get; set; } = new();
}

public class HistoryRowDto
{
    public int Id { get; set; }
    public int? Vol { get; set; }
    public float? Chapter { get; set; }
    public DateTime? Date { get; set; }
}

public enum ReadingStatusDto
{
    WantToRead = 0,
    Reading = 1,
    Dropped = 2,
    Read = 3,
}