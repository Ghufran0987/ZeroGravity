using System.Xml.Serialization;

namespace ZeroGravity.Models.StreamContent
{
    public class ShowHeroesRssChannelItemEnclosure
    {
        [XmlAttribute("url")]
        public string Url { get; set; }

        [XmlAttribute("length")]
        public string Length { get; set; }

        [XmlAttribute("type")]
        public string Type { get; set; }
    }
}