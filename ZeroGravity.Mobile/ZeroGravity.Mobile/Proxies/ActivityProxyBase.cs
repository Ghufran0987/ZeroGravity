using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class ActivityProxyBase : ProxyBase
    {
        private double _dailyThreshold;

        private double _duration;

        private int _syncId;

        public int SyncId
        {
            get => _syncId;
            set => SetProperty(ref _syncId, value);
        }

        private int _integrationId;

        public int IntegrationId
        {
            get => _integrationId;
            set => SetProperty(ref _integrationId, value);
        }

        public double DailyThreshold
        {
            get => _dailyThreshold;
            set => SetProperty(ref _dailyThreshold, value);
        }

        // Duration in Minutes
        public double Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }
    }
}