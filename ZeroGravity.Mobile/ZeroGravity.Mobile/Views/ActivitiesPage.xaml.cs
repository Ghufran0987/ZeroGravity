using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivitiesPage : IActivitiesPage
    {
        public ActivitiesPage()
        {
            InitializeComponent();
        }
    }
}