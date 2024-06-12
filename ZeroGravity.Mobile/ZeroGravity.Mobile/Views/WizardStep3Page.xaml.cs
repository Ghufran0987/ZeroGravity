using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizardStep3Page : IWizardStep3Page
    {
        public WizardStep3Page()
        {
            InitializeComponent();
        }
    }
}