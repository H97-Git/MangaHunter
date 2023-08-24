using System.Text.Json.Serialization;

using MangaHunter.Contracts.Mangadex.Helper;

namespace MangaHunter.Contracts.Mangadex.Models.Types;

/// <summary>
/// The publication status of the manga
/// </summary>
[JsonConverter(typeof(MangaDexEnumParser<Status>))]
public enum Status
{
	/// <summary>
	/// The series is actively being released
	/// </summary>
	ongoing,
	/// <summary>
	/// The series has been completed and is no longer being released
	/// </summary>
	completed,
	/// <summary>
	/// The manga has been paused
	/// </summary>
	hiatus,
	/// <summary>
	/// The manga has been cancelled and is no longer being released
	/// </summary>
	cancelled
}
