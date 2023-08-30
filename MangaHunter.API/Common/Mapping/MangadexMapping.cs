using MangaHunter.Contracts.Common;
using MangaHunter.Domain.Common;

using Mapster;

using ContentRating = MangaHunter.Contracts.Common.ContentRating;
using LocalizedString = MangaHunter.Contracts.Common.LocalizedString;
using MangaLinks = MangaHunter.Contracts.Common.MangaLinks;
using MangaPublicationDemographic = MangaHunter.Contracts.Common.MangaPublicationDemographic;
using MangaStatus = MangaHunter.Contracts.Common.MangaStatus;
using Tag = MangaHunter.Contracts.Common.Tagg;

namespace MangaHunter.API.Common.Mapping;

public static class MangadexMapping
{
    public static MangadexOldDto? MapTuple((MangaSerializable?, CoverArtSerializable?) src)
    {
        if (src.Item1 is null)
            return null;
        var manga = src.Item1;
        return new MangadexOldDto()
        {
            Id = manga.Id,
            Title = manga.Title.Adapt<LocalizedString>(),
            AlternativeTitles = manga.AlternativeTitles.Adapt<IEnumerable<LocalizedString>>(),
            Description = manga.Description.Adapt<LocalizedString>(),
            Year = manga.Year,
            MainCoverArtId = manga.MainCoverArtId,
            LastChapter = manga.LastChapter,
            LastVolume = manga.LastVolume,
            Status = manga.Status?.Adapt<MangaStatus>(),
            ContentRating = manga.ContentRating.Adapt<ContentRating>(),
            PublicationDemographic = manga.PublicationDemographic?.Adapt<MangaPublicationDemographic>(),
            OriginalLanguage = manga.OriginalLanguage,
            Tags = manga.Tags.Adapt<IReadOnlyCollection<Tag>>(),
            Links = manga.Links.Adapt<MangaLinks>(),
            CoverArt = src.Item2?.Adapt<CoverArtDto>()
        };
    }
}