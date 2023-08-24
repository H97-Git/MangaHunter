using System.Text.Json.Serialization;

using MangaHunter.Contracts.Mangadex.Helper;

namespace MangaHunter.Contracts.Mangadex.Models.Base;

/// <summary>
/// Represents a collection of localized (translated) strings
/// </summary>
[JsonConverter(typeof(MangaDexDictionaryParser))]
public class Localization : Dictionary<string, string> { }
