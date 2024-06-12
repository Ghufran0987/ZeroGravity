using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.SfRotator.XForms;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Providers;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.ViewModels
{
    public class FirstUseWizardPageViewModel : VmBase<IFirstUseWizardPage, IFirstUseWizardPageVmProvider,
        FirstUseWizardPageViewModel>
    {
        private readonly ILogger _logger;
        private readonly IUserDataService _userDataService;

        private CancellationTokenSource _cts;


        public FirstUseWizardPageViewModel(IVmCommonService service, IFirstUseWizardPageVmProvider provider,
            ILoggerFactory loggerFactory, IUserDataService userDataService) : base(service, provider, loggerFactory)
        {
            _userDataService = userDataService;

            _logger = loggerFactory?.CreateLogger<PersonalDataPageVmProvider>() ??
                      new NullLogger<PersonalDataPageVmProvider>();

            NextWizardStepCommand = new Command(GoToNextWizardStep);

            ButtonText = AppResources.Button_Continue;
        }

        private async void GoToNextWizardStep(object obj)
        {
            if (obj is SfRotator wizard)
            {
                if (wizard.SelectedIndex < wizard.DataSource.Count - 1)
                {
                    var currentItem = wizard.DataSource.ElementAt(wizard.SelectedIndex);

                    var view = currentItem.ItemContent;

                    var currentViewModelBase = view.BindingContext as ViewModelBase;

                    var navParams = NavigationParametersHelper.CreateNavigationParameter("IsFirstUse");

                    currentViewModelBase?.OnNavigatedFrom(navParams);

                    wizard.SelectedIndex++;

                    // IsFirstWizwardPage = false;
                    if (wizard.SelectedIndex == wizard.DataSource.Count - 1)
                    {
                        ButtonText = AppResources.Button_Finish;
                    }
                }
                else
                {
                    var currentItem = wizard.DataSource.ElementAt(wizard.SelectedIndex);

                    var view = currentItem.ItemContent;

                    var currentViewModelBase = view.BindingContext as ViewModelBase;

                    var navParams = NavigationParametersHelper.CreateNavigationParameter("IsFirstUse");

                    currentViewModelBase?.OnNavigatedFrom(navParams);

                    //After saving the last data of the first use wizard update the user account for the next login
                    await UpdateAccountData();

                    await Service.NavigationService.NavigateAsync("/" + ViewName.ContentShellPage);
                }
            }
        }

        private string _buttonText;

        public string ButtonText
        {
            get => _buttonText;
            set => SetProperty(ref _buttonText, value);
        }



        private bool _isFirstWizwardPage;

        public bool IsFirstWizwardPage
        {
            get => _isFirstWizwardPage;
            set => SetProperty(ref _isFirstWizwardPage, value);
        }

        

        public ICommand NextWizardStepCommand { get; set; }
        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            IsFirstWizwardPage = true;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }


        private async Task UpdateAccountData()
        {
            var updateWizardRequest = new UpdateWizardRequestDto
            {
                CompletedFirstUseWizard = true
            };

            var apiCallResult = await Provider.UpdateAccountDataAsnyc(updateWizardRequest, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"WizardState successfully updated.");
            }
            else
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.FirstUseWizard_Greeting_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);
            }
        }
    }
}
