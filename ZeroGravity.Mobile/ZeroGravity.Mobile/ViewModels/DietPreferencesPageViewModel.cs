using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class DietPreferencesPageViewModel : VmBase<IDietPreferencesPage, IDietPreferencesPageVmProvider, DietPreferencesPageViewModel>
    {
        public DelegateCommand ShowBreakfastTimePickerCommand { get; set; }
        public DelegateCommand ShowLunchTimePickerCommand { get; set; }
        public DelegateCommand ShowDinnerTimePickerCommand { get; set; }


        public DelegateCommand SelectStandardCommand { get; }
        public DelegateCommand SelectPescetarianCommand { get; }
        public DelegateCommand SelectVegetarianCommand { get; }
        public DelegateCommand SelectLactoVegetarianCommand { get; }
        public DelegateCommand SelectOvoVegetarianCommand { get; }
        public DelegateCommand SelectVeganCommand { get; }

        public DelegateCommand SaveDietPreferencesCommand { get; }

        public DietPreferencesPageViewModel(IVmCommonService service, IDietPreferencesPageVmProvider provider, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            _radioButtonItems = new List<RadioButtonItemProxy>();

            InitRadioButtonItems();

            SaveDietPreferencesCommand = new DelegateCommand(SaveDietPreferencesExecute);

            ShowBreakfastTimePickerCommand = new DelegateCommand(ShowBreakfastTimePicker);
            ShowLunchTimePickerCommand = new DelegateCommand(ShowLunchTimePicker);
            ShowDinnerTimePickerCommand = new DelegateCommand(ShowDinnerTimePicker);

            SelectStandardCommand = new DelegateCommand(SelectStandard);
            SelectPescetarianCommand = new DelegateCommand(SelectPescetarian);
            SelectVegetarianCommand = new DelegateCommand(SelectVegetarian);
            SelectOvoVegetarianCommand = new DelegateCommand(SelectOvoVegetarian);
            SelectLactoVegetarianCommand = new DelegateCommand(SelectLactoVegetarian);
            SelectVeganCommand = new DelegateCommand(SelectVegan);
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            try
            {
                //var isAuthorized = await ValidateToken();
                //if (isAuthorized) await 
                        LoadDietPreferences();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        private async Task LoadDietPreferences()
        {
            if (!HasInternetConnection) return;

            //  IsBusy = true;

            // await ValidateToken();
            DietPreferencesProxy = new DietPreferencesProxy();
            Provider.GetDietPreferencesAsnyc(_cts.Token).ContinueWith(async apiCallResult=> { 


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
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.DietPreferences_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
            });
        }

        private async void SaveDietPreferencesExecute()
        {
            _cts = new CancellationTokenSource();

            await SaveDietPreferences();
        }

        private async Task SaveDietPreferences()
        {
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

        private async Task UpdateDietPreferences()
        {
            if (!HasInternetConnection) return;

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

        private async Task CreateDietPreferences()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;

            await ValidateToken();

            var selectedDietItem = _radioButtonItems.FirstOrDefault(_ => _.IsChecked);

            if (selectedDietItem != null) DietPreferencesProxy.DietType = (DietType)selectedDietItem.Key;

            var apiCallResult = await Provider.CreateDietPreferencesAsnyc(DietPreferencesProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"DietPreferences for Account: {DietPreferencesProxy.AccountId} successfully created.");

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


        private bool _openBreakFastTimePicker;
        public bool OpenBreakFastTimePicker
        {
            get => _openBreakFastTimePicker;
            set => SetProperty(ref _openBreakFastTimePicker, value);
        }

        private bool _openLunchTimePicker;
        public bool OpenLunchTimePicker
        {
            get => _openLunchTimePicker;
            set => SetProperty(ref _openLunchTimePicker, value);
        }

        private bool _openDinnerTimePicker;
        public bool OpenDinnerTimePicker
        {
            get => _openDinnerTimePicker;
            set => SetProperty(ref _openDinnerTimePicker, value);
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


        private void ShowBreakfastTimePicker()
        {
            OpenBreakFastTimePicker = !OpenBreakFastTimePicker;
        }

        private void ShowLunchTimePicker()
        {
            OpenLunchTimePicker = !OpenLunchTimePicker;
        }

        private void ShowDinnerTimePicker()
        {
            OpenDinnerTimePicker = !OpenDinnerTimePicker;
        }

        private readonly List<RadioButtonItemProxy> _radioButtonItems;

        private CancellationTokenSource _cts;
    }
}