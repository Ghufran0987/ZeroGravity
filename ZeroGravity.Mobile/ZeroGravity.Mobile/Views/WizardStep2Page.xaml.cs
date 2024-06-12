using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizardStep2Page : IWizardStep2Page
    {
        public WizardStep2Page()
        {
            InitializeComponent();
        }
    }
}