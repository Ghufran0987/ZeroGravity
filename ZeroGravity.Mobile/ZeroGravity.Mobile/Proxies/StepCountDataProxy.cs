using System;
using ZeroGravity.Mobile.Base.Proxy;

namespace ZeroGravity.Mobile.Proxies
{
    public class StepCountDataProxy : ProxyBase
    {
        private int _accountId;

        private int _stepCount;

        private DateTime _targetDate;

        public int AccountId
        {
            get => _accountId;
            set => SetProperty(ref _accountId, value);
        }

        public int StepCount
        {
            get => _stepCount;
            set => SetProperty(ref _stepCount, value);
        }

        public DateTime TargetDate
        {
            get => _targetDate;
            set => SetProperty(ref _targetDate, value);
        }
    }
}