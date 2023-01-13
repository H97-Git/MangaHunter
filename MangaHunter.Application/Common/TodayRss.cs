using Newtonsoft.Json;

namespace MangaHunter.Application.Common;

public class Group
{
    [JsonProperty("name")] public string Name { get; set; }

    [JsonProperty("group_id")] public long? GroupId { get; set; }
}

public class Record
{
    [JsonProperty("id")] public long Id { get; set; }

    [JsonProperty("title")] public string Title { get; set; }

    [JsonProperty("volume")] public string Volume { get; set; }

    [JsonProperty("chapter")] public string Chapter { get; set; }

    [JsonProperty("groups")] public List<Group> Groups { get; set; }

    [JsonProperty("release_date")] public string ReleaseDate { get; set; }
}

public class Result
{
    [JsonProperty("record")] public Record Record { get; set; }
}

public class TodayRss
{
    [JsonProperty("total_hits")] public int TotalHits { get; set; }

    [JsonProperty("page")] public int Page { get; set; }

    [JsonProperty("per_page")] public int PerPage { get; set; }

    [JsonProperty("results")] public List<Result?> Results { get; set; } = new();
}