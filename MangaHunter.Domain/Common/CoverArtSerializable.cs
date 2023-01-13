namespace MangaHunter.Domain.Common;

public class CoverArtSerializable
{
    public string? Description { get; set; }

    public string FileName { get; set; }

    public Guid MangaId { get; set; }

    public Uri SourceUrl => new(string.Format(CoverUrls.Source, MangaId, FileName));

    public Guid UploaderId { get; set; }

    public Uri Thumbnail256Px => new(string.Format(CoverUrls.Thumbnail256Px, MangaId, FileName));

    public Uri Thumbnail512Px => new(string.Format(CoverUrls.Thumbnail512Px, MangaId, FileName));

    public string? Volume { get; set; }
}

public class CoverUrls
{
    public const string Source = "https://uploads.mangadex.org/covers/{0}/{1}";
    public const string Thumbnail256Px = "https://uploads.mangadex.org/covers/{0}/{1}.256.jpg";
    public const string Thumbnail512Px = "https://uploads.mangadex.org/covers/{0}/{1}.512.jpg";
}