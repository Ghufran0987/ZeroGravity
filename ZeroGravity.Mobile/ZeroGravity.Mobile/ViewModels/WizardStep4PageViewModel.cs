using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class WizardStep4PageViewModel : VmBase<IWizardStep4Page, IDietPreferencesPageVmProvider, WizardStep4PageViewModel>
    {
        public DelegateCommand SelectStandardCommand { get; }
        public DelegateCommand SelectPescetarianCommand { get; }
        public DelegateCommand SelectVegetarianCommand { get; }
        public DelegateCommand SelectLactoVegetarianCommand { get; }
        public DelegateCommand SelectOvoVegetarianCommand { get; }
        public DelegateCommand SelectVeganCommand { get; }

        public DelegateCommand WizardStep4SaveDataCommand { get; }

        public WizardStep4PageViewModel(IVmCommonService service, IDietPreferencesPageVmProvider provider, ILoggerFactory loggerFactory, bool checkInternet = true) : base(service, provider, loggerFactory, checkInternet)
        {
            _radioButtonItems = new List<RadioButtonItemProxy>();
            InitRadioButtonItems();

            SelectStandardCommand = new DelegateCommand(SelectStandard);
            SelectPescetarianCommand = new DelegateCommand(SelectPescetarian);
            SelectVegetarianCommand = new DelegateCommand(SelectVegetarian);
            SelectLactoVegetarianCommand = new DelegateCommand(SelectLactoVegetarian);
            SelectOvoVegetarianCommand = new DelegateCommand(SelectOvoVegetarian);
            SelectVeganCommand = new DelegateCommand(SelectVegan);

            WizardStep4SaveDataCommand = new DelegateCommand(SaveWizardStep4DataExecute);
            LogoImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            // Title = AppResources.WizardStep_Page4Title;

            if (!HasInternetConnection)
            {
                return;
            }

            //IsBusy = true;

            _cts = new CancellationTokenSource();

            //if (await ValidateToken())
            //{
            //    await
            //}
            LoadDietPreferences();
            // IsBusy = false;
        }

        private void InitRadioButtonItems()
        {
            StandardDietItem = new RadioButtonItemProxy
            {
                IsChecked = true,
                Key = (int)DietType.Standard
            };

            PescetarianDietItem = new RadioButtonItemProxy
            {
                IsChecked = false,
                Key = (int)DietType.Pescetarian
            };

            VegetarianDietItem = new RadioButtonItemProxy
            {
                IsChecked = false,
                Key = (int)DietType.Vegetarian
            };

            LactoVegetarianDietItem = new RadioButtonItemProxy
            {
                IsChecked = false,
                Key = (int)DietType.LactoVegetarian
            };

            OvoVegetarianDietItem = new RadioButtonItemProxy
            {
                IsChecked = false,
                Key = (int)DietType.OvoVegetarian
            };

            VeganDietItem = new RadioButtonItemProxy
            {
                IsChecked = false,
                Key = (int)DietType.Vegan
            };

            _radioButtonItems.Add(StandardDietItem);
            _radioButtonItems.Add(PescetarianDietItem);
            _radioButtonItems.Add(VegetarianDietItem);
            _radioButtonItems.Add(LactoVegetarianDietItem);
            _radioButtonItems.Add(OvoVegetarianDietItem);
            _radioButtonItems.Add(VeganDietItem);
        }

        private async Task LoadDietPreferences()
        {
            Provider.GetDietPreferencesAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    Logger.LogInformation($"DietPreferences for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");

                    DietPreferencesProxy = apiCallResult.Result.Value;

                    var itemToSelect = _radioButtonItems.FirstOrDefault(_ => _.Key == (int)DietPreferencesProxy.DietType);

                    if (itemToSelect != null) itemToSelect.IsChecked = true;
                }
                else
                {
                    if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                    {
                        return;
                    }

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Service.DialogService.DisplayAlertAsync(AppResources.DietPreferences_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                    });
                }
            });
        }

        private async void SaveWizardStep4DataExecute()
        {
            _cts = new CancellationTokenSource();

            await SaveDietPreferences();
            await Service.NavigationService.NavigateAsync(ViewName.WizardStep5Page);
        }

        private async Task SaveDietPreferences()
        {
            if (DietPreferencesProxy == null)
                return;

            if (DietPreferencesProxy.Id != 0)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await UpdateDietPreferences();
            }
            else
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await CreateDietPreferences();
            }
        }

        private async Task CreateDietPreferences()
        {
            if (!HasInternetConnection)
            {
                return;
            }

            IsBusy = true;

            await ValidateToken();

            var selectedDietItem = _radioButtonItems.FirstOrDefault(_ => _.IsChecked);

            if (selectedDietItem != null) DietPreferencesProxy.DietType = (DietType)selectedDietItem.Key;

            var apiCallResult = await Provider.CreateDietPreferencesAsnyc(DietPreferencesProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"DietPreferences for Account: {DietPreferencesProxy.AccountId} successfully created.");

                DietPreferencesProxy = apiCallResult.Value;
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.DietPreferences_Title, apiCallResult.ErrorMessage, AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async Task UpdateDietPreferences()
        {
            if (!HasInternetConnection)
            {
                return;
            }

            IsBusy = true;

            await ValidateToken();

            var selectedDietItem = _radioButtonItems.FirstOrDefault(_ => _.IsChecked);

            if (selectedDietItem != null) DietPreferencesProxy.DietType = (DietType)selectedDietItem.Key;

            var apiCallResult = await Provider.UpdateDietPreferencesAsnyc(DietPreferencesProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"DietPreferences for Account: {DietPreferencesProxy.AccountId} successfully updated.");

                DietPreferencesProxy = apiCallResult.Value;
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.DietPreferences_Title, apiCallResult.ErrorMessage, AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private void SelectStandard()
        {
            _standardDietItem.IsChecked = true;
        }

        private void SelectPescetarian()
        {
            _pescetarianDietItem.IsChecked = true;
        }

        private void SelectVegetarian()
        {
            _vegetarianDietItem.IsChecked = true;
        }

        private void SelectLactoVegetarian()
        {
            _lactoVegetarianDietItem.IsChecked = true;
        }

        private void SelectOvoVegetarian()
        {
            _ovoVegetarianDietItem.IsChecked = true;
        }

        private void SelectVegan()
        {
            _veganDietItem.IsChecked = true;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private DietPreferencesProxy _dietPreferencesProxy;

        public DietPreferencesProxy DietPreferencesProxy
        {
            get => _dietPreferencesProxy;
            set => SetProperty(ref _dietPreferencesProxy, value);
        }

        private RadioButtonItemProxy _standardDietItem;

        public RadioButtonItemProxy StandardDietItem
        {
            get => _standardDietItem;
            set => SetProperty(ref _standardDietItem, value);
        }

        private RadioButtonItemProxy _pescetarianDietItem;

        public RadioButtonItemProxy PescetarianDietItem
        {
            get => _pescetarianDietItem;
            set => SetProperty(ref _pescetarianDietItem, value);
        }

        private RadioButtonItemProxy _vegetarianDietItem;

        public RadioButtonItemProxy VegetarianDietItem
        {
            get => _vegetarianDietItem;
            set => SetProperty(ref _vegetarianDietItem, value);
        }

        private RadioButtonItemProxy _lactoVegetarianDietItem;

        public RadioButtonItemProxy LactoVegetarianDietItem
        {
            get => _lactoVegetarianDietItem;
            set => SetProperty(ref _lactoVegetarianDietItem, value);
        }

        private RadioButtonItemProxy _ovoVegetarianDietItem;

        public RadioButtonItemProxy OvoVegetarianDietItem
        {
            get => _ovoVegetarianDietItem;
            set => SetProperty(ref _ovoVegetarianDietItem, value);
        }

        private RadioButtonItemProxy _veganDietItem;

        public RadioButtonItemProxy VeganDietItem
        {
            get => _veganDietItem;
            set => SetProperty(ref _veganDietItem, value);
        }

        private ImageSource _logoImageSource;

        public ImageSource LogoImageSource
        {
            get => _logoImageSource;
            set => SetProperty(ref _logoImageSource, value);
        }

        private readonly List<RadioButtonItemProxy> _radioButtonItems;

        private CancellationTokenSource _cts;
    }
}