using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class CalorieDrinksAlcoholPageViewModel : VmBase<ICalorieDrinksAlcoholPage, ICalorieDrinksAlcoholPageVmProvider,
        CalorieDrinksAlcoholPageViewModel>
    {
        private CancellationTokenSource _cts;
        private FastingDataMessageProxy _fastingData;
        private LiquidIntakeDataProxy _liquidIntakeDataProxy;
        private IEducationalInfoProvider educationalInfoProvider;
        private string _liquidUnitDisplay;
        private LiquidIntakeNavParams _navParams;

        public CalorieDrinksAlcoholPageViewModel(IVmCommonService service, ICalorieDrinksAlcoholPageVmProvider provider,
            ILoggerFactory loggerFactory, IEducationalInfoProvider _educationalInfoProvider) : base(service, provider, loggerFactory)
        {
            SaveCommand = new Command(SaveExecute);
            educationalInfoProvider = _educationalInfoProvider;
            MinAmountMl = LiquidIntakeConstants.MinAmountMl;
            MaxAmountMl = LiquidIntakeConstants.MaxAmountMl;

            FastingData = new EmptyFastingDataMessageProxy();
            WaterImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.CalorieDrinks.png");
            WaterCupImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Glass.png");

            AddWaterAmount = new Command(AddWaterAmountExecute);
            RemoveWaterAmount = new Command(RemoveWaterAmountExecute);
        }

        public Command AddWaterAmount { get; }
        public Command RemoveWaterAmount { get; }
        private ImageSource _waterImageSource;

        public ImageSource WaterImageSource
        {
            get => _waterImageSource;
            set => SetProperty(ref _waterImageSource, value);
        }

        private ImageSource _waterCupImageSource;

        public ImageSource WaterCupImageSource
        {
            get => _waterCupImageSource;
            set => SetProperty(ref _waterCupImageSource, value);
        }

        public Command SaveCommand { get; }

        public double MinAmountMl { get; set; }

        public double MaxAmountMl { get; set; }

        public LiquidIntakeDataProxy LiquidIntakeDataProxy
        {
            get => _liquidIntakeDataProxy;
            set => SetProperty(ref _liquidIntakeDataProxy, value);
        }

        public FastingDataMessageProxy FastingData
        {
            get => _fastingData;
            set => SetProperty(ref _fastingData, value);
        }

        public string LiquidUnitDisplay
        {
            get => _liquidUnitDisplay;
            set => SetProperty(ref _liquidUnitDisplay, value);
        }

        private void SetDisplayUnits()
        {
            var displayPreferences = DisplayConversionService.GetDisplayPrefences();

            LiquidUnitDisplay = displayPreferences.UnitDisplayType == UnitDisplayType.Metric
                ? AppResources.Units_Milliliter
                : AppResources.Units_Ounce;
        }

        private async void RemoveWaterAmountExecute()
        {
            _liquidIntakeDataProxy.AmountMl -= 250;
            if (_liquidIntakeDataProxy.AmountMl < 250)
                _liquidIntakeDataProxy.AmountMl = 250;
        }

        private async void AddWaterAmountExecute()
        {
            if (_liquidIntakeDataProxy.AmountMl < (250 * 16))
                _liquidIntakeDataProxy.AmountMl += 250;
        }

        private async void SaveExecute()
        {
            if (!await ValidateToken()) return;

            try
            {
                LiquidIntakeDataProxy.CreateDateTime = new DateTime(LiquidIntakeDataProxy.CreateDateTime.Year,
                    LiquidIntakeDataProxy.CreateDateTime.Month, LiquidIntakeDataProxy.CreateDateTime.Day,
                    LiquidIntakeDataProxy.Time.Hours, LiquidIntakeDataProxy.Time.Minutes,
                    LiquidIntakeDataProxy.Time.Seconds);

                LiquidIntakeDataProxy.CreateDateTime =
                    DateTimeHelper.ToUniversalTime(LiquidIntakeDataProxy.CreateDateTime);

                var result = await Provider.CreateLiquidIntakeDataAsync(LiquidIntakeDataProxy, _cts.Token);

                if (!result.Success)
                {
                    if (result.ErrorReason == ErrorReason.TaskCancelledByUserOperation)
                    {
                        IsBusy = false;
                        return;
                    }

                    await Service.DialogService.DisplayAlertAsync("Error", result.ErrorMessage, "OK");
                }
                else
                {
                    await Service.NavigationService.GoBackAsync();
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                await Service.DialogService.DisplayAlertAsync("Error", e.Message, "OK");
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            LiquidIntakeDataProxy = new LiquidIntakeDataProxy(LiquidType.CalorieDrinkAndAlcohol);

            var liquidIntakeNavParams =
                NavigationParametersHelper.GetNavigationParameters<LiquidIntakeNavParams>(parameters);

            if (liquidIntakeNavParams != null)
            {
                _navParams = liquidIntakeNavParams;

                var localDate = DateTimeHelper.ToLocalCurrentDate(_navParams.DateTime);
                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(_navParams.DateTime);
                LiquidIntakeDataProxy.CreateDateTime = localDate;
                LiquidIntakeDataProxy.Time = DateTime.Now.TimeOfDay;

                SetDisplayUnits();

                Provider.GetActiveFastingDataAsync(localDate, _cts.Token).ContinueWith(fastingDataResult =>
                {
                    if (fastingDataResult.Result.Success)
                        FastingData = new FastingDataMessageProxy(fastingDataResult.Result.Value,
                            AppResources.CalorieDrinksAlcohol_FastingActive, localDate);
                });
            }

            var showOverlay = Service.HoldingPagesSettingsService.ShouldDailyShow(HoldingPageType.CaloriedDrink);
            if (showOverlay)
            {
                IsLoadingImageBusy = true;
                OpenOverlay();
                educationalInfoProvider.GetEducationalInfoByIdAsync(_cts.Token, StorageFolderConstants.CalorieAndDrink).ContinueWith(async apiCallEducation =>
                {
                    if (apiCallEducation.Result.Success && apiCallEducation.Result.Value != null)
                    {
                        var educationResult = apiCallEducation.Result;
                        ProductImage = new UriImageSource { Uri = new Uri(educationResult.Value.ImageUrl) };
                    }
                    IsLoadingImageBusy = false;
                });
            }
        }

        protected override void OnCustomCloseOverlay()
        {
            base.OnCustomCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowAgain(HoldingPageType.CaloriedDrink);
        }

        internal override void OnDailyCloseOverlay()
        {
            base.OnDailyCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowToday(HoldingPageType.CaloriedDrink);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }
    }
}