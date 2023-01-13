using MangaDexSharp.Parameters.Enums;
using MangaDexSharp.Parameters.Manga;
using MangaDexSharp.Parameters.Order.Manga;

using MangaHunter.Application.Hunter;
using MangaHunter.Contracts.Common;
using MangaHunter.Contracts.Hunter;
using MangaHunter.Domain.Common;
using MangaHunter.Domain.Entities;

using Mapster;

using MangaLinks = MangaHunter.Contracts.Common.MangaLinks;
using QueryParameters = MangaHunter.Contracts.Common.QueryParameters;

namespace MangaHunter.API.Common.Mapping;

public class HunterMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<PaginationParameters, QueryParameters>();

        config.NewConfig<SearchQueryParameters, GetMangaListParameters>()
            .MapWith(src => R(src));

        config.NewConfig<(MangaSerializable?, CoverArtSerializable?), MangadexDto?>()
            .MapWith(src => ReadFromMangadex(src));

        config.NewConfig<MangaDexSharp.Objects.MangaLinks, MangaLinks>();

        config.NewConfig<Hunter, HunterResponse>()
            .Map(dest => dest.HunterDto, src => src);

        config.NewConfig<HunterResultWithPagination, HunterResponseWithPagination>();

        config.NewConfig<HunterResult, HunterResponse>()
            .Map(dest => dest.HunterDto, src => src.Hunter)
            .Map(dest => dest.MangadexDto, src => src.Mangadex) // Tuple to MangadexDto
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

    private static MangadexDto? ReadFromMangadex((MangaSerializable?, CoverArtSerializable?) src)
    {
        return MangadexMapping.MapTuple(src);
    }
}