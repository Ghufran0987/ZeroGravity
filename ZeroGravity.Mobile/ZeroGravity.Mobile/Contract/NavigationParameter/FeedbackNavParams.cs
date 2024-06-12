namespace ZeroGravity.Mobile.Contract.NavigationParameter
{
    public class FeedbackNavParams : PageNavigationParams
    {
        public bool ScrollDown { get; set; }
    }

    public class BooleanNavParams : PageNavigationParams
    {
        public bool IsTrue { get; set; }
    }
}
