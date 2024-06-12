using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Proxies
{
    public class SugarBeatGlucoseProxy : GlucoseBaseProxy
    {
        private uint _activeAlarmBitmaps;
        private ushort _battery;

        public SugarBeatGlucoseProxy()
        {
        }

        public SugarBeatGlucoseProxy(SugarBeatGlucose model)
        {
            DateTime = model.DateTime;
            Battery = model.Battery;
            ActiveAlarmBitmaps = model.ActiveAlarmBitmaps;
            Glucose = model.Glucose;
        }

        public ushort Battery
        {
            get => _battery;
            set => SetProperty(ref _battery, value);
        }

        public uint ActiveAlarmBitmaps
        {
            get => _activeAlarmBitmaps;
            set => SetProperty(ref _activeAlarmBitmaps, value);
        }
    }
}