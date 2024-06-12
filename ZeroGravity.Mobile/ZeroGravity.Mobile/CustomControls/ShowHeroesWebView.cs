using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.CustomControls
{
    public class ShowHeroesWebView : WebView
    {
        private readonly string _html;
        private const string VideoIdMarkup = "#{VIDEOID}";
        private const string PlayerIdMarkup = "#{PLAYERID}";

        private string _videoId = null;
        private string _playerId = null;

        public ShowHeroesWebView()
        {
            var assembly = Assembly.GetCallingAssembly();
            var stream = assembly.GetManifestResourceStream("ZeroGravity.Mobile.Resources.ShowHeroes.html");
            using (var reader = new StreamReader(stream))
            {
                _html = reader.ReadToEnd();
            }
        }

        public static readonly BindableProperty VideoIdProperty = BindableProperty.Create(nameof(VideoId), typeof(string), typeof(ShowHeroesWebView));


        public string VideoId
        {
            get => (string)GetValue(VideoIdProperty); 
            set => SetValue(VideoIdProperty, value);
        }

        public static readonly BindableProperty VideoPlayerIdProperty = BindableProperty.Create(nameof(VideoPlayerId), typeof(string), typeof(ShowHeroesWebView));

        public string VideoPlayerId
        {
            get => (string)GetValue(VideoPlayerIdProperty); 
            set => SetValue(VideoPlayerIdProperty, value);
        }

        protected override void OnPropertyChanged(string propertyName)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(Height))
            {
                //Debug.WriteLine($"W: {HeightRequest} | WR: {HeightRequest}");
                return;
            }

            if (propertyName == nameof(VideoId) || 
                propertyName == nameof(VideoPlayerId))
            {

                if (string.IsNullOrEmpty(VideoId)) return;
                if (string.IsNullOrEmpty(VideoPlayerId)) return;

                var html = ReplaceMarkups(_html);

                var source = new HtmlWebViewSource
                {
                    Html = html
                };
                Source = source;
            }
        }

        //protected override void OnSizeAllocated(double width, double height)
        //{
        //    var newHeight = width / 9 * 16;

        //    HeightRequest = newHeight;

        //    base.OnSizeAllocated(width, newHeight);
        //}

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            var newHeight = widthConstraint / 9 * 16;

            HeightRequest = newHeight;

            return base.OnMeasure(widthConstraint, newHeight);
        }


        private string ReplaceMarkups(string html)
        {
            var sb = new StringBuilder(html);

            sb.Replace(VideoIdMarkup, VideoId);
            sb.Replace(PlayerIdMarkup, VideoPlayerId);

            return sb.ToString();
        }

        private string SetVideoId(string html)
        {
            return html.Replace(VideoIdMarkup, VideoId);
        }

        private string SetPlayerId(string html)
        {
            return html.Replace(PlayerIdMarkup, VideoPlayerId);
        }
    }
}
