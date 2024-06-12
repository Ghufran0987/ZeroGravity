using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.ViewModels
{
    public class AnalysisPageViewModel : VmBase<IAnalysisPage, IAnalysisPageVmProvider, AnalysisPageViewModel>
    {
        private CancellationTokenSource _cts;

        private ObservableCollection<AnalysisItemProxy> _analysisItemSource;

        public ObservableCollection<AnalysisItemProxy> AnalysisItemSource
        {
            get { return _analysisItemSource; }
            set { SetProperty(ref _analysisItemSource, value); }
        }

        private ProgressProxy progressProxyModel;

        public ProgressProxy ProgressProxyModel
        {
            get { return progressProxyModel; }
            set { SetProperty(ref progressProxyModel, value); }
        }

        private bool _trackedToday = true;

        public bool TrackedToday
        {
            get { return _trackedToday; }
            set { SetProperty(ref _trackedToday, value, onTrackedTodayChange); }
        }

        private void onTrackedTodayChange()
        {
            if (TrackedToday)
            {
                GetProgressDataByperiodAsync("Today");
            }
        }

        private bool _tracked7day;

        public bool Tracked7day
        {
            get { return _tracked7day; }
            set { SetProperty(ref _tracked7day, value, onTracked7DayChange); }
        }

        private void onTracked7DayChange()
        {
            if (Tracked7day)
            {
                GetProgressDataByperiodAsync("Last7Day");
            }
        }

        private bool _tracked30day;

        public bool Tracked30day
        {
            get { return _tracked30day; }
            set { SetProperty(ref _tracked30day, value, onTracked30DayChange); }
        }

        private void onTracked30DayChange()
        {
            if (Tracked7day)
            {
                GetProgressDataByperiodAsync("Last30Day");
            }
        }

        public AnalysisPageViewModel(IVmCommonService service, IAnalysisPageVmProvider provider,
            ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            ProgressProxyModel = new ProgressProxy();

            Task task = Task.Run(async () => await InitAnalysisPage());
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            InitAnalysisPage();
        }

        private async Task InitAnalysisPage()
        {
            GetProgressDataByperiodAsync("Today");
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            CancelPendingRequest(_cts);
        }

        private async Task GetProgressDataByperiodAsync(string progressType)
        {
            IsBusy = true;
            var dateTime = DateTime.Now;
            Title = DateTimeHelper.GetDateWithSuffix(dateTime);
            _cts = new CancellationTokenSource();

            Provider.GetProgressDataByDateAsync(dateTime, _cts.Token, progressType).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                    apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                if (apiCallResult.Result.Success)
                {
                    ProgressProxyModel = apiCallResult.Result.Value;

                    IsBusy = false;
                }
                else
                {
                    IsBusy = false;
                    //Device.BeginInvokeOnMainThread(async () =>
                    //{
                    //    await Service.DialogService.DisplayAlertAsync(AppResources.AnalysisPage_SubTitle,
                    //    apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                    //});
                }
            });
        }
    }
}