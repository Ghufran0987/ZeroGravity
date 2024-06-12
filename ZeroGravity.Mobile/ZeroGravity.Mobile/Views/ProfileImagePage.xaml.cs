using Syncfusion.XForms.BadgeView;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProfileImagePage : IProfileImagePage
    {
        public ProfileImagePage()
        {
            InitializeComponent();
        }
    }
}