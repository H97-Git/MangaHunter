using MangaDexSharp.Resources;

using MangaHunter.Application.Hunter;

using Mapster;

namespace MangaHunter.Application.Common.Mapping;

public class HunterMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Entities.Hunter, HunterResult>()
            .MapWith(src => CreateHunterResult(src));

        config.NewConfig<Manga, HunterResult>()
            .Map(dest => dest.Mangadex, src => src);
    }

    private static HunterResult CreateHunterResult(Domain.Entities.Hunter src)
    {
        return new HunterResult {Hunter = src};
    }
}