using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class PersonalGoalsPageViewModel : VmBase<IPersonalGoalsPage, IPersonalGoalsPageVmProvider, PersonalGoalsPageViewModel>
    {
        public DelegateCommand SavePersonalGoalsDelegateCommand { get; }

        private readonly int _initialMinMealAmount;

        public PersonalGoalsPageViewModel(IVmCommonService service, IPersonalGoalsPageVmProvider provider, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            SavePersonalGoalsDelegateCommand = new DelegateCommand(SavePersonalGoalExecute);

            MinWaterConsumption = 0.0;
            MaxWaterConsumption = 16;

            MinCalorieDrinkConsumption = 0.0;
            MaxCalorieDrinkConsumption = 16;

            MinActivityDuration = 20;
            MaxActivityDuration = 40;

            MinFastingDuration = 0.0;
            MaxFastingDuration = 24.0;


            MinMeditationDuration = 0.0;
            MaxMeditationDuration = 30.00;

            _initialMinMealAmount = (int)FoodAmountType.VeryLight;
            MinMealAmount = _initialMinMealAmount;
            MaxMealAmount = (int)FoodAmountType.VeryHeavy;

            _cts = new CancellationTokenSource();
        }

        private string _liquidUnitDisplay;

        public string LiquidUnitDisplay
        {
            get => _liquidUnitDisplay;
            set => SetProperty(ref _liquidUnitDisplay, value);
        }


        private int _activityDuration;

        public int ActivityDuration
        {
            get => _activityDuration;
            set => SetProperty(ref _activityDuration, value);
        }

        //private void SetDisplayUnits()
        //{
        //    //  var displayPreferences = DisplayConversionService.GetDisplayPrefences();

        //    //LiquidUnitDisplay = displayPreferences.UnitDisplayType == UnitDisplayType.Metric
        //    //    ? string.Format(AppResources.PersonalGoal_PerDay, AppResources.Units_Liter)
        //    //    : string.Format(AppResources.PersonalGoal_PerDay, AppResources.Units_Ounce);
        //    LiquidUnitDisplay = "Cups";
        //}

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            _cts = new CancellationTokenSource();
            GetPersonalGoalAsync();
        }

        private async Task GetPersonalGoalAsync()
        {
            ShowProgress = true;
            PersonalGoalProxy = new PersonalGoalProxy();
            Provider.GetPersonalDataAsnyc(_cts.Token).ContinueWith(async apiCallPersonalDataResult =>
            {
                if (apiCallPersonalDataResult.Result.Success)
                {
                    Bmi = apiCallPersonalDataResult.Result.Value.BodyMassIndex;
                    BodyFat = apiCallPersonalDataResult.Result.Value.BodyFat;
                }

                Provider.GetPersonalGoalAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
                {
                    if (apiCallResult.Result.Success)
                    {
                        Logger.LogInformation($"PersonalGoal for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");

                        PersonalGoalProxy = apiCallResult.Result.Value;

                        ActivityDuration = (int)(PersonalGoalProxy.ActivityDuration * 60);

                        var watCups = DisplayConversionService.ConvertLtrToCups(PersonalGoalProxy.WaterConsumption);
                        WaterConsumption = watCups;

                        var calcups = DisplayConversionService.ConvertLtrToCups(PersonalGoalProxy.CalorieDrinkConsumption);
                        CalorieDrinkConsumption = calcups;

                        // SetDisplayUnits();

                        //workaround für den komisch aussehenden Slider bei iOS, wenn Value <= MinimumValue

                        if (Device.RuntimePlatform == Device.iOS)
                        {
                            if (PersonalGoalProxy.BreakfastAmount < _initialMinMealAmount)
                            {
                                PersonalGoalProxy.BreakfastAmount = _initialMinMealAmount;
                            }
                            if (PersonalGoalProxy.LunchAmount < _initialMinMealAmount)
                            {
                                PersonalGoalProxy.LunchAmount = _initialMinMealAmount;
                            }
                            if (PersonalGoalProxy.DinnerAmount < _initialMinMealAmount)
                            {
                                PersonalGoalProxy.DinnerAmount = _initialMinMealAmount;
                            }
                            if (PersonalGoalProxy.HealthySnackAmount < _initialMinMealAmount)
                            {
                                PersonalGoalProxy.HealthySnackAmount = _initialMinMealAmount;
                            }
                            if (PersonalGoalProxy.UnhealthySnackAmount < _initialMinMealAmount)
                            {
                                PersonalGoalProxy.UnhealthySnackAmount = _initialMinMealAmount;
                            }
                        }
                    }
                    else
                    {
                        if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                        {
                            ShowProgress = false;
                            return;
                        }
                        ShowProgress = false;
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                        });
                        return;
                    }
                    ShowProgress = false;
                });
            });
        }

        private async void SavePersonalGoalExecute()
        {
            _cts = new CancellationTokenSource();
            await SavePersonalGoalAsync();
        }

        private async Task SavePersonalGoalAsync()
        {

            PersonalGoalProxy.WaterConsumption = DisplayConversionService.ConvertCupsToLtr(WaterConsumption); ;


            PersonalGoalProxy.CalorieDrinkConsumption = DisplayConversionService.ConvertCupsToLtr(CalorieDrinkConsumption); ;
            PersonalGoalProxy.ActivityDuration = ((double)ActivityDuration / 60);
            if (PersonalGoalProxy.Id != 0)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await UpdatePersonalGoal();
            }
            else
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await CreatePersonalGoal();
            }
        }

        private async Task UpdatePersonalGoal()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;

            await ValidateToken();

            var apiCallResult = await Provider.UpdatePersonalGoalAsnyc(PersonalGoalProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"PersonalGoal for Account: {PersonalGoalProxy.AccountId} successfully updated.");

                PersonalGoalProxy = apiCallResult.Value;
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

            IsBusy = false;
        }

        private async Task CreatePersonalGoal()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;

            await ValidateToken();

            var apiCallResult = await Provider.CreatePersonalGoalAsnyc(PersonalGoalProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"PersonalGoal for Account: {PersonalGoalProxy.AccountId} successfully created.");

                PersonalGoalProxy = apiCallResult.Value;
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

            IsBusy = false;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);

            PersonalGoalProxy = null;
        }

        private PersonalGoalProxy _personalGoalProxy;
        public PersonalGoalProxy PersonalGoalProxy
        {
            get => _personalGoalProxy;
            set => SetProperty(ref _personalGoalProxy, value);
        }

        private double _minMaterConsumption;
        public double MinWaterConsumption
        {
            get => _minMaterConsumption;
            set => SetProperty(ref _minMaterConsumption, value);
        }

        private double _maxWaterConsumption;
        public double MaxWaterConsumption
        {
            get => _maxWaterConsumption;
            set => SetProperty(ref _maxWaterConsumption, value);
        }

        private double _minCalorieDrinkConsumption;
        public double MinCalorieDrinkConsumption
        {
            get => _minCalorieDrinkConsumption;
            set => SetProperty(ref _minCalorieDrinkConsumption, value);
        }

        private double _maxCalorieDrinkConsumption;
        public double MaxCalorieDrinkConsumption
        {
            get => _maxCalorieDrinkConsumption;
            set => SetProperty(ref _maxCalorieDrinkConsumption, value);
        }

        private double _waterConsumptionn;
        public double WaterConsumption
        {
            get => _waterConsumptionn;
            set => SetProperty(ref _waterConsumptionn, value);
        }

        private double _calorieDrinkConsumption;
        public double CalorieDrinkConsumption
        {
            get => _calorieDrinkConsumption;
            set => SetProperty(ref _calorieDrinkConsumption, value);
        }

        private int _minMealAmount;
        public int MinMealAmount
        {
            get => _minMealAmount;
            set => SetProperty(ref _minMealAmount, value);
        }

        private int _maxMealAmount;
        public int MaxMealAmount
        {
            get => _maxMealAmount;
            set => SetProperty(ref _maxMealAmount, value);
        }

        private double _maxFastingDuration;
        public double MaxFastingDuration
        {
            get => _maxFastingDuration;
            set => SetProperty(ref _maxFastingDuration, value);
        }

        private double _minFastingDurationn;
        public double MinFastingDuration
        {
            get => _minFastingDurationn;
            set => SetProperty(ref _minFastingDurationn, value);
        }

        private double _minMeditationDurationn;
        public double MinMeditationDuration
        {
            get => _minMeditationDurationn;
            set => SetProperty(ref _minMeditationDurationn, value);
        }

        private double _maxMeditationDurationn;
        public double MaxMeditationDuration
        {
            get => _maxMeditationDurationn;
            set => SetProperty(ref _maxMeditationDurationn, value);
        }



        private double _maxActivityDuration;
        public double MaxActivityDuration
        {
            get => _maxActivityDuration;
            set => SetProperty(ref _maxActivityDuration, value);
        }

        private double _minActivityDuration;
        public double MinActivityDuration
        {
            get => _minActivityDuration;
            set => SetProperty(ref _minActivityDuration, value);
        }

        private double _bmi;
        public double Bmi
        {
            get => _bmi;
            set => SetProperty(ref _bmi, value);
        }

        private double _bodyFat;
        public double BodyFat
        {
            get => _bodyFat;
            set => SetProperty(ref _bodyFat, value);
        }


        private CancellationTokenSource _cts;
    }
}