using System.Text.Json.Serialization;

using MangaHunter.Contracts.Mangadex.Helper;

namespace MangaHunter.Contracts.Mangadex.Models.Types;

/// <summary>
/// The content rating used within MD (NSFW vs SFW)
/// </summary>
[JsonConverter(typeof(MangaDexEnumParser<ContentRating>))]
public enum ContentRating
{
	/// <summary>
	/// The manga is safe for all audiences
	/// </summary>
	safe,
	/// <summary>
	/// The manga may contain sensitive content and is potentially NSFW
	/// </summary>
	suggestive,
	/// <summary>
	/// The content contains sexually explicit content
	/// </summary>
	erotica,
	/// <summary>
	/// The content is straight up porn. What is this? Nhentai? Damn.
	/// </summary>
	pornographic
}
