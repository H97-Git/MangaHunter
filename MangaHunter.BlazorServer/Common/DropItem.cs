namespace MangaHunter.BlazorServer.Common;

public class DropItem
{
    public DropItem(Guid mangadexId, string identifier)
    {
        MangadexId = mangadexId;
        Identifier = identifier;
        OriginIdentifier = identifier;
    }

    public Guid MangadexId { get; set; }
    public string Identifier { get; set; }
    public string OriginIdentifier { get; }
}