using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
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

namespace ZeroGravity.Mobile.ViewModels
{
    public class
        WizardStep2PageViewModel : VmBase<IWizardStep2Page, IPersonalDataPageVmProvider, WizardStep2PageViewModel>
    {
        private CancellationTokenSource _cts;

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

        private PersonalDataProxy _personalDataProxy;

        private string _weightSliderUnit;

        private string _widthSliderUnit;

        public WizardStep2PageViewModel(IVmCommonService service, IPersonalDataPageVmProvider provider,
            ILoggerFactory loggerFactory, bool checkInternet = true) : base(service, provider, loggerFactory,
            checkInternet)
        {
            WizardStep2SaveDataCommand = new DelegateCommand(SaveWizardStep2DataExecute);

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

        public DelegateCommand WizardStep2SaveDataCommand { get; }

        public string WeightSliderUnit
        {
            get => _weightSliderUnit;
            set => SetProperty(ref _weightSliderUnit, value);
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

        public PersonalDataProxy PersonalDataProxy
        {
            get => _personalDataProxy;
            set => SetProperty(ref _personalDataProxy, value);
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

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Title = AppResources.WizardStep_Page2Title;

            if (!HasInternetConnection) return;

            //IsBusy = true;

            //var isAuthorized = await ValidateToken();
            //if (isAuthorized) await

            GetPersonalDataAsync();

            IsBusy = false;
        }

        private async Task GetPersonalDataAsync()
        {
            _cts = new CancellationTokenSource();

            await Provider.GetPersonalDataAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
             {
                 if (apiCallResult.Result.Success)
                 {
                     Logger.LogInformation(
                       $"PersonalData for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");

                     PersonalDataProxy = apiCallResult.Result.Value;

                     SetDisplayUnits();
                 }
                 else
                 {
                     if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                       apiCallResult.Result.ErrorReason == ErrorReason.TimeOut) return;

                     await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title,
                       apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                 }
             });
        }

        private async void SaveWizardStep2DataExecute()
        {
            _cts = new CancellationTokenSource();

            await SavePersonalDataAsync();
            await Service.NavigationService.NavigateAsync(ViewName.WizardStep3Page);
        }

        private async Task SavePersonalDataAsync()
        {
            if (PersonalDataProxy == null)
                return;
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

        private async Task CreatePersonalData()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;

            var apiCallResult = await Provider.CreatePersonalDataAsnyc(PersonalDataProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"PersonalData for Account: {PersonalDataProxy.AccountId} successfully created.");

                PersonalDataProxy = apiCallResult.Value;
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                    apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title,
                    apiCallResult.ErrorMessage, AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async Task UpdatePersonalData()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;

            var apiCallResult = await Provider.UpdatePersonalDataAsnyc(PersonalDataProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"PersonalData for Account: {PersonalDataProxy.AccountId} successfully updated.");

                PersonalDataProxy = apiCallResult.Value;
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                    apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title,
                    apiCallResult.ErrorMessage, AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async Task CreateInitialTargets()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;

            await ValidateToken();

            var age = DateTime.Today.Year - PersonalDataProxy.YearOfBirth;

            var bloodGlucoseLevel = 6.0;
            var fastingType = 0;

            var personalGoalProxy = Provider.CreateInitialTargets(age, (int)PersonalDataProxy.Gender,
                PersonalDataProxy.Weight, PersonalDataProxy.BodyMassIndex, PersonalDataProxy.BodyFat, bloodGlucoseLevel,
                fastingType);

            personalGoalProxy.AccountId = PersonalDataProxy.AccountId;

            var apiCallResult = await Provider.CreatePersonalGoalAsnyc(personalGoalProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"PersonalTargets for Account: {personalGoalProxy.AccountId} successfully created.");
            }
            else
            {
                Logger.LogInformation($"PersonalTargets for Account: {personalGoalProxy.AccountId} creation failed.");

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async Task UpdateInitialTargets()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;

            var age = DateTime.Today.Year - PersonalDataProxy.YearOfBirth;

            var bloodGlucoseLevel = 6.0;
            var fastingType = 0;

            var personalGoalProxy = Provider.CreateInitialTargets(age, (int)PersonalDataProxy.Gender,
                PersonalDataProxy.Weight, PersonalDataProxy.BodyMassIndex, PersonalDataProxy.BodyFat, bloodGlucoseLevel,
                fastingType);

            personalGoalProxy.AccountId = PersonalDataProxy.AccountId;

            var apiCallResult = await Provider.UpdatePersonalGoalAsnyc(personalGoalProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"PersonalTargets for Account: {personalGoalProxy.AccountId} successfully updated.");
            }
            else
            {
                Logger.LogInformation($"PersonalTargets for Account: {personalGoalProxy.AccountId} update failed.");

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
    }
}