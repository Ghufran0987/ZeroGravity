using System.Xml.Serialization;

namespace ZeroGravity.Models.StreamContent
{
    public class ShowHeroesRssChannelItem
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("guid")]
        public string Guid { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("pubDate")]
        public string PublishingDate { get; set; }

        [XmlElement("category")]
        public string Category { get; set; }

        [XmlElement("enclosure")]
        public ShowHeroesRssChannelItemEnclosure Enclosure { get; set; }

    }
}