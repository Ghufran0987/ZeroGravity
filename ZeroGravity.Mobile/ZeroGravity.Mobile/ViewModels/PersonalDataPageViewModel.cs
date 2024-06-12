using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Events;
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
    public class PersonalDataPageViewModel : VmBase<IPersonalDataPage, IPersonalDataPageVmProvider, PersonalDataPageViewModel>
    {
        private CancellationTokenSource _cts;
        public DelegateCommand SavePersonalDataCommand { get; }
        public ObservableCollection<ComboBoxItem> SalutationsList { get; set; }
        public ObservableCollection<ComboBoxItem> CountryList { get; set; }
        public ObservableCollection<ComboBoxItem> EthnicityList { get; set; }
        public ObservableCollection<ComboBoxItem> UnitTypeList { get; set; }
        public ObservableCollection<ComboBoxItem> DateTimeTypeList { get; set; }
        public DelegateCommand PrivacyCommand { get; }

        public PersonalDataPageViewModel(IVmCommonService service, IPersonalDataPageVmProvider provider, ILoggerFactory loggerFactory, IEventAggregator eventAggregator) : base(service, provider, loggerFactory)
        {
            SalutationsList = new ObservableCollection<ComboBoxItem>();
            SalutationsList.Add(new ComboBoxItem { Id = 1, Text = AppResources.Salutation_Male });
            SalutationsList.Add(new ComboBoxItem { Id = 2, Text = AppResources.Salutation_Female });
            SalutationsList.Add(new ComboBoxItem { Id = 3, Text = AppResources.Salutation_Miss });
            SalutationsList.Add(new ComboBoxItem { Id = 4, Text = AppResources.Salutation_Ms });
            SalutationsList.Add(new ComboBoxItem { Id = 5, Text = AppResources.Salutation_Dr });

            CountryList = new ObservableCollection<ComboBoxItem>();
            CountryList.Add(new ComboBoxItem { Id = 1, Text = AppResources.Country_Germany });
            CountryList.Add(new ComboBoxItem { Id = 2, Text = AppResources.Country_England });
            CountryList.Add(new ComboBoxItem { Id = 3, Text = AppResources.Country_France });
            CountryList.Add(new ComboBoxItem { Id = 4, Text = AppResources.Country_US });

            EthnicityList = new ObservableCollection<ComboBoxItem>();
            EthnicityList.Add(new ComboBoxItem { Id = 1, Text = "White or European American" });
            EthnicityList.Add(new ComboBoxItem { Id = 2, Text = "Black or African American" });
            EthnicityList.Add(new ComboBoxItem { Id = 3, Text = "Native American" });
            EthnicityList.Add(new ComboBoxItem { Id = 4, Text = "Alaska Native" });
            EthnicityList.Add(new ComboBoxItem { Id = 5, Text = "Asian American" });
            EthnicityList.Add(new ComboBoxItem { Id = 6, Text = "Native Hawaiian" });
            EthnicityList.Add(new ComboBoxItem { Id = 7, Text = " Other Pacific Islander" });

            UnitTypeList = new ObservableCollection<ComboBoxItem>();
            UnitTypeList.Add(new ComboBoxItem { Id = (int)UnitDisplayType.Imperial, Text = AppResources.UnitType_Imperial });
            UnitTypeList.Add(new ComboBoxItem { Id = (int)UnitDisplayType.Metric, Text = AppResources.UnitType_Metric });

            DateTimeTypeList = new ObservableCollection<ComboBoxItem>();
            DateTimeTypeList.Add(new ComboBoxItem { Id = (int)DateTimeDisplayType.Show12HourDay, Text = AppResources.DateTimeType_12HourDay });
            DateTimeTypeList.Add(new ComboBoxItem { Id = (int)DateTimeDisplayType.Show24HourDay, Text = AppResources.DateTimetype_24HourDay });

            SavePersonalDataCommand = new DelegateCommand(SavePersonalData);

            WeightSliderUnit = AppResources.Units_Kilogram;
            WidthSliderUnit = AppResources.Units_Centimeter;

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

            PrivacyCommand = new DelegateCommand(PrivacyExecute);
        }

        private void PrivacyExecute()
        {
            var termsAndPrivacyNavParams = new TermsAndPrivacyNavParams { TabTitle = AppResources.TermsAndPrivacy_TermsTabTitle, PageName = ViewName.TermsAndPrivacyPage };
            Service.EventAggregator.GetEvent<TermsAndPrivacyPageEvent>().Publish(termsAndPrivacyNavParams);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            _cts = new CancellationTokenSource();
            GetPersonalDataAsync();
        }

        private async Task GetPersonalDataAsync()
        {
            ShowProgress = true;
            Provider.GetPersonalDataAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
           {
              
                   if (apiCallResult.Result.Success)
                   {
                       Logger.LogInformation($"PersonalData for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");

                       PersonalDataProxy = apiCallResult.Result.Value;
                       ShowProgress = false;
                       SelectedSalutation = this.SalutationsList?.Where(x => x.Text == getSalutation(PersonalDataProxy.Salutation)).FirstOrDefault();
                       SelectedCountry = this.CountryList?.Where(x => x.Text == PersonalDataProxy.Country).FirstOrDefault();
                       SelectedEthnicity = PersonalDataProxy.Ethnicity;
                       SetDisplayUnits();
                   }
                   else
                   {
                       if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                       {
                           ShowProgress = false;
                           return;
                       }
                       ShowProgress = false;
                       //Device.BeginInvokeOnMainThread(async () =>
                       //{
                       await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                       //  });
                       return;
                   }
                  
              

           });
        }

        string getSalutation(SalutationType salutationType)
        {
            switch (salutationType)
            {
                case SalutationType.Dr:
                    return "Dr";

                case SalutationType.Miss:
                    return "Miss";
                case SalutationType.Mr:
                    return "Mr";

                case SalutationType.Mrs:
                    return "Mrs";

                case SalutationType.Ms:
                    return "Ms";

            }
            return string.Empty;
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

        private async void SavePersonalData()
        {
            _cts = new CancellationTokenSource();
            await SavePersonalDataAsync();
        }

        private async Task SavePersonalDataAsync()
        {
            if (PersonalDataProxy.AccountId != 0)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized)
                {
                    await UpdatePersonalData();
                    await UpdateInitialTargets();
                }
            }
            else
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized)
                {
                    await CreatePersonalData();
                    await CreateInitialTargets();
                }
            }
        }

        private async Task UpdatePersonalData()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;
            PersonalDataProxy.UnitDisplayType = IsMetricUnit == true ? 1 : 0;
            PersonalDataProxy.DateTimeDisplayType = Is24HourDay == true ? 1 : 0;
            PersonalDataProxy.Country = SelectedCountry?.Text;
            PersonalDataProxy.Ethnicity = SelectedEthnicity;

            switch (SelectedSalutation?.Text)
            {
                case "Dr":
                    PersonalDataProxy.Salutation = SalutationType.Dr;
                    break;
                case "Miss":
                    PersonalDataProxy.Salutation = SalutationType.Miss;
                    break;
                case "Mr":
                    PersonalDataProxy.Salutation = SalutationType.Mr;
                    break;
                case "Mrs":
                    PersonalDataProxy.Salutation = SalutationType.Mrs;
                    break;
                case "Ms":
                    PersonalDataProxy.Salutation = SalutationType.Ms;
                    break;
            }

            try
            {
                var apiCallResult = await Provider.UpdatePersonalDataAsnyc(PersonalDataProxy, _cts.Token);

                if (apiCallResult.Success)
                {

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        Logger.LogInformation($"PersonalData for Account: {PersonalDataProxy.AccountId} successfully updated.");

                        PersonalDataProxy = null;

                        PersonalDataProxy = apiCallResult.Value;

                        SetDisplayUnits();
                    });
                }
                else
                {
                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                        {
                            IsBusy = false;
                            return;
                        }

                        await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.ErrorMessage, AppResources.Button_Ok);

                        IsBusy = false;
                        return;
                    });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Execption in UpdatePersonalData " + ex.Message + "Stacktrace: " + ex.StackTrace);
            }
        }

        private async Task CreatePersonalData()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;

            PersonalDataProxy.UnitDisplayType = IsMetricUnit == true ? 1 : 0;
            PersonalDataProxy.DateTimeDisplayType = Is24HourDay == true ? 1 : 0;
            PersonalDataProxy.Country = SelectedCountry.Text;
            PersonalDataProxy.Ethnicity = SelectedEthnicity;

            switch (SelectedSalutation?.Text)
            {
                case "Dr":
                    PersonalDataProxy.Salutation = SalutationType.Dr;
                    break;
                case "Miss":
                    PersonalDataProxy.Salutation = SalutationType.Miss;
                    break;
                case "Mr":
                    PersonalDataProxy.Salutation = SalutationType.Mr;
                    break;
                case "Mrs":
                    PersonalDataProxy.Salutation = SalutationType.Mrs;
                    break;
                case "Ms":
                    PersonalDataProxy.Salutation = SalutationType.Ms;
                    break;
            }

            var apiCallResult = await Provider.CreatePersonalDataAsnyc(PersonalDataProxy, _cts.Token);
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (apiCallResult.Success)
                {
                    Logger.LogInformation($"PersonalData for Account: {PersonalDataProxy.AccountId} successfully created.");
                    PersonalDataProxy = apiCallResult.Value;
                    IsMetricUnit = PersonalDataProxy.UnitDisplayType == 1 ? true : false;
                    Is24HourDay = PersonalDataProxy.DateTimeDisplayType == 1 ? true : false;
                    SetDisplayUnits();
                }
                else
                {
                    if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                    {
                        IsBusy = false;
                        return;
                    }

                    await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.ErrorMessage, AppResources.Button_Ok);

                    IsBusy = false;
                    return;
                }
            });


        }

        private async Task UpdateInitialTargets()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;
            PersonalDataProxy.UnitDisplayType = IsMetricUnit == true ? 1 : 0;
            PersonalDataProxy.DateTimeDisplayType = Is24HourDay == true ? 1 : 0;

            var age = DateTime.Today.Year - PersonalDataProxy.YearOfBirth;

            var bloodGlucoseLevel = 6.0;
            var fastingType = 0;

            var personalGoalProxy = Provider.CreateInitialTargets(age, (int)PersonalDataProxy.Gender, PersonalDataProxy.Weight, PersonalDataProxy.BodyMassIndex, PersonalDataProxy.BodyFat, bloodGlucoseLevel, fastingType);

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
                return;
            }

            IsBusy = false;
        }

        private async Task CreateInitialTargets()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;

            var age = DateTime.Today.Year - PersonalDataProxy.YearOfBirth;

            var bloodGlucoseLevel = 6.0;
            var fastingType = 0;

            var personalGoalProxy = Provider.CreateInitialTargets(age, (int)PersonalDataProxy.Gender, PersonalDataProxy.Weight, PersonalDataProxy.BodyMassIndex, PersonalDataProxy.BodyFat, bloodGlucoseLevel, fastingType);

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
                return;
            }

            IsBusy = false;
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

        private double _minWeight;

        public double MinWeight
        {
            get => _minWeight;
            set => SetProperty(ref _minWeight, value);
        }

        private double _maxWeight;

        public double MaxWeight
        {
            get => _maxWeight;
            set => SetProperty(ref _maxWeight, value);
        }

        private double _minHeight;

        public double MinHeight
        {
            get => _minHeight;
            set => SetProperty(ref _minHeight, value);
        }

        private double _maxHeight;

        public double MaxHeight
        {
            get => _maxHeight;
            set => SetProperty(ref _maxHeight, value);
        }

        private double _minNeckDiameter;

        public double MinNeckDiameter
        {
            get => _minNeckDiameter;
            set => SetProperty(ref _minNeckDiameter, value);
        }

        private double _maxNeckDiameter;

        public double MaxNeckDiameter
        {
            get => _maxNeckDiameter;
            set => SetProperty(ref _maxNeckDiameter, value);
        }

        private double _minWaistDiameter;

        public double MinWaistDiameter
        {
            get => _minWaistDiameter;
            set => SetProperty(ref _minWaistDiameter, value);
        }

        private double _maxWaistDiameter;

        public double MaxWaistDiameter
        {
            get => _maxWaistDiameter;
            set => SetProperty(ref _maxWaistDiameter, value);
        }

        private double _minHipDiameter;

        public double MinHipDiameter
        {
            get => _minHipDiameter;
            set => SetProperty(ref _minHipDiameter, value);
        }

        private double _maxHipDiameter;

        public double MaxHipDiameter
        {
            get => _maxHipDiameter;
            set => SetProperty(ref _maxHipDiameter, value);
        }

        private string _weightSliderUnit;

        public string WeightSliderUnit
        {
            get => _weightSliderUnit;
            set => SetProperty(ref _weightSliderUnit, value);
        }

        private string _widthSliderUnit;
        private bool _isMetricUnit;
        private bool _is24HourDay;
        private ComboBoxItem _selectedCountry;
        private string _selectedEthnicity;
        private ComboBoxItem _selectedSalutation;

        public string WidthSliderUnit
        {
            get => _widthSliderUnit;
            set => SetProperty(ref _widthSliderUnit, value);
        }

        public bool IsMetricUnit
        {
            get => _isMetricUnit;
            set
            {
                SetProperty(ref _isMetricUnit, value);
                if (PersonalDataProxy != null)
                {
                    PersonalDataProxy.UnitDisplayType = _isMetricUnit == true ? 1 : 0;
                }
            }
        }

        public bool Is24HourDay
        {
            get => _is24HourDay;
            set
            {
                SetProperty(ref _is24HourDay, value);
                if (PersonalDataProxy != null)
                {
                    PersonalDataProxy.DateTimeDisplayType = _is24HourDay == true ? 1 : 0;
                }
            }
        }

        public ComboBoxItem SelectedCountry
        {
            get => _selectedCountry;
            set => SetProperty(ref _selectedCountry, value);
        }

        public string SelectedEthnicity
        {
            get => _selectedEthnicity;
            set => SetProperty(ref _selectedEthnicity, value);
        }

        public ComboBoxItem SelectedSalutation
        {
            get => _selectedSalutation;
            set => SetProperty(ref _selectedSalutation, value);
        }
    }
}