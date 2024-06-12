namespace ZeroGravity.Mobile.Contract.NavigationParameter
{
    public class GeneralSuccessNavParams
    {
        public GeneralSuccessNavParams(string pageTitle = "", string text = "", string subText = "", string iconUnicode = "\uf00c")
        {
            Title = pageTitle;
            Text = text;
            SubText = subText;
            IconUnicode = iconUnicode;
        }
        public string Title { get; }
        public string Text { get; }
        public string SubText { get; }
        public string IconUnicode { get; }

    }
}
