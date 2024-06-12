using System;
using System.Collections.Generic;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Contract.NavigationParameter
{
    public class SyncActivityNavParams
    {
        public DateTime TargetDateTime { get; set; }

        public List<SyncActivityProxy> ExerciseActivityProxies { get; set; }
    }
}