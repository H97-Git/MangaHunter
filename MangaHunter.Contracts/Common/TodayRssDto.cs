namespace MangaHunter.Contracts.Common;

public class Group
{
    public string Name { get; set; }

    public long? GroupId { get; set; }
}

public class Record
{
    public long Id { get; set; }

    public string Title { get; set; }

    public string Volume { get; set; }

    public string Chapter { get; set; }

    public List<Group> Groups { get; set; }

    public string ReleaseDate { get; set; }
}

public class Result
{
    public Record Record { get; set; }
}

public class TodayRssDto
{
    public int TotalHits { get; set; }

    public int Page { get; set; }

    public int PerPage { get; set; }

    public List<Result?> Results { get; set; } = new();
}