using MangaHunter.Contracts.Common;

namespace MangaHunter.Contracts.Hunter;

public class SearchQueryParameters
{
    public string? Title { get; set; }
    public int? Amount { get; set; }

    public ICollection<Guid>? Authors { get; set; }

    public ICollection<Guid>? Artists { get; set; }

    public int? Year { get; set; }

    public ICollection<Guid>? IncludeTags { get; set; }

    public IncludeTagsMode? IncludeTagsMode { get; set; }

    public ICollection<Guid>? ExcludeTags { get; set; }

    public ExcludeTagsMode? ExcludeTagsMode { get; set; }

    public ICollection<MangaStatus>? Status { get; set; }

    public ICollection<string>? OriginalLanguages { get; set; }

    public ICollection<string>? ExcludedOriginalLanguages { get; set; }

    public ICollection<string>? AvalibileTranslatedLanguages { get; set; }

    public ICollection<MangaPublicationDemographic>? PublicationDemographics { get; set; }

    public ICollection<ContentRating>? ContentRatings { get; set; }

    public ICollection<Guid>? MangaIds { get; set; }

    public DateTime? CreatedAtSince { get; set; }

    public DateTime? UpdatedAtSince { get; set; }

    public Func<string?>? ToQueryString;

}

public enum ExcludeTagsMode
{
    And,
    Or
}

public enum IncludeTagsMode
{
    And,
    Or
}