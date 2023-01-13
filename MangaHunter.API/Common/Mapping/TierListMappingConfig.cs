using MangaHunter.Application.TierList;
using MangaHunter.Contracts.Common;
using MangaHunter.Contracts.TierList;
using MangaHunter.Domain.Entities;

using Mapster;

namespace MangaHunter.API.Common.Mapping;

public class TierListMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<List<Guid>, string>()
            .MapWith(src => CreateStringFromIds(src));
        config.NewConfig<string, List<Guid>>()
            .MapWith(src => CreateIdsListFromString(src));
        config.NewConfig<TierList, TierListDto>();
        config.NewConfig<TierListResult, TierListResponse>()
            .MapWith(src => CreateTierListResponse(src));
    }

    private static TierListResponse CreateTierListResponse(TierListResult src)
    {
        var tierListResponse = new TierListResponse {TierList = src.TierList.Adapt<TierListDto>()};

        var mangadex = src.Mangas
            .ConvertAll(manga => (manga, src.CoverArts.Find(c => c.MangaId.Equals(manga.Id))))
            .ConvertAll(tuple => tuple.Adapt<MangadexDto>());

        tierListResponse.MangadexDtos.AddRange(mangadex);
        
        return tierListResponse;
    }

    private static string CreateStringFromIds(IEnumerable<Guid> src)
    {
        return string.Join(",", src);
    }

    private static List<Guid> CreateIdsListFromString(string src)
    {
        List<Guid> ids = new();
        if (!string.IsNullOrEmpty(src))
            ids = src.Split(",",StringSplitOptions.RemoveEmptyEntries).ToList().ConvertAll(x => new Guid(x));
        return ids;
    }
}