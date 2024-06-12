using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZeroGravity.Mobile.Proxies
{
    public class StreamContentProxy : BindableBase
    {

        private string _title;
        private string _description;
        private string _videoUrl;
        private string _thumbnailUrl;
        private IEnumerable<string> _categories;

        public string VideoPlayerId { get; set; }
        public DateTime Published { get; set; }

        public Guid Id { get; set; }

        public string VideoId { get; set; }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public string VideoUrl
        {
            get => _videoUrl;
            set => SetProperty(ref _videoUrl, value);
        }

        public string ThumbnailUrl
        {
            get => _thumbnailUrl;
            set => SetProperty(ref _thumbnailUrl, value);
        }

        public IEnumerable<string> Categories
        {
            get => _categories;
            set => SetProperty(ref _categories, value);
        }
    }
}
