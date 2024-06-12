using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.ViewModels
{
    public class FeedbackPageViewModel : VmBase<IFeedbackPage, IFeedbackPageVmProvider, FeedbackPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger _logger;

        private CancellationTokenSource _cts;

        private FeedbackSummaryProxy _feedbackSummaryProxy;

        private string _liquidUnitDisplay;
        private AccountResponseDto _accountDetails;
        private bool _showAdviceText;

        public FeedbackPageViewModel(IVmCommonService service, IFeedbackPageVmProvider provider,
            ILoggerFactory loggerFactory, IEventAggregator eventAggregator) : base(service, provider, loggerFactory)
        {
            _eventAggregator = eventAggregator;

            _logger = loggerFactory?.CreateLogger<FeedbackPageViewModel>() ??
                      new NullLogger<FeedbackPageViewModel>();

            _cts = new CancellationTokenSource();

            OpenBodyMeasurementPageCommand = new DelegateCommand(OpenBodyMeasurementPage);
            OpenNutritionConsultingDetailPageCommand = new DelegateCommand(OpenNutritionConsultingDetailPage);
            OpenPersonalTrainingDetailPageCommand = new DelegateCommand(OpenPersonalTrainingDetailPage);
            OpenMentalTrainingDetailPageCommand = new DelegateCommand(OpenMentalTrainingDetailPage);

            //SetDisplayUnits();
        }

        public DelegateCommand OpenBodyMeasurementPageCommand { get; set; }
        public DelegateCommand OpenNutritionConsultingDetailPageCommand { get; private set; }
        public DelegateCommand OpenPersonalTrainingDetailPageCommand { get; private set; }
        public DelegateCommand OpenMentalTrainingDetailPageCommand { get; private set; }

        public FeedbackSummaryProxy FeedbackSummaryProxy
        {
            get => _feedbackSummaryProxy;
            set => SetProperty(ref _feedbackSummaryProxy, value);
        }

        public AccountResponseDto AccountDetails
        {
            get => _accountDetails;
            set => SetProperty(ref _accountDetails, value);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        public string LiquidUnitDisplay
        {
            get => _liquidUnitDisplay;
            set => SetProperty(ref _liquidUnitDisplay, value);
        }

        public bool ShowAdviceText
        {
            get => _showAdviceText;
            set => SetProperty(ref _showAdviceText, value);
        }

        private void SetDisplayUnits()
        {
            LiquidUnitDisplay = string.Empty;

            var displayPreferences = DisplayConversionService.GetDisplayPrefences();

            LiquidUnitDisplay = displayPreferences.UnitDisplayType == UnitDisplayType.Metric
                ? AppResources.Units_Liter
                : AppResources.Units_Ounce;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            var targetDate = DateTime.Now;

            //var isAuthorized = await ValidateToken();
            //if (isAuthorized) await

            GetFeedbackSummaryAsync(targetDate);

            GetAccountDetails();

            Title = DateTimeHelper.ToLocalDateZeroGravityFormat(targetDate);
        }

        private void GetAccountDetails()
        {
            IsBusy = true;
            Provider.GetAccountDetailsAsync(_cts.Token).ContinueWith(async apiCallResult =>
           {
               if (apiCallResult.Result.Success)
               {
                   Logger.LogInformation(
                       "AccountDetails successfully loaded.");

                   AccountDetails = apiCallResult.Result.Value;
                   if (AccountDetails?.OnboardingDate != null)
                   {
                       if (DateTime.Now > AccountDetails.OnboardingDate.Value.AddDays(1))
                       {
                           ShowAdviceText = true;
                       }
                       else
                       {
                           ShowAdviceText = false;
                       }
                   }
                   else
                   {
                       // Old account show advice
                       ShowAdviceText = true;
                   }
               }
               else
               {
                   if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                       apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                   {
                       IsBusy = false;
                       return;
                   }

                   Device.BeginInvokeOnMainThread(async () =>
                   {
                       await Service.DialogService.DisplayAlertAsync(AppResources.Feedback_Title,
                       apiCallResult.Result.ErrorMessage,
                       AppResources.Button_Ok);
                   });

                   IsBusy = false;
                   return;
               }
               IsBusy = false;
           });
        }

        private void GetFeedbackSummaryAsync(DateTime targetDateTime)
        {
            IsBusy = true;
            Provider.GetFeedbackSummaryAsync(targetDateTime, _cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    Logger.LogInformation(
                        "FeedbackSummary successfully loaded.");

                    FeedbackSummaryProxy = apiCallResult.Result.Value;

                    SetDisplayUnits();
                }
                else
                {
                    if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                        apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                    {
                        IsBusy = false;
                        return;
                    }

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Service.DialogService.DisplayAlertAsync(AppResources.Feedback_Title,
                        apiCallResult.Result.ErrorMessage,
                        AppResources.Button_Ok);
                    });

                    IsBusy = false;
                    return;
                }
                IsBusy = false;
            });
        }

        private void OpenBodyMeasurementPage()
        {
            var pageNavigationParams = new PageNavigationParams
            {
                DateTime = DateTime.Now,
                PageName = ViewName.Main
            };

            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(pageNavigationParams);
        }

        private void OpenNutritionConsultingDetailPage()
        {
            NavigateToConsultingDetailPage(CoachingType.Nutrition);
        }

        private void OpenPersonalTrainingDetailPage()
        {
            NavigateToConsultingDetailPage(CoachingType.Personal);
        }

        private void OpenMentalTrainingDetailPage()
        {
            NavigateToConsultingDetailPage(CoachingType.Mental);
        }

        private void NavigateToConsultingDetailPage(CoachingType type)
        {
            var pageNavigationParams = new PageNavigationParams<CoachingType>
            {
                PageName = ViewName.ConsultingDetailPage,
                Payload = type
            };
            _eventAggregator.GetEvent<PageNavigationEvent>().Publish(pageNavigationParams);
        }
    }
}