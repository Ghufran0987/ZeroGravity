using System;
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
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Providers;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.ViewModels
{
    public class WizardFinishSetupPageViewModel : VmBase<IWizardFinishSetupPage, IWizardFinishSetupPageVmProvider, WizardFinishSetupPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        public DelegateCommand WizardFinishSetupSaveDataCommand { get; }

        private IPersonalDataPageVmProvider _personalProvider;
        private ProgressProxy progressProxyModel;

        public ProgressProxy ProgressProxyModel
        {
            get { return progressProxyModel; }
            set { SetProperty(ref progressProxyModel, value); }
        }

        private readonly int _initialMinMealAmount;
        private IWizardNewPageVmProvider _wizardNewVMProvider;

        public WizardFinishSetupPageViewModel(IVmCommonService service, IWizardFinishSetupPageVmProvider provider,
            ILoggerFactory loggerFactory, IEventAggregator eventAggregator, IWizardNewPageVmProvider wizardNewVMProvider,
            IPersonalDataPageVmProvider personalProvider,
            bool checkInternet = true) : base(service, provider, loggerFactory, checkInternet)
        {
            _wizardNewVMProvider = wizardNewVMProvider;
            //MinWaterConsumption = 0.0;
            //MaxWaterConsumption = 5.0;

            //MinCalorieDrinkConsumption = 0.0;
            //MaxCalorieDrinkConsumption = 2.0;

            //_initialMinMealAmount = (int)FoodAmountType.VeryLight;
            //MinMealAmount = _initialMinMealAmount;
            //MaxMealAmount = (int)FoodAmountType.VeryHeavy;

            //MinActivityDuration = 0.0;
            //MaxActivityDuration = 16.0;
            ProgressProxyModel = new ProgressProxy();
            _eventAggregator = eventAggregator;
            WizardFinishSetupSaveDataCommand = new DelegateCommand(WizardFinishSetupSaveDataExecute);
            _personalProvider = personalProvider;
            SetDisplayUnits();

            LogoImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");
        }

        private ImageSource _logoImageSource;

        public ImageSource LogoImageSource
        {
            get => _logoImageSource;
            set => SetProperty(ref _logoImageSource, value);
        }

        private ImageSource _genderImage;

        public ImageSource GenderImage
        {
            get => _genderImage;
            set => SetProperty(ref _genderImage, value);
        }

        private string _liquidUnitDisplay;

        public string LiquidUnitDisplay
        {
            get => _liquidUnitDisplay;
            set => SetProperty(ref _liquidUnitDisplay, value);
        }

        private string _fastingMsgDisplay;

        public string FastingMsgDisplay
        {
            get => _fastingMsgDisplay;
            set => SetProperty(ref _fastingMsgDisplay, value);
        }

        private void SetDisplayUnits()
        {
            var displayPreferences = DisplayConversionService.GetDisplayPrefences();

            LiquidUnitDisplay = displayPreferences.UnitDisplayType == UnitDisplayType.Metric
                ? string.Format(AppResources.PersonalGoal_PerDay, AppResources.Units_Liter)
                : string.Format(AppResources.PersonalGoal_PerDay, AppResources.Units_Ounce);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //  Title = DateTimeHelper.ToLocalDateZeroGravityFormat(DateTime.Now);

            BodyFatGoalActive = true;
            if (!HasInternetConnection)
            {
                return;
            }

            IsBusy = true;
            _cts = new CancellationTokenSource();

            try
            {
                if (await ValidateToken())
                {
                    await GetPersonalGoalAsync();
                }
            }
            catch (Exception e)
            {
                IsBusy = false;
                Logger.LogError(e, e.Message);
                await Service.DialogService.DisplayAlertAsync(AppResources.AnalysisPage_SubTitle, AppResources.AnalysisPage_LoadData_ErrorMessage,
                    AppResources.Button_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private bool IsGoalsCreated = false;

        private async Task GetPersonalGoalAsync()
        {
            var apiCallPersonalDataResult = await Provider.GetPersonalDataAsnyc(_cts.Token);

            if (apiCallPersonalDataResult.Success)
            {
                persnalDataProxy = apiCallPersonalDataResult.Value;
                Bmi = apiCallPersonalDataResult.Value.BodyMassIndex;
                BodyFat = apiCallPersonalDataResult.Value.BodyFat;
                switch (persnalDataProxy.Gender)
                {
                    case GenderBiologicalType.Male:
                        GenderImage = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Male.png");
                        break;

                    case GenderBiologicalType.Female:
                        GenderImage = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Female.png");
                        break;

                    case GenderBiologicalType.NonBinary:
                    case GenderBiologicalType.Undefined:
                        GenderImage = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.neutral_gender.png");
                        break;

                    default:
                        GenderImage = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.neutral_gender.png");
                        break;
                }

                await SavePersonalGoal();

                if (IsGoalsCreated == true)
                {
                    SetDisplayUnits();

                    ProgressProxyModel.WaterProgress = new LiquidProgress("\uf75c", AppResources.AnalysisPage_Items_Water, 0, PersonalGoalProxy.WaterConsumption);
                    progressProxyModel.BreakFastProgress = new FoodProgress("\uf7f6", AppResources.AnalysisPage_Items_Breakfast, 0, (FoodAmountType)PersonalGoalProxy.BreakfastAmount);
                    progressProxyModel.LunchProgress = new FoodProgress("\uf7f6", AppResources.AnalysisPage_Items_Lunch, 0, (FoodAmountType)PersonalGoalProxy.LunchAmount);
                    progressProxyModel.DinnerProgress = new FoodProgress("\uf7f6", AppResources.AnalysisPage_Items_Dinner, 0, (FoodAmountType)PersonalGoalProxy.DinnerAmount);
                    progressProxyModel.HealthySnacksProgress = new FoodProgress("\uf7f6", AppResources.AnalysisPage_Items_HealthySnack, 0, (FoodAmountType)PersonalGoalProxy.HealthySnackAmount);
                    progressProxyModel.UnHealthySnacksProgress = new FoodProgress("\uf564", AppResources.AnalysisPage_Items_UnhealthySnack, 0, (FoodAmountType)PersonalGoalProxy.UnhealthySnackAmount);
                    progressProxyModel.CalorieProgress = new LiquidProgress("\uf869", AppResources.AnalysisPage_Items_CalorieDrink, 0, PersonalGoalProxy.CalorieDrinkConsumption);
                    progressProxyModel.ActivitiesProgress = new FoodProgress("\uf554", AppResources.AnalysisPage_Items_Activity,
                                    0, GetActivityStatus(PersonalGoalProxy.ActivityDuration));
                    progressProxyModel.FastingProgress = new FoodProgress("\uf554", AppResources.AnalysisPage_Items_Activity,
                                    0, GetFastingSatatus(PersonalGoalProxy.FastingDuration));
                    if (PersonalGoalProxy.FastingDuration == 0)
                    {
                        FastingMsgDisplay = "You have opted out of fasting goals as part of your onboarding. If you wish to opt back in edit the fasting profile setting to your desired fasting duration.";
                    }
                    else
                    {
                        FastingMsgDisplay = "Recommended target twice per week";
                    }
                    progressProxyModel.MeditationProgress = new FoodProgress("\uf554", AppResources.AnalysisPage_Items_Activity,
                                    0, GetMeditationSatatus(PersonalGoalProxy.MeditationDuration));
                }
            }
            else
            {
                //TODO Could not be loaded
            }
        }

        private async void WizardFinishSetupSaveDataExecute()
        {
            switch (GoalSetupPageStates)
            {
                case GoalSetupPageState.BodyFatGoal:
                    GoalSetupPageStates = GoalSetupPageState.BodyGoal;
                    break;

                case GoalSetupPageState.BodyGoal:
                    GoalSetupPageStates = GoalSetupPageState.WaterGoal;
                    break;

                case GoalSetupPageState.WaterGoal:
                    GoalSetupPageStates = GoalSetupPageState.BreakFastGoal;
                    break;

                case GoalSetupPageState.BreakFastGoal:
                    GoalSetupPageStates = GoalSetupPageState.LunchGoal;
                    break;

                case GoalSetupPageState.LunchGoal:
                    GoalSetupPageStates = GoalSetupPageState.DinnerGoal;
                    break;

                case GoalSetupPageState.DinnerGoal:
                    GoalSetupPageStates = GoalSetupPageState.HealthyGoal;
                    break;

                case GoalSetupPageState.HealthyGoal:
                    GoalSetupPageStates = GoalSetupPageState.UnHealthyGoal;
                    break;

                case GoalSetupPageState.UnHealthyGoal:
                    GoalSetupPageStates = GoalSetupPageState.CalorieGoal;
                    break;

                case GoalSetupPageState.CalorieGoal:
                    GoalSetupPageStates = GoalSetupPageState.ActivityGoal;
                    break;

                case GoalSetupPageState.ActivityGoal:
                    GoalSetupPageStates = GoalSetupPageState.FastingGoal;
                    break;

                case GoalSetupPageState.FastingGoal:
                    GoalSetupPageStates = GoalSetupPageState.MeditationGoal;
                    break;

                case GoalSetupPageState.MeditationGoal:
                    GoalSetupPageStates = GoalSetupPageState.Finish;
                    break;
            }

            if (GoalSetupPageStates != GoalSetupPageState.Finish)
                return;
            try
            {
                _cts = new CancellationTokenSource();
                IsBusy = true;

                await SavePersonalGoal();
                IsBusy = true;
                var updateDto = new UpdateWizardRequestDto
                {
                    CompletedFirstUseWizard = true
                };

                await Provider.UpdateWizardCompletionStatusAsync(updateDto, _cts.Token);
                await Service.NavigationService.NavigateAsync("/" + ViewName.ContentShellPage);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
            finally
            {
                //IsBusy = false;
            }
        }

        private async Task SavePersonalGoal()
        {
            var isAuthorized = await ValidateToken();
            if (isAuthorized) await UpdateInitialTargets();
        }

        private async Task CreatePersonalGoal()
        {
            if (!HasInternetConnection)
            {
                return;
            }

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

        private async Task UpdatePersonalGoal()
        {
            if (!HasInternetConnection)
            {
                return;
            }

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

        private async Task UpdateInitialTargets()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;
            persnalDataProxy.UnitDisplayType = 1;
            persnalDataProxy.DateTimeDisplayType = 0;

            var age = DateTime.Today.Year - persnalDataProxy.YearOfBirth;

            var bloodGlucoseLevel = 6.0;
            var fastingType = 0;


            PersonalGoalProxy = _personalProvider.CreateInitialTargets(age, (int)persnalDataProxy.Gender, persnalDataProxy.Weight, persnalDataProxy.BodyMassIndex, persnalDataProxy.BodyFat, bloodGlucoseLevel, fastingType);

            PersonalGoalProxy.AccountId = persnalDataProxy.AccountId;

            var questions = await _wizardNewVMProvider.GetQuestionsAsync("all", true, _cts.Token);
            if (questions.Value != null)
            {
                var desiredWeightQuestion = questions.Value.FirstOrDefault(x => x.DataFieldType == DataFieldType.DesiredWeight);
                var fastingQuestion = questions.Value.FirstOrDefault(x => x.DataFieldType == DataFieldType.FastingGoal);
                var desiredWeight = (persnalDataProxy.QuestionAnswers.FirstOrDefault(x => x.QuestionId == desiredWeightQuestion.Id)).Value;

                if (persnalDataProxy != null && persnalDataProxy.QuestionAnswers != null)
                {
                    var q1 = persnalDataProxy.QuestionAnswers.FirstOrDefault(x => x.QuestionId == 1);
                    var weightUnit = q1 != null ? q1.Tag1 : string.Empty;
                    PersonalGoalProxy.Weight = Convert.ToDouble(desiredWeight);
                    Weight = PersonalGoalProxy.Weight.ToString();
                    WeightUnit = weightUnit;
                }
                var fastingValue = persnalDataProxy.QuestionAnswers.FirstOrDefault(x => x.QuestionId == fastingQuestion.Id).Value;
                if (fastingValue == "1")
                {
                    PersonalGoalProxy.FastingDuration = 16;
                }
                else
                {
                    PersonalGoalProxy.FastingDuration = 0;
                }
                PersonalGoalProxy.MeditationDuration = 10;
            }
            PersonalGoalProxy.BodyFat = BodyFat;
            PersonalGoalProxy.BodyMassIndex = Bmi;

            var apiCallResult = await Provider.GetPersonalGoalAsnyc(_cts.Token);

            if (apiCallResult?.Value?.Id > 0)
            {
                PersonalGoalProxy.Id = apiCallResult.Value.Id;
                var result = await Provider.UpdatePersonalGoalAsnyc(PersonalGoalProxy, _cts.Token);

                if (result.Success)
                {
                    Logger.LogInformation($"PersonalTargets for Account: {PersonalGoalProxy.AccountId} successfully updated.");
                    IsGoalsCreated = true;
                }
                else
                {
                    if (result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || result.ErrorReason == ErrorReason.TimeOut)
                    {
                        IsGoalsCreated = false;
                        IsBusy = false;
                        return;
                    }

                    await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.ErrorMessage, AppResources.Button_Ok);

                    Logger.LogInformation($"PersonalTargets for Account: {PersonalGoalProxy.AccountId} update failed.");
                    IsGoalsCreated = false;
                }

                IsBusy = false;
            }
            else
            {
                var result = await Provider.CreatePersonalGoalAsnyc(PersonalGoalProxy, _cts.Token);

                if (result.Success)
                {
                    Logger.LogInformation($"PersonalTargets for Account: {PersonalGoalProxy.AccountId} successfully updated.");
                    IsGoalsCreated = true;
                }
                else
                {
                    if (result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || result.ErrorReason == ErrorReason.TimeOut)
                    {
                        IsGoalsCreated = false;
                        IsBusy = false;
                        return;
                    }

                    await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.ErrorMessage, AppResources.Button_Ok);

                    Logger.LogInformation($"PersonalTargets for Account: {PersonalGoalProxy.AccountId} create failed.");
                    IsGoalsCreated = false;
                }

                IsBusy = false;
            }
        }

        private async Task CreateInitialTargets()
        {
            if (!HasInternetConnection) return;

            IsBusy = true;

            var age = DateTime.Today.Year - persnalDataProxy.YearOfBirth;

            var bloodGlucoseLevel = 6.0;
            var fastingType = 0;

            var personalGoalProxy = _personalProvider.CreateInitialTargets(age, (int)persnalDataProxy.Gender, persnalDataProxy.Weight, persnalDataProxy.BodyMassIndex, persnalDataProxy.BodyFat, bloodGlucoseLevel, fastingType);

            personalGoalProxy.AccountId = persnalDataProxy.AccountId;

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

        private PersonalGoalProxy _personalGoalProxy;

        public PersonalGoalProxy PersonalGoalProxy
        {
            get => _personalGoalProxy;
            set => SetProperty(ref _personalGoalProxy, value);
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

        private string _weight;

        public string Weight
        {
            get => _weight;
            set => SetProperty(ref _weight, value);
        }

        private string _weightUnit;

        public string WeightUnit
        {
            get => _weightUnit;
            set => SetProperty(ref _weightUnit, value);
        }

        private double _minWaterConsumption;

        public double MinWaterConsumption
        {
            get => _minWaterConsumption;
            set => SetProperty(ref _minWaterConsumption, value);
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

        private double _minActivityDuration;

        public double MinActivityDuration
        {
            get => _minActivityDuration;
            set => SetProperty(ref _minActivityDuration, value);
        }

        private double _maxActivityDuration;

        public double MaxActivityDuration
        {
            get => _maxActivityDuration;
            set => SetProperty(ref _maxActivityDuration, value);
        }

        private CancellationTokenSource _cts;

        private bool _bodyFatGoalActive;

        public bool BodyFatGoalActive
        {
            get => _bodyFatGoalActive;
            set => SetProperty(ref _bodyFatGoalActive, value);
        }

        private bool _bodyGoalActive;

        public bool BodyGoalActive
        {
            get => _bodyGoalActive;
            set => SetProperty(ref _bodyGoalActive, value);
        }

        private bool _waterGoalActive;

        public bool WaterGoalActive
        {
            get => _waterGoalActive;
            set => SetProperty(ref _waterGoalActive, value);
        }

        private bool _breakFastGoalActive;

        public bool BreakFastGoalActive
        {
            get => _breakFastGoalActive;
            set => SetProperty(ref _breakFastGoalActive, value);
        }

        private bool _lunchGoalActive;

        public bool LunchGoalActive
        {
            get => _lunchGoalActive;
            set => SetProperty(ref _lunchGoalActive, value);
        }

        private bool _dinnerGoalActive;

        public bool DinnerGoalActive
        {
            get => _dinnerGoalActive;
            set => SetProperty(ref _dinnerGoalActive, value);
        }

        private bool _healthyGoalActive;

        public bool HealthyGoalActive
        {
            get => _healthyGoalActive;
            set => SetProperty(ref _healthyGoalActive, value);
        }

        private bool _unHealthyGoalActive;

        public bool UnHealthyGoalActive
        {
            get => _unHealthyGoalActive;
            set => SetProperty(ref _unHealthyGoalActive, value);
        }

        private bool _calorieGoalActive;

        public bool CalorieGoalActive
        {
            get => _calorieGoalActive;
            set => SetProperty(ref _calorieGoalActive, value);
        }

        private bool _activityGoalActive;

        public bool ActivityGoalActive
        {
            get => _activityGoalActive;
            set => SetProperty(ref _activityGoalActive, value);
        }

        private bool _fastingGoalActive;

        public bool FastingGoalActive
        {
            get => _fastingGoalActive;
            set => SetProperty(ref _fastingGoalActive, value);
        }

        private bool _meditationGoalActive;

        public bool MeditationGoalActive
        {
            get => _meditationGoalActive;
            set => SetProperty(ref _meditationGoalActive, value);
        }

        private GoalSetupPageState _goalSetupPageState;
        private PersonalDataProxy persnalDataProxy;

        public GoalSetupPageState GoalSetupPageStates
        {
            get => _goalSetupPageState;
            set => SetProperty(ref _goalSetupPageState, value, OnGoalSetUpPageStateChange);
        }

        private void OnGoalSetUpPageStateChange()
        {
            switch (GoalSetupPageStates)
            {
                case GoalSetupPageState.BodyFatGoal:
                    BodyFatGoalActive = true;
                    BodyGoalActive = false;
                    WaterGoalActive = false;
                    BreakFastGoalActive = false;
                    LunchGoalActive = false;
                    DinnerGoalActive = false;
                    HealthyGoalActive = false;
                    UnHealthyGoalActive = false;
                    CalorieGoalActive = false;
                    ActivityGoalActive = false;
                    FastingGoalActive = false;
                    MeditationGoalActive = false;
                    break;

                case GoalSetupPageState.BodyGoal:
                    BodyFatGoalActive = false;
                    BodyGoalActive = true;

                    break;

                case GoalSetupPageState.WaterGoal:
                    BodyGoalActive = false;
                    WaterGoalActive = true;
                    break;

                case GoalSetupPageState.BreakFastGoal:
                    WaterGoalActive = false;
                    BreakFastGoalActive = true;
                    break;

                case GoalSetupPageState.LunchGoal:
                    BreakFastGoalActive = false;
                    LunchGoalActive = true;
                    break;

                case GoalSetupPageState.DinnerGoal:
                    LunchGoalActive = false;
                    DinnerGoalActive = true;
                    break;

                case GoalSetupPageState.HealthyGoal:
                    DinnerGoalActive = false;
                    HealthyGoalActive = true;
                    break;

                case GoalSetupPageState.UnHealthyGoal:
                    HealthyGoalActive = false;
                    UnHealthyGoalActive = true;
                    break;

                case GoalSetupPageState.CalorieGoal:
                    UnHealthyGoalActive = false;
                    CalorieGoalActive = true;
                    break;

                case GoalSetupPageState.ActivityGoal:
                    CalorieGoalActive = false;
                    ActivityGoalActive = true;
                    break;

                case GoalSetupPageState.FastingGoal:
                    ActivityGoalActive = false;
                    FastingGoalActive = true;
                    break;

                case GoalSetupPageState.MeditationGoal:
                    FastingGoalActive = false;
                    MeditationGoalActive = true;
                    break;

                case GoalSetupPageState.Finish:
                    BodyFatGoalActive = false;
                    BodyGoalActive = false;
                    WaterGoalActive = false;
                    BreakFastGoalActive = false;
                    LunchGoalActive = false;
                    DinnerGoalActive = false;
                    HealthyGoalActive = false;
                    UnHealthyGoalActive = false;
                    CalorieGoalActive = false;
                    ActivityGoalActive = false;
                    FastingGoalActive = false;
                    MeditationGoalActive = false;
                    break;

                default:
                    break;
            }
        }

        private FoodAmountType GetActivityStatus(double activityAmount)
        {
            //Converting to mins from hours
            var ActivityAmount = activityAmount * 60;

            var activityAmountType = FoodAmountType.None;
            if (ActivityAmount >= 40)
            {
                activityAmountType = FoodAmountType.VeryHeavy;
            }
            else if (ActivityAmount >= 35 && ActivityAmount < 40)
            {
                activityAmountType = FoodAmountType.Heavy;
            }
            else if (ActivityAmount >= 30 && ActivityAmount < 35)
            {
                activityAmountType = FoodAmountType.Medium;
            }
            else if (ActivityAmount >= 25 && ActivityAmount < 30)
            {
                activityAmountType = FoodAmountType.Light;
            }
            else if (ActivityAmount >= 20 && ActivityAmount < 25)
            {
                activityAmountType = FoodAmountType.VeryLight;
            }
            else if (ActivityAmount < 0)
            {
                activityAmountType = FoodAmountType.None;
            }
            return activityAmountType;
        }

        private FoodAmountType GetFastingSatatus(double fastingAmount)
        {
            var activityAmountType = FoodAmountType.None;
            if (fastingAmount >= 24)
            {
                activityAmountType = FoodAmountType.VeryHeavy;
            }
            else if (fastingAmount >= 20 && fastingAmount < 24)
            {
                activityAmountType = FoodAmountType.Heavy;
            }
            else if (fastingAmount >= 16 && fastingAmount < 20)
            {
                activityAmountType = FoodAmountType.Medium;
            }
            else if (fastingAmount >= 12 && fastingAmount < 16)
            {
                activityAmountType = FoodAmountType.Light;
            }
            else if (fastingAmount >= 8 && fastingAmount < 12)
            {
                activityAmountType = FoodAmountType.VeryLight;
            }
            else if (fastingAmount < 8)
            {
                activityAmountType = FoodAmountType.None;
            }
            return activityAmountType;
        }

        private FoodAmountType GetMeditationSatatus(double meditationAmount)
        {
            var meditationAmountType = FoodAmountType.None;
            if (meditationAmount >= 30)
            {
                meditationAmountType = FoodAmountType.VeryHeavy;
            }
            else if (meditationAmount >= 25 && meditationAmount < 30)
            {
                meditationAmountType = FoodAmountType.Heavy;
            }
            else if (meditationAmount >= 20 && meditationAmount < 25)
            {
                meditationAmountType = FoodAmountType.Medium;
            }
            else if (meditationAmount >= 15 && meditationAmount < 20)
            {
                meditationAmountType = FoodAmountType.Light;
            }
            else if (meditationAmount >= 10 && meditationAmount < 15)
            {
                meditationAmountType = FoodAmountType.VeryLight;
            }
            else if (meditationAmount < 10)
            {
                meditationAmountType = FoodAmountType.None;
            }
            return meditationAmountType;
        }
    }
}