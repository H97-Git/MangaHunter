using System.Text.Json.Serialization;

namespace MangaHunter.Contracts.Mangadex.Models.Manga;

/// <summary>
/// Represents the relationship between manga
/// </summary>
public class MangaRelation
{
	/// <summary>
	/// The ID of the manga the relationship is with
	/// </summary>
	[JsonPropertyName("targetManga")]
	public string TargetManga { get; set; } = string.Empty;

	/// <summary>
	/// The type of relationship
	/// </summary>
	[JsonPropertyName("relation")]
	public Types.Relationships Relation { get; set; }
}
