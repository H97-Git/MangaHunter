using System.Text.Json.Serialization;

namespace MangaHunter.Contracts.Common;

public class MangaUpdatesDto
{
    [JsonPropertyName("series_id")] public long? SeriesId { get; set; }

    [JsonPropertyName("title")] public string? Title { get; set; }

    [JsonPropertyName("url")] public string? Url { get; set; }

    [JsonPropertyName("associated")] public List<Associated> Associated { get; set; }

    [JsonPropertyName("description")] public string? Description { get; set; }

    [JsonPropertyName("image")] public Image Image { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("year")] public string? Year { get; set; }

    [JsonPropertyName("bayesian_rating")] public double? BayesianRating { get; set; }

    [JsonPropertyName("rating_votes")] public int? RatingVotes { get; set; }

    [JsonPropertyName("genres")] public List<Genre> Genres { get; set; }

    [JsonPropertyName("categories")] public List<Category> Categories { get; set; }

    [JsonPropertyName("latest_chapter")] public int? LatestChapter { get; set; }

    [JsonPropertyName("forum_id")] public long? ForumId { get; set; }

    [JsonPropertyName("status")] public string? Status { get; set; }

    [JsonPropertyName("licensed")] public bool? Licensed { get; set; }

    [JsonPropertyName("completed")] public bool? Completed { get; set; }

    [JsonPropertyName("anime")] public Anime Anime { get; set; }

    [JsonPropertyName("related_series")] public List<RelatedSeries> RelatedSeries { get; set; }

    [JsonPropertyName("authors")] public List<Author> Authors { get; set; }

    [JsonPropertyName("publishers")] public List<Publisher> Publishers { get; set; }

    [JsonPropertyName("publications")] public List<Publication> Publications { get; set; }

    [JsonPropertyName("recommendations")] public List<Recommendation> Recommendations { get; set; }

    [JsonPropertyName("category_recommendations")]
    public List<CategoryRecommendation> CategoryRecommendations { get; set; }

    [JsonPropertyName("rank")] public Rank Rank { get; set; }

    [JsonPropertyName("last_updated")] public LastUpdated LastUpdated { get; set; }
}



public class Anime
{
    [JsonPropertyName("start")] public string? Start { get; set; }

    [JsonPropertyName("end")] public string? End { get; set; }
}

public class Associated
{
    [JsonPropertyName("title")] public string? Title { get; set; }
}

public class Author
{
    [JsonPropertyName("name")] public string? Name { get; set; }

    [JsonPropertyName("author_id")] public long? AuthorId { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }
}

public class Category
{
    [JsonPropertyName("series_id")] public long? SeriesId { get; set; }

    [JsonPropertyName("category")] public string? Value { get; set; }

    [JsonPropertyName("votes")] public int? Votes { get; set; }

    [JsonPropertyName("votes_plus")] public int? VotesPlus { get; set; }

    [JsonPropertyName("votes_minus")] public int? VotesMinus { get; set; }

    [JsonPropertyName("added_by")] public long? AddedBy { get; set; }
}

public class CategoryRecommendation
{
    [JsonPropertyName("series_name")] public string? SeriesName { get; set; }

    [JsonPropertyName("series_id")] public long? SeriesId { get; set; }

    [JsonPropertyName("weight")] public int? Weight { get; set; }
}

public class Genre
{
    [JsonPropertyName("genre")] public string? Value { get; set; }
}

public class Image
{
    [JsonPropertyName("url")] public Url Url { get; set; }

    [JsonPropertyName("height")] public int? Height { get; set; }

    [JsonPropertyName("width")] public int? Width { get; set; }
}

public class LastUpdated
{
    [JsonPropertyName("timestamp")] public int? Timestamp { get; set; }

    [JsonPropertyName("as_rfc3339")] public DateTime? AsRfc3339 { get; set; }

    [JsonPropertyName("as_string?")] public string? AsString { get; set; }
}

public class Lists
{
    [JsonPropertyName("reading")] public int? Reading { get; set; }

    [JsonPropertyName("wish")] public int? Wish { get; set; }

    [JsonPropertyName("complete")] public int? Complete { get; set; }

    [JsonPropertyName("unfinished")] public int? Unfinished { get; set; }

    [JsonPropertyName("custom")] public int? Custom { get; set; }
}

public class OldPosition
{
    [JsonPropertyName("week")] public int? Week { get; set; }

    [JsonPropertyName("month")] public int? Month { get; set; }

    [JsonPropertyName("three_months")] public int? ThreeMonths { get; set; }

    [JsonPropertyName("six_months")] public int? SixMonths { get; set; }

    [JsonPropertyName("year")] public int? Year { get; set; }
}

public class Position
{
    [JsonPropertyName("week")] public int? Week { get; set; }

    [JsonPropertyName("month")] public int? Month { get; set; }

    [JsonPropertyName("three_months")] public int? ThreeMonths { get; set; }

    [JsonPropertyName("six_months")] public int? SixMonths { get; set; }

    [JsonPropertyName("year")] public int? Year { get; set; }
}

public class Publication
{
    [JsonPropertyName("publication_name")] public string? publication_name { get; set; }

    [JsonPropertyName("publisher_name")] public string? publisher_name { get; set; }

    [JsonPropertyName("publisher_id")] public long? publisher_id { get; set; }
}

public class Publisher
{
    [JsonPropertyName("publisher_name")] public string? PublisherName { get; set; }

    [JsonPropertyName("publisher_id")] public long? PublisherId { get; set; }

    [JsonPropertyName("type")] public string? Type { get; set; }

    [JsonPropertyName("notes")] public string? Notes { get; set; }
}

public class Rank
{
    [JsonPropertyName("position")] public Position Position { get; set; }

    [JsonPropertyName("old_position")] public OldPosition OldPosition { get; set; }

    [JsonPropertyName("lists")] public Lists Lists { get; set; }
}

public class RelatedSeries
{
    [JsonPropertyName("relation_id")] public int? RelationId { get; set; }

    [JsonPropertyName("relation_type")] public string? RelationType { get; set; }

    [JsonPropertyName("related_series_id")]
    public long? RelatedSeriesId { get; set; }

    [JsonPropertyName("related_series_name")]
    public string? RelatedSeriesName { get; set; }

    [JsonPropertyName("triggered_by_relation_id")]
    public int? TriggeredByRelationId { get; set; }
}

public class Recommendation
{
    [JsonPropertyName("series_name")]
    public string? SeriesName { get; set; }

    [JsonPropertyName("series_id")]
    public long? SeriesId { get; set; }

    [JsonPropertyName("weight")]
    public int? Weight { get; set; }
}

public class Url
{
    [JsonPropertyName("original")] public string? Original { get; set; }

    [JsonPropertyName("thumb")] public string? Thumb { get; set; }
}