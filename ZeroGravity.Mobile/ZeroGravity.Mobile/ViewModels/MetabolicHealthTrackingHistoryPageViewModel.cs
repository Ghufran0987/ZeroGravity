using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    public class MetabolicHealthTrackingHistoryPageViewModel : VmBase<IMetabolicHealthTrackingHistoryPage, ISugarBeatEatingSessionProvider, MetabolicHealthTrackingHistoryPageViewModel>
    {

        private readonly ISugarBeatEatingSessionProvider _eatingSessionProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly ISecureStorageService _secureStorageService;
        private readonly ILoginPageVmProvider _loginPageVmProvider;
        private CancellationTokenSource _cts;
        private string _userName;
        private bool _isEnable;
        private DateTime _currentHistoryDateTime;
        private ObservableCollection<SugarBeatEatingSessionProxy> _todaysEatingSessions;
        private bool _isNoDataActive;
        public MetabolicHealthTrackingHistoryPageViewModel(IVmCommonService service, ISugarBeatEatingSessionProvider provider, ILoginPageVmProvider loginPageVmProvider, IEventAggregator eventAggregator, ISecureStorageService secureStorageService, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory, false)
        {
            _eventAggregator = eventAggregator;
            _eatingSessionProvider = provider;
            _secureStorageService = secureStorageService;
            _loginPageVmProvider = loginPageVmProvider;
            _cts = new CancellationTokenSource();
            PreviousHistoryDateCommand = new DelegateCommand(OnPreviousHistoryDate);
            NextHistoryDateCommand = new DelegateCommand(OnNextHistoryDate, CanNextHistoryDate);
        }

        public DateTime CurrentHistoryDateTime
        {
            get => _currentHistoryDateTime;
            set
            {
                if (_currentHistoryDateTime != value)
                {
                    SetProperty(ref _currentHistoryDateTime, value);
                    Common.MetabolicHealthTrackingHistorySelectedDate = value;
                }
            }
        }

        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }
        public bool IsEnable
        {
            get => _isEnable;
            set => SetProperty(ref _isEnable, value);
        }

        public bool IsNoDataActive
        {
            get => _isNoDataActive;
            set => SetProperty(ref _isNoDataActive, value);
        }
        public ObservableCollection<SugarBeatEatingSessionProxy> TodaysEatingSessions
        {
            get
            {
                if (_todaysEatingSessions == null)
                {
                    _todaysEatingSessions = new ObservableCollection<SugarBeatEatingSessionProxy>();
                }
                return _todaysEatingSessions;
            }
            set => SetProperty(ref _todaysEatingSessions, value);
        }

        private SugarBeatEatingSessionProxy _selectedEatingSession;

        public SugarBeatEatingSessionProxy SelectedEatingSession
        {
            get => _selectedEatingSession;
            set => SetProperty(ref _selectedEatingSession, value);
        }



        #region "Navigation"

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            UpdateTodayDate();
            Title = AppResources.SugarBeatConnect_Title;
            var userName = await _secureStorageService.LoadString(SecureStorageKey.DisplayName);
            UserName = "Hello " + userName;
            await GetMetabolicHealthHistory(DateTime.Parse("2021/01/01"), DateTime.Now);


            //_loginPageVmProvider.GetAccountDataAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
            //{
            //    if (apiCallResult.Result.Success)
            //    {
            //        Logger.LogInformation(
            //            "AccountDetails successfully loaded.");

            //      var  AccountDetails = apiCallResult.Result.Value;

            //    }
            //});

        }

        private async Task GetMetabolicHealthHistory(DateTime startdate, DateTime endDate)
        {
            try
            {
                IsBusy = true;
                IsEnable = false;
                var cts = new CancellationTokenSource().Token;

                var list = await _eatingSessionProvider.GetSugarBeatEatingSessionsAsync(startdate.StartOfDay(), endDate, cts);

                if (list != null)
                {
                    if (list.Value != null)
                    {
                        if (list.Value.ToList().Count > 0)
                        {
                            TodaysEatingSessions = new ObservableCollection<SugarBeatEatingSessionProxy>(list.Value.ToList());
                            IsNoDataActive = false;
                        }
                        else
                        {
                            TodaysEatingSessions = new ObservableCollection<SugarBeatEatingSessionProxy>();
                            IsNoDataActive = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
            finally
            {
                IsBusy = false;
                IsEnable = true;
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }
        #endregion

        #region "Command"

        public DelegateCommand PreviousHistoryDateCommand { get; set; }
        public DelegateCommand NextHistoryDateCommand { get; set; }

        #endregion

        #region "Method"
        private void UpdateTodayDate()
        {
            if (Common.MetabolicHealthTrackingHistorySelectedDate != null && Common.MetabolicHealthTrackingHistorySelectedDate.Date != new DateTime(1900, 1, 1))
            {
                CurrentHistoryDateTime = Common.MetabolicHealthTrackingHistorySelectedDate;
            }
            else
            {
                CurrentHistoryDateTime = DateTime.Today;
            }

            IsEnable = true;
        }
        private bool CanNextHistoryDate()
        {
            return CurrentHistoryDateTime.AddDays(1) <= DateTime.Today ? true : false;
        }
        private void OnNextHistoryDate()
        {
            if (CurrentHistoryDateTime.AddDays(1) <= DateTime.Today)
            {
                CurrentHistoryDateTime = CurrentHistoryDateTime.AddDays(1);
                //  Task.Run(async () => await GetMetabolicHealthHistory(CurrentHistoryDateTime));
            }

        }
        private void OnPreviousHistoryDate()
        {
            CurrentHistoryDateTime = CurrentHistoryDateTime.AddDays(-1);
            // Task.Run(async () => await GetMetabolicHealthHistory(CurrentHistoryDateTime));
        }
        #endregion

        public async void OnEatingSessionTappedCommand(SugarBeatEatingSessionProxy selectedSession)
        {
            try
            {

                var navParams = NavigationParametersHelper.CreateNavigationParameter(new SugarBeatMainPageNavParams
                { EatingSession = SelectedEatingSession, IsCurrentSession = false });

                //  Send Selected Eating Session
                await Service.NavigationService.NavigateAsync(ViewName.SugarBeatMainPage, navParams);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in sugar beat connect page viewmodel -> OnEatingSessionTappedCommand : " + ex.Message + " Stacktrace: " + ex.StackTrace);
                Logger.LogCritical(ex, ex.Message);
            }
        }
    }
}
