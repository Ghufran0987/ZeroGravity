using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizardStep5Page : IWizardStep5Page
    {
        public WizardStep5Page()
        {
            InitializeComponent();
        }
    }
}