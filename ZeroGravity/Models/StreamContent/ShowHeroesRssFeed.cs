using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ZeroGravity.Models.StreamContent
{
    [Serializable]
    [XmlRoot("rss")]
    public class ShowHeroesRssFeed
    {
        public ShowHeroesRssFeed()
        {
            Channels = new List<ShowHeroesRssChannel>();
        }

        [XmlAttribute("version")]
        public string Version { get; set; }

        [XmlElement("channel")]
        public List<ShowHeroesRssChannel> Channels { get; set; }
    }
}
