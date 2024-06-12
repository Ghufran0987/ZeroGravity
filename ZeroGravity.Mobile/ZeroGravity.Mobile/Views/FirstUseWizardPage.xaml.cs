using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirstUseWizardPage : IFirstUseWizardPage
    {
        public FirstUseWizardPage()
        {
            InitializeComponent();

            var navParams = NavigationParametersHelper.CreateNavigationParameter("IsFirstUse");

            var personalDataViewModel = PersonalDataPage.BindingContext as ViewModelBase;

            var dietPreferencesViewModel = DietPreferencesPage.BindingContext as ViewModelBase;

            var medicalPreConditionsViewModel = MedicalPreConditionsPage.BindingContext as ViewModelBase;

            var personalGoalsViewModel = PersonalGoalsPage.BindingContext as ViewModelBase;

            personalDataViewModel?.OnNavigatedTo(navParams);
            dietPreferencesViewModel?.OnNavigatedTo(navParams);
            medicalPreConditionsViewModel?.OnNavigatedTo(navParams);
            personalGoalsViewModel?.OnNavigatedTo(navParams);
        }
    }
}