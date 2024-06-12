using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizardStep4Page : IWizardStep4Page
    {
        public WizardStep4Page()
        {
            InitializeComponent();
        }
    }
}