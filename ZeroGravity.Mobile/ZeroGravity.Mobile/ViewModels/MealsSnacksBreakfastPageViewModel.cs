using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Proxies.MealIngredientsProxy;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.ViewModels
{
    public class MealsSnacksBreakfastPageViewModel : VmBase<IMealsSnacksBreakfastPage,
        IMealsSnacksBreakfastPageVmProvider, MealsSnacksBreakfastPageViewModel>
    {
        #region Private Property

        private CancellationTokenSource _cts;
        private FastingDataMessageProxy _fastingData;
        private MealDataProxy _meal;
        private MealNavParams _navParams;
        private bool _openTimePicker;

        private bool _none;
        private bool _veryLight;
        private bool _light;
        private bool _medium;
        private bool _heavy;
        private bool _veryHeavy;

        #endregion Private Property

        #region Command

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand ShowTimePickerCommand { get; }

        #endregion Command

        public bool None
        {
            get { return _none; }
            set => SetProperty(ref _none, value, OnFoodAmountTypeChange);
        }

        public bool VeryLight
        {
            get { return _veryLight; }
            set => SetProperty(ref _veryLight, value, OnFoodAmountTypeChange);
        }

        public bool Light
        {
            get { return _light; }
            set => SetProperty(ref _light, value, OnFoodAmountTypeChange);
        }

        public bool Medium
        {
            get { return _medium; }
            set => SetProperty(ref _medium, value, OnFoodAmountTypeChange);
        }

        public bool Heavy
        {
            get { return _heavy; }
            set => SetProperty(ref _heavy, value, OnFoodAmountTypeChange);
        }

        public bool VeryHeavy
        {
            get { return _veryHeavy; }
            set => SetProperty(ref _veryHeavy, value, OnFoodAmountTypeChange);
        }

        public MealDataProxy Meal
        {
            get => _meal;
            set => SetProperty(ref _meal, value, OnMealChange);
        }

        public bool OpenTimePicker
        {
            get => _openTimePicker;
            set => SetProperty(ref _openTimePicker, value);
        }

        public FastingDataMessageProxy FastingData
        {
            get => _fastingData;
            set => SetProperty(ref _fastingData, value);
        }

        private IEducationalInfoProvider educationalInfoProvider;

        public MealsSnacksBreakfastPageViewModel(IVmCommonService service, IMealsSnacksBreakfastPageVmProvider provider,
            ILoggerFactory loggerFactory, IApiService apiService, IEducationalInfoProvider _educationalInfoProvider) : base(service, provider, loggerFactory, apiService)
        {
            educationalInfoProvider = _educationalInfoProvider;
            ShowTimePickerCommand = new DelegateCommand(() => OpenTimePicker = !OpenTimePicker);
            SaveCommand = new DelegateCommand(SaveExecute);
            FastingData = new EmptyFastingDataMessageProxy();
            MealsSnacksImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Meal_Breakfast.png");
        }

        private ImageSource _breakfastImageSource;

        public ImageSource MealsSnacksImageSource
        {
            get => _breakfastImageSource;
            set => SetProperty(ref _breakfastImageSource, value);
        }

        private async void SaveExecute()
        {
            try
            {
                if (!HasInternetConnection)
                {
                    return;
                }

                IsBusy = true;
                await ValidateToken();
                _cts = new CancellationTokenSource();

                Meal = MealsDataIngredientsHelper.GatherIngredients(Meal, Meal.HasIngredientGrains, Meal.HasIngredientVegetables,
                    Meal.HasIngredientFruits, Meal.HasIngredientDairy, Meal.HasIngredientProtein);

                if (Meal?.UpdateNeeded == true)
                {
                    var apiCallResult = await Provider.UpdateBreakfastAsync(Meal, _cts.Token);

                    if (!apiCallResult.Success)
                    {
                        if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                            apiCallResult.ErrorReason == ErrorReason.TimeOut)
                        {
                            IsBusy = false;
                            return;
                        }

                        IsBusy = false;
                        await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error,
                            apiCallResult.ErrorMessage,
                            AppResources.Button_Ok);
                    }
                    else
                    {
                        await Service.NavigationService.GoBackAsync();
                    }
                }
                else
                {
                    var apiCallResult = await Provider.AddBreakfastAsync(Meal, _cts.Token);

                    if (!apiCallResult.Success)
                    {
                        if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                            apiCallResult.ErrorReason == ErrorReason.TimeOut)
                        {
                            IsBusy = false;
                            return;
                        }

                        IsBusy = false;
                        await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error,
                            apiCallResult.ErrorMessage,
                            AppResources.Button_Ok);
                    }
                    else
                    {
                        await Service.NavigationService.GoBackAsync();
                    }
                }
            }
            catch (Exception e)
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, e.Message,
                    AppResources.Button_Ok);
            }
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            if (!HasInternetConnection)
            {
                return;
            }

            ShowProgress = true;
            //  await ValidateToken();

            _cts = new CancellationTokenSource();
            _navParams = NavigationParametersHelper.GetNavigationParameters<MealNavParams>(parameters);

            var showOverlay = Service.HoldingPagesSettingsService.ShouldDailyShow(HoldingPageType.Breakfast);
            if (showOverlay)
            {
                IsLoadingImageBusy = true;
                OpenOverlay();
                educationalInfoProvider.GetEducationalInfoByIdAsync(_cts.Token, StorageFolderConstants.Breakfast).ContinueWith(async apiCallEducation =>
                {
                    if (apiCallEducation.Result.Success && apiCallEducation.Result.Value != null)
                    {
                        var educationResult = apiCallEducation.Result;
                        ProductImage = new UriImageSource { Uri = new Uri(educationResult.Value.ImageUrl) };
                    }
                    IsLoadingImageBusy = false;
                });
            }
            var filterMealDataDto = new FilterMealDataDto
            { DateTime = DateTime.Today, SlotType = MealSlotType.Breakfast, WithIngredients = true };

            Provider.GetMealDataWithFilterAsync(filterMealDataDto, _cts.Token).ContinueWith(result =>
            {
                Provider.GetPersonalGoalAsync(_cts.Token).ContinueWith(async apiCallResult =>
                {
                    if (result.Result.Success && apiCallResult.Result.Success)
                    {
                        var mealExist = result.Result.Value.FirstOrDefault(m => m.MealSlotType == MealSlotType.Breakfast);
                        if (mealExist != null)
                        {
                            Meal = mealExist;
                            Meal.UpdateIngredients();
                            Meal.UpdateNeeded = true;
                        }
                        else
                        {
                            Meal = UpdateDefaultMealData(apiCallResult.Result.Value, MealSlotType.Breakfast);
                        }

                        var fastingDataResult = await Provider.GetActiveFastingDataAsync(Meal.CreateDateTime, _cts.Token);
                        if (fastingDataResult.Success)
                        {
                            ShowProgress = false;
                            FastingData = new FastingDataMessageProxy(fastingDataResult.Value,
                                     AppResources.MealsAndSnacks_FastingActive, Meal.CreateDateTime);
                            if (!string.IsNullOrEmpty(FastingData.Message))
                            {
                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await Service.DialogService.DisplayAlertAsync(FastingData.Message, result.Result.ErrorMessage, AppResources.Button_Ok);
                                });
                            }
                        }
                        ShowProgress = false;
                    }
                    else
                    {
                        ShowProgress = false;
                        if (result.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                                 result.Result.ErrorReason == ErrorReason.TimeOut)
                        {
                            return;
                        }
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, result.Result.ErrorMessage,
                                 AppResources.Button_Ok);
                        });
                    }
                });
            });
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            CancelPendingRequest(_cts);
        }

        protected override void OnCustomCloseOverlay()
        {
            base.OnCustomCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowAgain(HoldingPageType.Breakfast);
        }

        internal override void OnDailyCloseOverlay()
        {
            base.OnDailyCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowToday(HoldingPageType.Breakfast);
        }

        private MealDataProxy UpdateDefaultMealData(PersonalGoalProxy personalGoalProxy, MealSlotType mealSlotType)
        {
            var meal = new MealDataProxy(mealSlotType);
            var localTime = DateTimeHelper.ToLocalTime(DateTime.Now);
            var localDate = DateTimeHelper.ToLocalCurrentDate(DateTime.Now);
            Title = DateTimeHelper.ToLocalDateZeroGravityFormat(DateTime.Now);
            meal.CreateDateTime = localDate;
            meal.Time = new TimeSpan(localTime.Hour, localTime.Minute, localTime.Second);
            switch (mealSlotType)
            {
                case MealSlotType.Breakfast:
                    meal.FoodAmount = personalGoalProxy.BreakfastAmount;
                    break;

                case MealSlotType.Lunch:
                    meal.FoodAmount = personalGoalProxy.LunchAmount;
                    break;

                case MealSlotType.Dinner:
                    meal.FoodAmount = personalGoalProxy.DinnerAmount;
                    break;

                case MealSlotType.HealthySnack:
                    meal.FoodAmount = personalGoalProxy.HealthySnackAmount;
                    break;

                case MealSlotType.UnhealthySnack:
                    meal.FoodAmount = personalGoalProxy.UnhealthySnackAmount;
                    break;
            }
            meal.HasIngredientDairy = true;
            meal.HasIngredientFruits = true;
            meal.HasIngredientProtein = true;
            meal.HasIngredientVegetables = true;
            meal.HasIngredientGrains = true;
            meal.AccountId = personalGoalProxy.AccountId;

            return meal;
        }

        private void OnFoodAmountTypeChange()
        {
            if (Meal == null) return;
            if (None)
            {
                Meal.FoodAmount = 0;
                Meal.UpdateIngredientStatus(false);
            }
            else if (VeryLight)
            {
                Meal.FoodAmount = 1;
                Meal.UpdateIngredientStatus(true);
            }
            else if (Light)
            {
                Meal.FoodAmount = 2;
                Meal.UpdateIngredientStatus(true);
            }
            else if (Medium)
            {
                Meal.FoodAmount = 3;
                Meal.UpdateIngredientStatus(true);
            }
            else if (Heavy)
            {
                Meal.FoodAmount = 4;
                Meal.UpdateIngredientStatus(true);
            }
            else if (VeryHeavy)
            {
                Meal.FoodAmount = 5;
                Meal.UpdateIngredientStatus(true);
            }
                
        }

        private void OnMealChange()
        {
            if (Meal == null) return;
            switch (Meal.FoodAmount)
            {
                case 0:
                    None = true;
                    break;

                case 1:
                    VeryLight = true;
                    break;

                case 2:
                    Light = true;
                    break;

                case 3:
                    Medium = true;
                    break;

                case 4:
                    Heavy = true;
                    break;

                case 5:
                    VeryHeavy = true;
                    break;

                default:
                    break;
            }
        }
    }
}