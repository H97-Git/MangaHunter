namespace MangaHunter.Contracts.Common;

public class QueryParameters
{
    public int Page { get; set; } = 1;
    public int Size { get; set; }
    public string? Status { get; init; }
    public bool ExcludedStatus { get; set; } = false;
    public bool HasMangadex { get; set; } = false;
    public bool HasMangaUpdates { get; set; } = false;
    public bool HasMangaUpdatesRss { get; set; } = false;
}