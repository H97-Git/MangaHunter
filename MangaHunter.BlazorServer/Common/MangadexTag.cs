
using System.Text.Json.Serialization;

namespace MangaHunter.BlazorServer.Common;

public sealed class MangadexTag
{
    [JsonPropertyName("id")] public string Id { get; set; }

    [JsonPropertyName("attributes")] public Attributes Attributes { get; set; }
}

public sealed class Attributes
{
    [JsonPropertyName("name")] public Name Name { get; set; }
    [JsonPropertyName("group")] public string Group { get; set; }
}

public sealed class Name
{
    [JsonPropertyName("en")] public string En { get; set; }
}