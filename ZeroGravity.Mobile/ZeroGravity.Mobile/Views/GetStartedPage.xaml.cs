using Xamarin.Forms;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.Views
{
    public partial class GetStartedPage : IGetStartedPage
    {
        public GetStartedPage()
        {
            InitializeComponent();

            // this page has no title,
            // therefore this will be the BackButtonTitle in the NavigationBar on the Pages that will navigate to this page
            //NavigationPage.SetBackButtonTitle(this, AppResources.Common_Back);
        }
    }
}
