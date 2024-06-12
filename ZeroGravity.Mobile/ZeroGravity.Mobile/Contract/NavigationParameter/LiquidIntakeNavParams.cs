using System;

namespace ZeroGravity.Mobile.Contract.NavigationParameter
{
    public class LiquidIntakeNavParams
    {
        public LiquidIntakeNavParams(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public DateTime DateTime { get; }
    }
}