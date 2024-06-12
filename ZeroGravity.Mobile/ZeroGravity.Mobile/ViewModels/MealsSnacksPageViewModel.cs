using System;
using System.Threading;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    public class
        MealsSnacksPageViewModel : VmBase<IMealsSnacksPage, IMealsSnacksPageVmProvider, MealsSnacksPageViewModel>
    {
        private CancellationTokenSource _cts;
        private MealNavParams _mealNavParams;

        private MealsBadgeInformationProxy _mealsBadgeInformationProxy;

        public MealsSnacksPageViewModel(IVmCommonService service, IMealsSnacksPageVmProvider provider,
            ILoggerFactory loggerFactory, IApiService apiService) : base(service, provider, loggerFactory, apiService)
        {
            BreakfastCommand = new DelegateCommand(BreakfastExecute);
            LunchCommand = new DelegateCommand(LunchExecute);
            HealthySnackCommand = new DelegateCommand(HealthySnackExecute);
            UnhealthySnackCommand = new DelegateCommand(UnhealthySnackExecute);
            DinnerCommand = new DelegateCommand(DinnerExecute);
            MealsSnacksImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MealsSnacks.png");
        }

        private ImageSource _mealsSnacksImageSource;

        public ImageSource MealsSnacksImageSource
        {
            get => _mealsSnacksImageSource;
            set => SetProperty(ref _mealsSnacksImageSource, value);
        }

        public DelegateCommand BreakfastCommand { get; }
        public DelegateCommand LunchCommand { get; }
        public DelegateCommand HealthySnackCommand { get; }
        public DelegateCommand DinnerCommand { get; }
        public DelegateCommand UnhealthySnackCommand { get; }

        public MealsBadgeInformationProxy MealsBadgeInformationProxy
        {
            get => _mealsBadgeInformationProxy;
            set => SetProperty(ref _mealsBadgeInformationProxy, value);
        }

        private void UnhealthySnackExecute()
        {
            var navigationParameters =
                NavigationParametersHelper.CreateNavigationParameter(
                    new MealNavParams(DateTime.UtcNow));
            Service.NavigationService.NavigateAsync(ViewName.MealsSnacksUnhealthySnackPage, navigationParameters);
        }

        private async void DinnerExecute()
        {
            var navigationParameters =
                NavigationParametersHelper.CreateNavigationParameter(new MealNavParams(DateTime.Now));
            await Service.NavigationService.NavigateAsync(ViewName.MealsSnacksDinnerPage, navigationParameters);
        }

        private void HealthySnackExecute()
        {
            var navigationParameters =
                NavigationParametersHelper.CreateNavigationParameter(
                    new MealNavParams(DateTime.UtcNow));
            Service.NavigationService.NavigateAsync(ViewName.MealsSnacksHealthySnackPage, navigationParameters);
        }

        private async void LunchExecute()
        {
            var navigationParameters =
                NavigationParametersHelper.CreateNavigationParameter(
                    new MealNavParams(DateTime.Now));
            await Service.NavigationService.NavigateAsync(ViewName.MealsSnacksLunchPage, navigationParameters);
        }

        private async void BreakfastExecute()
        {
            var navigationParameters =
                NavigationParametersHelper.CreateNavigationParameter(
                    new MealNavParams(DateTime.Now));
            await Service.NavigationService.NavigateAsync(ViewName.MealsSnacksBreakfastPage, navigationParameters);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            //IsBusy = true;

            _cts = new CancellationTokenSource();

            var navparams = NavigationParametersHelper.GetNavigationParameters<MealNavParams>(parameters);

            if (navparams != null)
            {
                _mealNavParams = navparams;
                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(navparams.DateTime);

                //  var isAuthorized = await ValidateToken();
                //  if (isAuthorized)
                GetMealsBadgeInformationAsync(navparams.DateTime);
            }
            else if (_mealNavParams != null)
            {
                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(_mealNavParams.DateTime);

                //var isAuthorized = await ValidateToken();
                //if (isAuthorized)

                GetMealsBadgeInformationAsync(_mealNavParams.DateTime);
            }
            else
            {
                MealsBadgeInformationProxy = new MealsBadgeInformationProxy();
            }

            //IsBusy = false;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            CancelPendingRequest(_cts);
            //IsBusy = false;
        }

        private void GetMealsBadgeInformationAsync(DateTime dateTime)
        {
            Provider.GetMealsBadgeInformationAsnyc(dateTime, _cts.Token).ContinueWith(async apiCallResult =>
          {
              if (apiCallResult.Result.Success)
              {
                  MealsBadgeInformationProxy = apiCallResult.Result.Value;
              }
              else
              {
                  if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                           apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                  {
                      return;
                  }

                  Device.BeginInvokeOnMainThread(async () =>
                  {
                      await Service.DialogService.DisplayAlertAsync(AppResources.MealsSnacks_Title,
                         apiCallResult.Result.ErrorMessage,
                         AppResources.Button_Ok);
                  });
              }
          });
        }
    }
}