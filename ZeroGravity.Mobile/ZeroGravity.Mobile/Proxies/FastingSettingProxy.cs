using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class FastingSettingProxy : ProxyBase
    {
        private bool _includeFridays;

        private bool _includeMondays;

        private bool _includeSaturdays;

        private bool _includeSundays;

        private bool _includeThursdays;

        private bool _includeTuesdays;

        private bool _includeWednesdays;

        private bool _skipBreakfast;

        private bool _skipDinner;

        private bool _skipLunch;
        public int AccountId { get; set; }

        public bool SkipBreakfast
        {
            get => _skipBreakfast;
            set => SetProperty(ref _skipBreakfast, value);
        }

        public bool SkipLunch
        {
            get => _skipLunch;
            set => SetProperty(ref _skipLunch, value);
        }

        public bool SkipDinner
        {
            get => _skipDinner;
            set => SetProperty(ref _skipDinner, value);
        }

        public bool IncludeMondays
        {
            get => _includeMondays;
            set => SetProperty(ref _includeMondays, value);
        }

        public bool IncludeTuesdays
        {
            get => _includeTuesdays;
            set => SetProperty(ref _includeTuesdays, value);
        }

        public bool IncludeWednesdays
        {
            get => _includeWednesdays;
            set => SetProperty(ref _includeWednesdays, value);
        }

        public bool IncludeThursdays
        {
            get => _includeThursdays;
            set => SetProperty(ref _includeThursdays, value);
        }

        public bool IncludeFridays
        {
            get => _includeFridays;
            set => SetProperty(ref _includeFridays, value);
        }

        public bool IncludeSaturdays
        {
            get => _includeSaturdays;
            set => SetProperty(ref _includeSaturdays, value);
        }

        public bool IncludeSundays
        {
            get => _includeSundays;
            set => SetProperty(ref _includeSundays, value);
        }
    }
}