
using System.Text.Json.Serialization;

namespace MangaHunter.BlazorServer.Common.Json;

public sealed class AnimePlanet
{
    [JsonPropertyName("user")] public User User { get; set; }

    [JsonPropertyName("export")] public Export Export { get; set; }

    [JsonPropertyName("entries")] public List<Entry> Entries { get; set; }
}

public sealed class Entry
{
    [JsonPropertyName("name")] public string Name { get; set; }

    [JsonPropertyName("status")] public string Status { get; set; }

    [JsonPropertyName("started")] public string Started { get; set; }

    [JsonPropertyName("completed")] public string Completed { get; set; }

    [JsonPropertyName("rating")] public float Rating { get; set; }

    [JsonPropertyName("ch")] public int Ch { get; set; }

    [JsonPropertyName("vol")] public int Vol { get; set; }
}

public sealed class Export
{
    [JsonPropertyName("type")] public string Type { get; set; }

    [JsonPropertyName("date")] public string Date { get; set; }

    [JsonPropertyName("version")] public string Version { get; set; }
}

public sealed class User
{
    [JsonPropertyName("name")] public string Name { get; set; }
}