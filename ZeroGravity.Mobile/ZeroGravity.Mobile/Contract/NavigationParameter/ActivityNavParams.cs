using System;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Contract.NavigationParameter
{
    public class ActivityNavParams
    {
        public int ActivityId { get; set; }
        public DateTime TargetDateTime { get; set; }
    }



    public class SugarBeatDeviceNavParams
    {
        public SugarBeatDevice Device { get; set; }
    }
    public class SugarBeatDeviceDisconnectedNavParams
    {
        public bool IsDeviceDisconnected { get; set; }
    }


    public class SugarBeatMainPageNavParams
    {
        public SugarBeatEatingSessionProxy EatingSession { get; set; }
        public bool IsCurrentSession { get; set; }

    }

}