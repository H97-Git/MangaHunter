namespace MangaHunter.Application.Hunter;

public class HunterResultWithPagination
{
    public List<HunterResult> List { get; init; } = new();
    public int Filtered { get; set; }
    public int Total { get; set; }
}