using System.Text.Json.Serialization;

using MangaHunter.Contracts.Mangadex.Models.Base;

namespace MangaHunter.Contracts.Mangadex.Models.ReadMarker;

/// <summary>
/// Represents a collection of read chapter IDs
/// </summary>
public class ReadMarkerList : MangaDexRoot
{
	/// <summary>
	/// All of the chapter IDs that have been read
	/// </summary>
	[JsonPropertyName("data")]
	public string[] Data { get; set; } = Array.Empty<string>();
}
