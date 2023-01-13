namespace MangaHunter.Contracts.Hunter;

public class HunterResponseWithPagination
{
    public List<HunterResponse> List { get; init; } = new();
    public int Filtered { get; set; }
    public int Total { get; set; }

    // public void AbsorbMangaUpdates(HunterResponseWithPagination src)
    // {
    //     if (src.List.Count != this.List.Count || src.Total != this.Total || src.Filtered != this.Filtered)
    //         return;
    //     foreach (var hunterResponse in this.List)
    //     {
    //         if (hunterResponse.HunterDto == null)
    //         {
    //             continue;
    //         }
    //
    //         var hunterId = hunterResponse.HunterDto.MangadexId;
    //         var mangaUpdates = src.List.Find(x => x.HunterDto != null && x.HunterDto.MangadexId.Equals(hunterId));
    //         if (mangaUpdates is not null or {MangaUpdatesDto: null})
    //         {
    //             hunterResponse.MangaUpdatesDto = mangaUpdates.MangaUpdatesDto;
    //         }
    //     }
    // }
}