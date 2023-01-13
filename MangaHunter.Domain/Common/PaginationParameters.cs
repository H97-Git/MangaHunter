namespace MangaHunter.Domain.Common;

public class PaginationParameters
{
    private const int MaxPageSize = 100;

    public int Page
    {
        get => _page;
        init => _page = (value <= 0) ? 1 : value;
    }

    public int Size
    {
        get => _size;
        init => _size = (value <= 0) ? 1 : (value >= MaxPageSize) ? MaxPageSize : value;
    }

    private readonly int _size = 10;
    private readonly int _page = 1;

    public string? Status { get; init; }
    public bool ExcludedStatus { get; set; } = false;
}