using MangaHunter.Contracts.Common;

namespace MangaHunter.Contracts.TierList;

public class TierListResponse
{
    public TierListDto TierList { get; set; }
    public List<MangadexDto> MangadexDtos { get; set; } = new();
}