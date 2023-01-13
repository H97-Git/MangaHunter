namespace MangaHunter.Contracts.Common;

public class RssMangaUpdatesDto
{
    public Channel Channel { get; set; }

    public double Version { get; set; }

    public string Content { get; set; }

    public string Text { get; set; }
}

public class Channel
{
    public string Title { get; set; }

    public string Link { get; set; }

    public string Description { get; set; }

    public List<Item> Item { get; set; }
}

public class Item
{
    public string Title { get; set; }

    public string Description { get; set; }
}