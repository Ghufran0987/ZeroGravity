using System;
using System.Threading;
using System.Threading.Tasks;
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
    public class WaterIntakePageViewModel : VmBase<IWaterIntakePage, IWaterIntakePageVmProvider, WaterIntakePageViewModel>
    {
        private CancellationTokenSource _cts;
        private LiquidIntakeDataProxy _liquidIntakeDataProxy;
        private IEducationalInfoProvider educationalInfoProvider;

        private string _liquidUnitDisplay;
        private LiquidIntakeNavParams _navParams;

        private double _summary;
        private string _summaryText;

        public WaterIntakePageViewModel(IVmCommonService service, IWaterIntakePageVmProvider provider,
            ILoggerFactory loggerFactory, IEducationalInfoProvider _educationalInfoProvider) : base(service, provider, loggerFactory)
        {
            educationalInfoProvider = _educationalInfoProvider;
            SaveCommand = new Command(SaveExecute);
            AddWaterAmount = new Command(AddWaterAmountExecute);
            RemoveWaterAmount = new Command(RemoveWaterAmountExecute);

            MinAmountMl = LiquidIntakeConstants.MinAmountMl;
            MaxAmountMl = LiquidIntakeConstants.MaxAmountMl;
            WaterImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Water.png");
            WaterCupImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Glass.png");
        }

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

        public double MinAmountMl { get; set; }

        public double MaxAmountMl { get; set; }

        public Command SaveCommand { get; }
        public Command AddWaterAmount { get; }
        public Command RemoveWaterAmount { get; }

        public LiquidIntakeDataProxy LiquidIntakeDataProxy
        {
            get => _liquidIntakeDataProxy;
            set => SetProperty(ref _liquidIntakeDataProxy, value);
        }

        public double Summary
        {
            get => _summary;
            set => SetProperty(ref _summary, value);
        }

        public string LiquidUnitDisplay
        {
            get => _liquidUnitDisplay;
            set => SetProperty(ref _liquidUnitDisplay, value);
        }

        public string SummaryText
        {
            get => _summaryText;
            set => SetProperty(ref _summaryText, value);
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
            try
            {
                IsBusy = true;
                if (!await ValidateToken()) return;

                _cts = new CancellationTokenSource();
                LiquidIntakeDataProxy.CreateDateTime = new DateTime(LiquidIntakeDataProxy.CreateDateTime.Year,
                    LiquidIntakeDataProxy.CreateDateTime.Month, LiquidIntakeDataProxy.CreateDateTime.Day,
                    LiquidIntakeDataProxy.Time.Hours, LiquidIntakeDataProxy.Time.Minutes,
                    LiquidIntakeDataProxy.Time.Seconds);

                var apiCallResult = await Provider.CreateLiquidIntakeDataAsnyc(LiquidIntakeDataProxy, _cts.Token);

                if (apiCallResult.Success)
                {
                    //await CreateSummary(DateTime.Now);

                    await Service.NavigationService.GoBackAsync();
                }
                else
                {
                    if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                        apiCallResult.ErrorReason == ErrorReason.TimeOut)
                        return;

                    IsBusy = false;
                    await Service.DialogService.DisplayAlertAsync(AppResources.WaterIntake_Title,
                        apiCallResult.ErrorMessage,
                        AppResources.Button_Ok);
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, e.Message,
                    AppResources.Button_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                base.OnNavigatedTo(parameters);
                //if (!await ValidateToken()) return;
                IsBusy = true;
                _cts = new CancellationTokenSource();

                LiquidIntakeDataProxy = new LiquidIntakeDataProxy(LiquidType.Water);

                var liquidIntakeNavParams =
                    NavigationParametersHelper.GetNavigationParameters<LiquidIntakeNavParams>(parameters);

                if (liquidIntakeNavParams != null)
                {
                    _navParams = liquidIntakeNavParams;

                    var localDate = DateTimeHelper.ToLocalCurrentDate(_navParams.DateTime);
                    //Title = DateTimeHelper.ToLocalDateZeroGravityFormat(_navParams.DateTime);
                    LiquidIntakeDataProxy.CreateDateTime = localDate;
                    LiquidIntakeDataProxy.Time = DateTime.Now.TimeOfDay;
                    CreateSummary(DateTime.Now);

                    SetDisplayUnits();
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, e.Message,
                    AppResources.Button_Ok);
            }
            finally
            {
                //IsBusy = false;
            }

            var showOverlay = Service.HoldingPagesSettingsService.ShouldDailyShow(HoldingPageType.Water);
            if (showOverlay)
            {
                IsLoadingImageBusy = true;
                OpenOverlay();
                educationalInfoProvider.GetEducationalInfoByIdAsync(_cts.Token, StorageFolderConstants.Water).ContinueWith(async apiCallEducation =>
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
            Service.HoldingPagesSettingsService.DoNotShowAgain(HoldingPageType.Water);
        }

        internal override void OnDailyCloseOverlay()
        {
            base.OnDailyCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowToday(HoldingPageType.Water);
        }

        private async Task CreateSummary(DateTime dateTime)
        {
            Provider.GetLiquidIntakeDataSummaryAsync(dateTime, LiquidType.Water, _cts.Token).ContinueWith(async result =>
             {
                 IsBusy = false;
                 if (result.Result.Success)
                 {
                     Summary = result.Result.Value;
                     //  SummaryText = string.Format("{0}{1}", Summary / 1000, LiquidUnitDisplay);
                    
                     SummaryText = (Summary/250) .ToString();
                     return;
                 }

                 if (result.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                     result.Result.ErrorReason == ErrorReason.TimeOut)
                     return;

                 Device.BeginInvokeOnMainThread(async () =>
                 {
                     await Service.DialogService.DisplayAlertAsync(AppResources.WaterIntake_Title, result.Result.ErrorMessage,
                     AppResources.Button_Ok);
                 });
             });
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            CancelPendingRequest(_cts);
        }
    }
}