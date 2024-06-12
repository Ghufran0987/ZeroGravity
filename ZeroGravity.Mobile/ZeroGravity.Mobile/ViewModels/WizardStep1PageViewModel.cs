using System;
using System.Collections.ObjectModel;
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
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.ViewModels
{
    public class WizardStep1PageViewModel : VmBase<IWizardStep1Page, IPersonalDataPageVmProvider, WizardStep1PageViewModel>
    {
        public DelegateCommand WizardStep1SaveDataCommand { get; }
        public ObservableCollection<ComboBoxItem> SalutationsList { get; }
        public ObservableCollection<ComboBoxItem> CountryList { get; }
        public ObservableCollection<ComboBoxItem> EthnicityList { get; }
        public ObservableCollection<ComboBoxItem> UnitTypeList { get; set; }
        public ObservableCollection<ComboBoxItem> DateTimeTypeList { get; set; }

        public WizardStep1PageViewModel(IVmCommonService service, IPersonalDataPageVmProvider provider, ILoggerFactory loggerFactory, bool checkInternet = true) : base(service, provider, loggerFactory, checkInternet)
        {
            WizardStep1SaveDataCommand = new DelegateCommand(SaveWizardStep1DataExecute);

            SalutationsList = new ObservableCollection<ComboBoxItem>
            {
                new ComboBoxItem { Id = 1, Text = AppResources.Salutation_Male },
                new ComboBoxItem { Id = 2, Text = AppResources.Salutation_Female },
                new ComboBoxItem { Id = 3, Text = AppResources.Salutation_Miss },
                new ComboBoxItem { Id = 4, Text = AppResources.Salutation_Ms },
                new ComboBoxItem { Id = 5, Text = AppResources.Salutation_Dr }
             };

            CountryList = new ObservableCollection<ComboBoxItem>
            {
                new ComboBoxItem { Id = 1, Text = AppResources.Country_Germany },
                new ComboBoxItem { Id = 2, Text = AppResources.Country_England },
                new ComboBoxItem { Id = 3, Text = AppResources.Country_France },
                new ComboBoxItem { Id = 4, Text = AppResources.Country_US }
            };

            EthnicityList = new ObservableCollection<ComboBoxItem>
            {
                new ComboBoxItem { Id = 1, Text = "White or European American" },
                new ComboBoxItem { Id = 2, Text = "Black or African American" },
                new ComboBoxItem { Id = 3, Text = "Native American" },
                new ComboBoxItem { Id = 4, Text = "Alaska Native" },
                new ComboBoxItem { Id = 5, Text = "Asian American" },
                new ComboBoxItem { Id = 6, Text = "Native Hawaiian" },
                new ComboBoxItem { Id = 7, Text = "Other Pacific Islander" }
            };

            UnitTypeList = new ObservableCollection<ComboBoxItem>();
            UnitTypeList.Add(new ComboBoxItem { Id = (int)UnitDisplayType.Imperial, Text = AppResources.UnitType_Imperial });
            UnitTypeList.Add(new ComboBoxItem { Id = (int)UnitDisplayType.Metric, Text = AppResources.UnitType_Metric });

            DateTimeTypeList = new ObservableCollection<ComboBoxItem>();
            DateTimeTypeList.Add(new ComboBoxItem { Id = (int)DateTimeDisplayType.Show12HourDay, Text = AppResources.DateTimeType_12HourDay });
            DateTimeTypeList.Add(new ComboBoxItem { Id = (int)DateTimeDisplayType.Show24HourDay, Text = AppResources.DateTimetype_24HourDay });
            LogoImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");

            MinHipDiameter = PersonalDataConstants.MinHipDiametertCm;
            MaxHipDiameter = PersonalDataConstants.MaxHipDiametertCm;

            MinNeckDiameter = PersonalDataConstants.MinNeckDiametertCm;
            MaxNeckDiameter = PersonalDataConstants.MaxNeckDiametertCm;

            MinWaistDiameter = PersonalDataConstants.MinWaistDiametertCm;
            MaxWaistDiameter = PersonalDataConstants.MaxWaistDiametertCm;

            MinHeight = PersonalDataConstants.MinHeightCm;
            MaxHeight = PersonalDataConstants.MaxHeightCm;

            MinWeight = (int)PersonalDataConstants.MinWeightKg;
            MaxWeight = (int)PersonalDataConstants.MaxWeightKg;

            SetDisplayUnits();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //  Title = AppResources.WizardStep_Page1Title;

            if (!HasInternetConnection)
            {
                return;
            }

            // IsBusy = true;

            //var isAuthorized = await ValidateToken();
            //if (isAuthorized)
            //{
            //    await
            //}
            GetPersonalDataAsync();
            // IsBusy = false;
        }

        private async Task GetPersonalDataAsync()
        {
            _cts = new CancellationTokenSource();

            Provider.GetPersonalDataAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    Logger.LogInformation($"PersonalData for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");

                    PersonalDataProxy = apiCallResult.Result.Value;
                    IsMetricUnit = PersonalDataProxy.UnitDisplayType == 1 ? true : false;
                    Is24HourDay = PersonalDataProxy.DateTimeDisplayType == 1 ? true : false;
                    SelectedCountry = this.CountryList.Where(x => x.Text == PersonalDataProxy.Country).FirstOrDefault();
                    SelectedEthnicity = this.EthnicityList.Where(x => x.Text == PersonalDataProxy.Ethnicity).FirstOrDefault();
                }
                else
                {
                    if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                    {
                        return;
                    }

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                    });
                }
            });
        }

        private async void SaveWizardStep1DataExecute()
        {
            _cts = new CancellationTokenSource();

            var saveResult = await SavePersonalDataAsync();
            if (saveResult)
            {
                await Service.NavigationService.NavigateAsync(ViewName.WizardStep4Page);
            }
        }

        private async Task<bool> SavePersonalDataAsync()
        {
            bool saveResult = false;

            if (PersonalDataProxy == null)
                return false;

            if (PersonalDataProxy.AccountId != 0)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized)
                {
                    saveResult = await UpdatePersonalData();
                    if (!saveResult)
                    {
                        return false;
                    }

                    saveResult = await UpdateInitialTargets();
                }
            }
            else
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized)
                {
                    saveResult = await CreatePersonalData();
                    if (!saveResult)
                    {
                        return false;
                    }

                    await CreateInitialTargets();
                }
            }
            return saveResult;
        }

        private async Task<bool> CreatePersonalData()
        {
            if (!HasInternetConnection)
            {
                return false;
            }

            IsBusy = true;

            PersonalDataProxy.UnitDisplayType = IsMetricUnit == true ? 1 : 0;
            PersonalDataProxy.DateTimeDisplayType = Is24HourDay == true ? 1 : 0;
            PersonalDataProxy.Country = SelectedCountry?.Text;
            PersonalDataProxy.Ethnicity = SelectedEthnicity?.Text;

            var apiCallResult = await Provider.CreatePersonalDataAsnyc(PersonalDataProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"PersonalData for Account: {PersonalDataProxy.AccountId} successfully created.");

                PersonalDataProxy = apiCallResult.Value;
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return false;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.ErrorMessage, AppResources.Button_Ok);

                IsBusy = false;
                return false;
            }

            IsBusy = false;
            return true;
        }

        private async Task<bool> UpdatePersonalData()
        {
            if (!HasInternetConnection)
            {
                return false;
            }

            IsBusy = true;

            PersonalDataProxy.UnitDisplayType = IsMetricUnit == true ? 1 : 0;
            PersonalDataProxy.DateTimeDisplayType = Is24HourDay == true ? 1 : 0;
            PersonalDataProxy.Country = SelectedCountry?.Text;
            PersonalDataProxy.Ethnicity = SelectedEthnicity?.Text;

            var apiCallResult = await Provider.UpdatePersonalDataAsnyc(PersonalDataProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"PersonalData for Account: {PersonalDataProxy.AccountId} successfully updated.");

                PersonalDataProxy = apiCallResult.Value;
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return false;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.ErrorMessage, AppResources.Button_Ok);

                IsBusy = false;
                return false;
            }

            IsBusy = false;
            return true;
        }

        private async Task<bool> CreateInitialTargets()
        {
            if (!HasInternetConnection)
            {
                return false;
            }

            IsBusy = true;

            await ValidateToken();

            var age = DateTime.Today.Year - PersonalDataProxy.YearOfBirth;

            var bloodGlucoseLevel = 6.0;
            var fastingType = 0;

            PersonalGoalProxy personalGoalProxy = Provider.CreateInitialTargets(age, (int)PersonalDataProxy.Gender, PersonalDataProxy.Weight, PersonalDataProxy.BodyMassIndex, PersonalDataProxy.BodyFat, bloodGlucoseLevel, fastingType);

            personalGoalProxy.AccountId = PersonalDataProxy.AccountId;

            var apiCallResult = await Provider.CreatePersonalGoalAsnyc(personalGoalProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"PersonalTargets for Account: {personalGoalProxy.AccountId} successfully created.");
            }
            else
            {
                Logger.LogInformation($"PersonalTargets for Account: {personalGoalProxy.AccountId} creation failed.");

                IsBusy = false;
                return false;
            }

            IsBusy = false;
            return true;
        }

        private async Task<bool> UpdateInitialTargets()
        {
            if (!HasInternetConnection)
            {
                return false;
            }

            IsBusy = true;

            var age = DateTime.Today.Year - PersonalDataProxy.YearOfBirth;

            var bloodGlucoseLevel = 6.0;
            var fastingType = 0;

            PersonalGoalProxy personalGoalProxy = Provider.CreateInitialTargets(age, (int)PersonalDataProxy.Gender, PersonalDataProxy.Weight, PersonalDataProxy.BodyMassIndex, PersonalDataProxy.BodyFat, bloodGlucoseLevel, fastingType);

            personalGoalProxy.AccountId = PersonalDataProxy.AccountId;

            var apiCallResult = await Provider.UpdatePersonalGoalAsnyc(personalGoalProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"PersonalTargets for Account: {personalGoalProxy.AccountId} successfully updated.");
            }
            else
            {
                Logger.LogInformation($"PersonalTargets for Account: {personalGoalProxy.AccountId} update failed.");

                IsBusy = false;
                return false;
            }

            IsBusy = false;
            return true;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private PersonalDataProxy _personalDataProxy;

        public PersonalDataProxy PersonalDataProxy
        {
            get => _personalDataProxy;
            set => SetProperty(ref _personalDataProxy, value);
        }

        private CancellationTokenSource _cts;
        private ImageSource _logoImageSource;

        public ImageSource LogoImageSource
        {
            get => _logoImageSource;
            set => SetProperty(ref _logoImageSource, value);
        }

        private double _maxHeight;

        private double _maxHipDiameter;

        private double _maxNeckDiameter;

        private double _maxWaistDiameter;

        private double _maxWeight;

        private double _minHeight;

        private double _minHipDiameter;

        private double _minNeckDiameter;

        private double _minWaistDiameter;

        private double _minWeight;

        private bool _isMetricUnit;
        private string _weightSliderUnit;

        private string _widthSliderUnit;
        private bool _is24HourDay;
        private ComboBoxItem _selectedCountry;
        private ComboBoxItem _selectedEthnicity;

        public string WeightSliderUnit
        {
            get => _weightSliderUnit;
            set => SetProperty(ref _weightSliderUnit, value);
        }

        public bool IsMetricUnit
        {
            get => _isMetricUnit;
            set
            {
                SetProperty(ref _isMetricUnit, value);
                if (_personalDataProxy != null)
                {
                    _personalDataProxy.UnitDisplayType = _isMetricUnit == true ? 1 : 0;
                }
            }
        }

        public bool Is24HourDay
        {
            get => _is24HourDay;
            set
            {
                SetProperty(ref _is24HourDay, value);
                if (_personalDataProxy != null)
                {
                    _personalDataProxy.DateTimeDisplayType = _is24HourDay == true ? 1 : 0;
                }
            }
        }

        public string WidthSliderUnit
        {
            get => _widthSliderUnit;
            set => SetProperty(ref _widthSliderUnit, value);
        }

        public double MinWeight
        {
            get => _minWeight;
            set => SetProperty(ref _minWeight, value);
        }

        public double MaxWeight
        {
            get => _maxWeight;
            set => SetProperty(ref _maxWeight, value);
        }

        public double MinHeight
        {
            get => _minHeight;
            set => SetProperty(ref _minHeight, value);
        }

        public double MaxHeight
        {
            get => _maxHeight;
            set => SetProperty(ref _maxHeight, value);
        }

        public double MinNeckDiameter
        {
            get => _minNeckDiameter;
            set => SetProperty(ref _minNeckDiameter, value);
        }

        public double MaxNeckDiameter
        {
            get => _maxNeckDiameter;
            set => SetProperty(ref _maxNeckDiameter, value);
        }

        public double MinWaistDiameter
        {
            get => _minWaistDiameter;
            set => SetProperty(ref _minWaistDiameter, value);
        }

        public double MaxWaistDiameter
        {
            get => _maxWaistDiameter;
            set => SetProperty(ref _maxWaistDiameter, value);
        }

        public double MinHipDiameter
        {
            get => _minHipDiameter;
            set => SetProperty(ref _minHipDiameter, value);
        }

        public double MaxHipDiameter
        {
            get => _maxHipDiameter;
            set => SetProperty(ref _maxHipDiameter, value);
        }

        public ComboBoxItem SelectedCountry
        {
            get => _selectedCountry;
            set => SetProperty(ref _selectedCountry, value);
        }

        public ComboBoxItem SelectedEthnicity
        {
            get => _selectedEthnicity;
            set => SetProperty(ref _selectedEthnicity, value);
        }

        private void SetDisplayUnits()
        {
            var displayPreferences = DisplayConversionService.GetDisplayPrefences();

            WeightSliderUnit = displayPreferences.UnitDisplayType == UnitDisplayType.Metric
                ? AppResources.Units_Kilogram
                : AppResources.Units_Pound;

            WidthSliderUnit = displayPreferences.UnitDisplayType == UnitDisplayType.Metric
                ? AppResources.Units_Centimeter
                : AppResources.Units_Inch;
        }
    }
}