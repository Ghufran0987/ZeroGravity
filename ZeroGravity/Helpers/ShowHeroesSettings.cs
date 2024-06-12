namespace ZeroGravity.Helpers
{
    public class ShowHeroesSettings
    {
        public string BaseUrl { get; set; }
        public ShowHeroesSettingsItem Stream { get; set; }
        public ShowHeroesSettingsItem Meditation { get; set; }

        public string client_id { get; set; }
        public string grant_type { get; set; }
        public string client_secret { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string scope { get; set; }
    }

    public class ShowHeroesSettingsItem
    {
        public string PlayerId { get; set; }
        public string RssFeed { get; set; }
        public string PlaylistId { get; set; }
        public string ApiUrl { get; set; }
    }

    public class VimeoSettings
    {
        public string BaseUrl { get; set; }
        public VimeoSettingsItem Stream { get; set; }
        public VimeoSettingsItem Meditation { get; set; }

        public string client_id { get; set; }
        public string client_secret { get; set; }
        public string scope { get; set; }
        public string token { get; set; }
    }

    public class VimeoSettingsItem
    {
        public string PlayerId { get; set; }
        public string RssFeed { get; set; }
        public string PlaylistId { get; set; }
        public string ApiUrl { get; set; }
    }
}