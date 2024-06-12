using System;

namespace ZeroGravity.Mobile.Contract.NavigationParameter
{
    public class PageNavigationParams
    {
        public string PageName { get; set; }
        public DateTime DateTime { get; set; }
    }

    public class PageNavigationParams<T> : PageNavigationParams
    {
        public T Payload { get; set; }
    }
}