namespace MangaHunter.Application.Common.Mapping;

using MangaDexSharpOld.Resources;

using Hunter;

using Mapster;

public class HunterMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Domain.Entities.Hunter, HunterResult>()
            .MapWith(src => CreateHunterResult(src));

        config.NewConfig<Manga, HunterResult>()
            .Map(dest => dest.Mangadex, src => src);

        config.NewConfig<MangaDexSharp.MangaDexRoot<MangaDexSharp.Manga>, HunterResultNEW>()
            .Map(dest => dest.Mangadex, src => src);
    }

    private static HunterResult CreateHunterResult(Domain.Entities.Hunter src)
    {
        return new HunterResult {Hunter = src};
    }
}