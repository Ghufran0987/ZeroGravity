using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PersonalDataPage : IPersonalDataPage
    {
        public PersonalDataPage()
        {
            InitializeComponent();

            PersonalDataPageViewModel = BindingContext as PersonalDataPageViewModel;
        }

        public static PersonalDataPageViewModel PersonalDataPageViewModel { get; set; }
    }
}