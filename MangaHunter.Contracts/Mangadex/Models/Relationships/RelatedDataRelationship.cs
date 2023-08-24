using System.Text.Json.Serialization;

using MangaHunter.Contracts.Mangadex.Models.Manga;

namespace MangaHunter.Contracts.Mangadex.Models.Relationships;

/// <summary>
/// Represents a related item
/// </summary>
public class RelatedDataRelationship : MangaDto, IRelationship
{
	/// <summary>
	/// How the item is related 
	/// </summary>
	[JsonPropertyName("related")]
	public Types.Relationships Related { get; set; }
}
