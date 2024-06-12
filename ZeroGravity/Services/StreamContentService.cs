using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Options;
using ZeroGravity.Helpers;
using ZeroGravity.Interfaces;
using ZeroGravity.Models.StreamContent;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Services
{
    public class StreamContentService : IStreamContentService
    {
        private readonly ShowHeroesSettings _settings;
        
        private readonly HttpClient _client;

        public StreamContentService(IOptions<AppSettings> appSettings)
        {
            _client = new HttpClient();
            _settings = appSettings.Value.ShowHeroesSettings;
        }

        public async Task<IEnumerable<StreamContentDto>> GetAvailableContentAsync(StreamContentType type)
        {
            BuildUrl(type, out var url, out var videoPlayerId);
            var raw = await _client.GetStringAsync(url);

            var serializer = new XmlSerializer(typeof(ShowHeroesRssFeed));

            ShowHeroesRssFeed feed = null;
            using (var reader = new StringReader(raw))
            {
                var obj = serializer.Deserialize(reader);
                feed = obj as ShowHeroesRssFeed;
            }

            var result = Convert(feed, videoPlayerId);

            return result;
        }

        private IEnumerable<StreamContentDto> Convert(ShowHeroesRssFeed feed, string videoPlayerId)
        {
            var result = new List<StreamContentDto>();

            foreach (var channel in feed.Channels)
            {
                foreach (var item in channel.Items)
                {
                    var streamContent = Convert(item, videoPlayerId);
                    result.Add(streamContent);
                }
            }

            return result;
        }

        private StreamContentDto Convert(ShowHeroesRssChannelItem item, string videoPlayerId)
        {
            var id = Guid.Parse(item.Guid);
            var videoId = item.Link.Split('/').Last();
            var categories = new List<string>();

            foreach (var chunk in item.Category.Split(','))
            {
                categories.Add(chunk.Trim());
            }

            return new StreamContentDto
            {
                Id = id,
                Title =  item.Title,
                Description = item.Description,
                VideoUrl = item.Link,
                VideoId = videoId,
                ThumbnailUrl = item.Enclosure.Url,
                Categories = categories,
                VideoPlayerId = videoPlayerId
            };
        }

        private void BuildUrl(StreamContentType type, out string url, out string videoPlayerId)
        {
            string urlPath;
            switch (type)
            {
                case StreamContentType.Stream:
                    urlPath = _settings.Stream.RssFeed;
                    videoPlayerId = _settings.Stream.PlayerId;
                    break;
                case StreamContentType.Meditation:
                    urlPath = _settings.Meditation.RssFeed;
                    videoPlayerId = _settings.Meditation.PlayerId;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            url = $"{_settings.BaseUrl}{urlPath}";
        }
    }
}
