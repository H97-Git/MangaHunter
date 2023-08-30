namespace MangaHunter.API.Common.Mapping;

using MangaDexSharpOld.Parameters.Enums;
using MangaDexSharpOld.Parameters.Manga;
using MangaDexSharpOld.Parameters.Order.Manga;

using MangaHunter.Application.Hunter;
using MangaHunter.Contracts.Common;
using MangaHunter.Contracts.Hunter;
using MangaHunter.Domain.Common;
using MangaHunter.Domain.Entities;

using Mapster;

using MangaLinks = MangaHunter.Contracts.Common.MangaLinks;
using QueryParameters = MangaHunter.Contracts.Common.QueryParameters;

public class HunterMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaginationParameters, QueryParameters>();

        config.NewConfig<SearchQueryParameters, GetMangaListParameters>()
            .MapWith(src => R(src));

        config.NewConfig<(MangaSerializable?, CoverArtSerializable?), MangadexOldDto?>()
            .MapWith(src => ReadFromMangadex(src));

        config.NewConfig<MangaDexSharpOld.Objects.MangaLinks, MangaLinks>();

        config.NewConfig<Hunter, HunterResponse>()
            .Map(dest => dest.HunterDto, src => src);

        config.NewConfig<HunterResultWithPagination, HunterResponseWithPagination>();

        config.NewConfig<HunterResult, HunterResponse>()
            .Map(dest => dest.HunterDto, src => src.Hunter)
            .Map(dest => dest.MangadexDto, src => src.Mangadex) // Tuple to MangadexDto
            .Map(dest => dest.MangaUpdatesDto, src => src.MangaUpdates)
            .Map(dest => dest.RssMangaUpdatesDto, src => src.RssMangaUpdates);

        config.NewConfig<HunterResultNEW, HunterResponseNew>()
            .Map(dest => dest.HunterDto, src => src.Hunter)
            .Map(dest => dest.MangadexDto, src => src.Mangadex)
            .Map(dest => dest.MangaUpdatesDto, src => src.MangaUpdates)
            .Map(dest => dest.RssMangaUpdatesDto, src => src.RssMangaUpdates);
    }

    private static GetMangaListParameters R(SearchQueryParameters src)
    {
        return new GetMangaListParameters(new GetMangaListOrderParameters {FollowedCount = OrderByType.Descending})
        {
            Amount = src.Amount,
            Title = src.Title,
            ExcludeTags = src.ExcludeTags,
            IncludeTags = src.IncludeTags,
            MangaIds = src.MangaIds,
        };
    }

    private static MangadexOldDto? ReadFromMangadex((MangaSerializable?, CoverArtSerializable?) src)
    {
        return MangadexMapping.MapTuple(src);
    }
}