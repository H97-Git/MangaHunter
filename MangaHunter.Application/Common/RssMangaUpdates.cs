using System.Xml.Serialization;

namespace MangaHunter.Application.Common;

[XmlRoot(ElementName="item")]
public class Item { 

    [XmlElement(ElementName="title")] 
    public string Title { get; set; } 

    [XmlElement(ElementName="description")] 
    public string Description { get; set; } 
}

[XmlRoot(ElementName="channel")]
public class Channel { 

    [XmlElement(ElementName="title")] 
    public string Title { get; set; } 

    [XmlElement(ElementName="link")] 
    public string Link { get; set; } 

    [XmlElement(ElementName="description")] 
    public string Description { get; set; } 

    [XmlElement(ElementName="item")] 
    public List<Item> Item { get; set; } 
}

[XmlRoot(ElementName="rss")]
public class RssMangaUpdates { 

    [XmlElement(ElementName="channel")] 
    public Channel Channel { get; set; } 

    [XmlAttribute(AttributeName="version")] 
    public double Version { get; set; } 

    [XmlAttribute(AttributeName="content")] 
    public string Content { get; set; } 

    [XmlText] 
    public string Text { get; set; } 
}