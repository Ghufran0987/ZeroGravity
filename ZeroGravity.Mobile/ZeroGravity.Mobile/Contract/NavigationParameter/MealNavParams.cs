using System;

namespace ZeroGravity.Mobile.Contract.NavigationParameter
{
    public class MealNavParams
    {
        public MealNavParams(DateTime dateTime)
        {
            DateTime = dateTime;
        }

        public DateTime DateTime { get; }
    }
}