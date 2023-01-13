namespace MangaHunter.API.Common.Http;

public class Gateway
{
    public string Host { get; set; }
    public Hunter Hunter { get; set; }
    public Tierlist Tierlist { get; set; }
}

public class Hunter
{
    public string all { get; set; }
    public string id { get; set; }
    public string add { get; set; }
    public string edit { get; set; }
    public string delete { get; set; }
    public string mangaupdatesid { get; set; }
    public string search { get; set; }
}

public class Tierlist
{
    public string all { get; set; }
    public string id { get; set; }
    public string add { get; set; }
    public string edit { get; set; }
    public string delete { get; set; }
    public string sharecode { get; set; }
}