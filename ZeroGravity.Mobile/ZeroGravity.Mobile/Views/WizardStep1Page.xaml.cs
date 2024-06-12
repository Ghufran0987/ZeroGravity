using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WizardStep1Page : IWizardStep1Page
    {
        public WizardStep1Page()
        {
            InitializeComponent();
        }
    }
}