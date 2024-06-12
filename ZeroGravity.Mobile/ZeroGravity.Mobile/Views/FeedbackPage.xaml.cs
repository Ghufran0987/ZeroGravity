using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FeedbackPage : IFeedbackPage
    {
        private readonly ScrollView _scrollView;
        private readonly Element _lastCoachingElement;

        public FeedbackPage()
        {
            InitializeComponent();

           // _scrollView = RootScrollView;
            //_lastCoachingElement = LastCoachingItem;
        }

        public void ScrollToCoaching()
        {
            //Device.BeginInvokeOnMainThread(async () =>
            //{
            //    await _scrollView.ScrollToAsync(RootLayout, ScrollToPosition.End, false);
            //});
        }
    }
}