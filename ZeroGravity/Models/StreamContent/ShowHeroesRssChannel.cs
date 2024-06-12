using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZeroGravity.Models.StreamContent
{
    public class ShowHeroesRssChannel
    {
        public ShowHeroesRssChannel()
        {
            Items = new List<ShowHeroesRssChannelItem>();
        }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("language")]
        public string Language { get; set; }

        [XmlElement("generator")]
        public string Generator { get; set; }

        [XmlElement("ttl")]
        public string Ttl { get; set; }

        [XmlElement("lastBuildDate")]
        public string LastBuildDate { get; set; }

        [XmlElement("pubDate")]
        public string PublishingDate { get; set; }

        [XmlElement("item")]
        public List<ShowHeroesRssChannelItem> Items { get; set; }
    }
}