using ZeroGravity.Infrastructure;

namespace ZeroGravity.Helpers
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public string EmailFrom { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUser { get; set; }
        public string SmtpPass { get; set; }
        public bool InTestMode { get; set; }
        public string VideoSource { get; set; }

        public ShowHeroesSettings ShowHeroesSettings { get; set; }
        public VimeoSettings VimeoSettings { get; set; }
        public FitbitSettings FitbitSettings { get; set; }
        public CoachingSettings CoachingSettings { get; set; }
    }
}