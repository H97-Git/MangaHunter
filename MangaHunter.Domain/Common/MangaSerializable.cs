namespace MangaHunter.Domain.Common;

public class MangaSerializable
{
    public Guid Id { get; set; }
    public LocalizedString Title { get; set; }
    public IEnumerable<LocalizedString> AlternativeTitles { get; set; }
    public Guid MainCoverArtId { get; set; }
    public LocalizedString Description { get; set; }
    public MangaStatus? Status { get; set; }
    public string? LastChapter { get; set; }
    public string? LastVolume { get; set; }
    public string OriginalLanguage { get; set; }
    public MangaLinks Links { get; set; }
    public ContentRating ContentRating { get; set; }
    public MangaPublicationDemographic? PublicationDemographic { get; set; }
    public IReadOnlyCollection<Tag> Tags { get; set; }
    public int? Year { get; set; }
}

public class LocalizedString : Dictionary<string, string>
{
    private string Default
    {
        get
        {
            if (Count == 0) return string.Empty;
            return First.Value;
        }
    }

    private string? English => this[LanguageKeys.English];

    public string EnglishOrDefault
    {
        get
        {
            return ContainsKey(LanguageKeys.English) ? English : Default;
        }
    }

    public KeyValuePair<string, string> First => this.First();

    public IEnumerable<string> Languages => Keys;

    public bool HasEnglish => ContainsKey(LanguageKeys.English);

    public bool HasLanguage(string lang)
    {
        return ContainsKey(lang);
    }
}

public class LanguageKeys
{
    public const string ChineseRomanized = "zh-ro";
    public const string ChineseSimplified = "zh";
    public const string ChineseTraditional = "zh-hk";
    public const string English = "en";
    public const string French = "fr";
    public const string Hungarian = "hu";
    public const string Italian = "it";
    public const string Korean = "kr";
    public const string KoreanRomanized = "kr-ro";
    public const string Japanese = "ja";
    public const string JapaneseRomanized = "ja-ro";
    public const string PortugueseBrasilian = "pt-br";
    public const string Russian = "ru";
    public const string SpanishCastilian = "es";
    public const string SpanishLatinAmerican = "es-la";
    public const string Vietnamese = "vi";
}

public class MangaLinks
{
    public string? Anilist { get; set; }

    public string? AnimePlanet { get; set; }

    public string? BookwalkerJp { get; set; }

    public string? MangaUpdates { get; set; }
    public string? NovelUpdates { get; set; }

    public string? KitsuIo { get; set; }


    public string? Amazon { get; set; }

    public string? EBookJapan { get; set; }

    public string? MyAnimeList { get; set; }


    public string? Raw { get; set; }

    public string? EnglishTranslation { get; set; }
}

public enum ContentRating
{
    Safe,
    Suggestive,
    Erotica,
    Pornographic
}

public enum MangaStatus
{
    None,
    Ongoing,
    Completed,
    Hiatus,
    Cancelled
}

public enum MangaPublicationDemographic
{
    Shounen = 1,
    Shoujo,
    Josei,
    Seinen,
    None = 0
}

public class Tag
{
    public TagGroup Group { get; set; }
    public LocalizedString Name { get; set; }
    public LocalizedString Description { get; set; }
}

public enum TagGroup
{
    Genre,
    Theme,
    Format,
    Content
}