using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MedicalPreConditionsPage : IMedicalPreConditionsPage
    {
        public MedicalPreConditionsPage()
        {
            InitializeComponent();
        }
    }
}