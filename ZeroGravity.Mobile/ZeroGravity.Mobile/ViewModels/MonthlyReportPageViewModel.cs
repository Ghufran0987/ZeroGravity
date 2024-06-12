using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.ViewModels
{
    public class MonthlyReportPageViewModel : VmBase<IMonthlyReportPage, IMonthlyReportPageVmProvider, MonthlyReportPageViewModel>
    {
        private IPersonalDataPageVmProvider _personalDataProvider;
        private readonly IEventAggregator _eventAggregator;
        private readonly ILogger _logger;
        private readonly ISugarBeatSettingsSevice _settingsService;

        private CancellationTokenSource _cts = new CancellationTokenSource();
        private ProgressProxy _reportData;
        private KeyValuePair<DateTime, ProgressProxy> _selectedMonth;
        private ObservableCollection<KeyValuePair<DateTime, ProgressProxy>> _months;
        private ApiCallResult<AccountResponseDto> AccountResponse;
        private bool _isVisible;
        private bool canExcuteLetgo;
        private string _displayName;
        private bool _isMetascoreDeviceActive;

        public MonthlyReportPageViewModel(IVmCommonService service, IMonthlyReportPageVmProvider provider, IPersonalDataPageVmProvider personalDataProvider,
                 ILoggerFactory loggerFactory, IEventAggregator eventAggregator, ISugarBeatSettingsSevice settingsService) : base(service, provider, loggerFactory)
        {
            _personalDataProvider = personalDataProvider;
            _eventAggregator = eventAggregator;
            _settingsService = settingsService;
            _logger = loggerFactory?.CreateLogger<MonthlyReportPageViewModel>() ??
                      new NullLogger<MonthlyReportPageViewModel>();
            canExcuteLetgo = true;
            SelectedMonth = new KeyValuePair<DateTime, ProgressProxy>(DateTime.Now.AddMonths(-1), null);
            LetsGoCommand = new DelegateCommand(OnLetsGo, () => { return canExcuteLetgo; });
            BlogCommand = new DelegateCommand(OnBlogSubmit);
            UpdateQuestion = new DelegateCommand<object>(OnUpdateQuestion);
            ShowProgress = true;

        }

        private void OnUpdateQuestion(object obj)
        {
            if (PersonalDataProxy != null)
            {
                PersonalDataProxy.QuestionAnswers.Where(x => x.QuestionId == 13).Select(y => { y.Value = obj.ToString(); return y; }).ToList();
                _cts = new CancellationTokenSource();
                _personalDataProvider.UpdatePersonalDataAsnyc(PersonalDataProxy, _cts.Token);
            }
        }

        public DelegateCommand BlogCommand { get; }

        public DelegateCommand<object> UpdateQuestion { get; }

        public ProgressProxy ReportData
        {
            get => _reportData;
            set => SetProperty(ref _reportData, value);
        }

        public string DisplayName
        {
            get => _displayName;
            set => SetProperty(ref _displayName, value);
        }

        public bool IsVisible
        {
            get => _isVisible;
            set => SetProperty(ref _isVisible, value);
        }

        public KeyValuePair<DateTime, ProgressProxy> SelectedMonth
        {
            get => _selectedMonth;
            set
            {
                SetProperty(ref _selectedMonth, value);
                ReportData = _selectedMonth.Value;
            }
        }

        public ObservableCollection<KeyValuePair<DateTime, ProgressProxy>> Months
        {
            get => _months;
            set => SetProperty(ref _months, value);
        }

        public async override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            IsVisible = false;
            await GetAccountDetails();
            if (AccountResponse?.Value != null) { ConstructMonths(); }

            GetPersonalDataAsync();
        }

        private async Task GetAccountDetails()
        {
            IsBusy = true;
            var MetascoreDeviceSetting = await _settingsService.GetByAccountIdAsync(_cts.Token);
            AccountResponse = await Provider.GetAccountDetailsAsync(_cts.Token);

            if (MetascoreDeviceSetting != null && MetascoreDeviceSetting.Value != null)
            {
                _isMetascoreDeviceActive = true;
            }
            else
            {
                _isMetascoreDeviceActive = false;
            }

            if (AccountResponse == null)
            {
                await Service.DialogService.DisplayAlertAsync("Monthly Report",
           "Unable to Fetch data. Please try again.",
           AppResources.Button_Ok);
                IsBusy = false;
                return;
            }

            if (AccountResponse.Value == null)
            {
                await Service.DialogService.DisplayAlertAsync("Monthly Report",
           "Unable to Fetch data. Please try again.",
           AppResources.Button_Ok);
                IsBusy = false;
                return;
            }

            DisplayName = AccountResponse.Value.FirstName;

            if (AccountResponse.Value.OnboardingDate == null)
            {
                await Service.DialogService.DisplayAlertAsync("Monthly Report",
            "Your progress report will be available on-demand from 7 days onwards app usage.",
            AppResources.Button_Ok);
                IsBusy = false;
                return;
            }

            if (DateTime.Now <= AccountResponse?.Value?.OnboardingDate.Value.AddDays(7))
            {
                await Service.DialogService.DisplayAlertAsync("Monthly Report",
                "Your progress report will be available on-demand from 7 days onwards app usage.",
                AppResources.Button_Ok);
                IsBusy = false;
                return;
            }
        }

        public DelegateCommand LetsGoCommand { get; }

        private async void OnLetsGo()
        {
            //ShowProgress = true;
            //if (AccountResponse == null || AccountResponse?.Value == null)
            //{
            //    GetAccountDetails();
            //}

            if (AccountResponse?.Value?.OnboardingDate.HasValue == true)
            {
                // allow report after 7 days from on-boarding date
                if (DateTime.Now <= AccountResponse?.Value?.OnboardingDate.Value.AddDays(7))
                {
                    await Service.DialogService.DisplayAlertAsync("Monthly Report",
                        "Your progress report will be available on-demand from 7 days onwards app usage.",
                        AppResources.Button_Ok);

                    return;
                }

                await ConstructMonths();
                if (ReportData != null) {   
                    IsVisible = true;
                    return;
                }
                else
                {
                    await Service.DialogService.DisplayAlertAsync("Monthly Report",
                     "Your progress report will be available on-demand from 7 days onwards app usage.",
                     AppResources.Button_Ok);
                    return;
                }
            }

            await Service.DialogService.DisplayAlertAsync("Monthly Report",
               "Your progress report will be available on-demand from 7 days onwards app usage.",
               AppResources.Button_Ok);

            return;
        }

        private async Task ConstructMonths()
        {
            this.ShowProgress = true;
            try
            {
                var now = DateTime.Now;
                var months = Enumerable.Range(1, 6).Select(i => now.AddMonths(-i)).ToList().OrderBy(x => x);
                Months = new ObservableCollection<KeyValuePair<DateTime, ProgressProxy>>();
                foreach (var m in months)
                {
                    if (AccountResponse?.Value?.OnboardingDate != null)
                    {
                        if (m > AccountResponse.Value.OnboardingDate)
                        {
                            var proxy = await GetMonthData(m);
                            Months.Add(new KeyValuePair<DateTime, ProgressProxy>(m, proxy));
                        }
                    }
                }
                if (Months.Count > 0)
                {
                    SelectedMonth = Months[Months.Count - 1];
                }
            }
            catch (Exception ex)
            {
            }
            this.ShowProgress = false;
        }

        private async Task<ProgressProxy> GetMonthData(DateTime dateTime)
        {
            try
            {
                if (dateTime == DateTime.MinValue)
                {
                    return null;
                }

                var fromDate = dateTime.StartOfMonth();
                var toDate = dateTime.EndOfMonth();

                var result = await this.Provider.GetProgressDataByDateAsync(fromDate, toDate, _isMetascoreDeviceActive, _cts.Token);
                if (result != null)
                {
                    return result.Value;
                }
            }

            catch (Exception ex)
            {
            }
            return null;
        }

        //private async void GetData()
        //{
        //    try
        //    {
        //        if (SelectedMonth.Key == DateTime.MinValue)
        //        {
        //            ShowProgress = false;
        //            await Service.DialogService.DisplayAlertAsync("Monthly Report",
        //                   "You can view the monthly report once the month is elapsed.",
        //                   AppResources.Button_Ok);
        //            return;
        //        }

        //        if (SelectedMonth.Key.Month == DateTime.Now.Month)
        //        {
        //            ShowProgress = false;
        //            await Service.DialogService.DisplayAlertAsync("Monthly Report",
        //                   "You can view the monthly report once the month is elapsed.",
        //                   AppResources.Button_Ok);
        //        }
        //        else
        //        {
        //            var fromDate = SelectedMonth.Key.StartOfMonth();
        //            var toDate = SelectedMonth.Key.EndOfMonth();

        //            var result = await this.Provider.GetProgressDataByDateAsync(fromDate, toDate, _cts.Token);
        //            Device.BeginInvokeOnMainThread(() =>
        //            {
        //                try
        //                {
        //                    ReportData = result.Value;
        //                    ShowProgress = false;
        //                    IsVisible = true;
        //                }
        //                catch (Exception)
        //                {
        //                }
        //            });
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //}

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        private async void OnBlogSubmit()
        {
            try
            {
                await Browser.OpenAsync("https://miboko.com", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        public PersonalDataProxy PersonalDataProxy = new PersonalDataProxy();
        private async Task GetPersonalDataAsync()
        {
            try
            {
                _cts = new CancellationTokenSource();

                _personalDataProvider.GetPersonalDataAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
                {
                    try
                    {
                        if (apiCallResult.Result.Success)
                        {
                            if (apiCallResult.Result.Value != null)
                            {
                                Logger.LogInformation($"PersonalData for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");
                                PersonalDataProxy = apiCallResult.Result.Value;
                            }

                        }

                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in GetPersonalDataAsync " + ex.Message + " stacktarce: " + ex.StackTrace);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception  while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);

            }
        }
    }

    public static class DateTimeExtention
    {
        public static DateTime EndOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59, 999);
        }

        public static DateTime StartOfDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 0, 0, 0, 0);
        }

        public static DateTime EndOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0).AddMonths(1).AddTicks(-1);
        }

        public static DateTime StartOfMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1, 0, 0, 0, 0);
        }
    }
}