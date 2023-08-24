using System.Text.Json.Serialization;

using MangaHunter.Contracts.Mangadex.Helper;

namespace MangaHunter.Contracts.Mangadex.Models.Types;

/// <summary>
/// The query filter join mode
/// </summary>
[JsonConverter(typeof(MangaDexEnumParser<Mode>))]
public enum Mode
{
	/// <summary>
	/// All of the filters has to be present
	/// </summary>
	and,
	/// <summary>
	/// At least one of the filters has to be present
	/// </summary>
	or
}
