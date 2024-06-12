using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class CoachingDetailPageViewModel : VmBase<ICoachingDetailPage, ICoachingDetailPageVmProvider, CoachingDetailPageViewModel>
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly IEventAggregator _eventAggregator;
        private readonly CancellationTokenSource _cts;
        private CoachingType _coaching;
        private string _iconUnicode;
        private string _email;

        private ObservableCollection<string> _coachingOptions;
        private ObservableCollection<object> _coachingSelectedOptions;

        public CoachingDetailPageViewModel(IVmCommonService service,
            ICoachingDetailPageVmProvider provider,
            ILoggerFactory loggerFactory,
            IApiService apiService,
            ISecureStorageService secureStorageService,
            IEventAggregator eventAggregator,
            bool checkInternet = true) : base(service, provider, loggerFactory, apiService, checkInternet)
        {
            _secureStorageService = secureStorageService;
            _eventAggregator = eventAggregator;
            _cts = new CancellationTokenSource();
            _coachingOptions = new ObservableCollection<string>()
            {
                "Personal Trainer",
                "Dietitian",
                "Medical Advice",
                "On-line Chat",
            };
            _coachingSelectedOptions = new ObservableCollection<object>();
            SubmitInterestCommand = new DelegateCommand(OnSubmitInterest, CanSubmitInterest);
            BlogCommand = new DelegateCommand(OnBlogSubmit);
            PoadCastCommand = new DelegateCommand(OnPoadCastSubmit);
            CoursesCommand = new DelegateCommand(OnCoursesSubmit);
        }

        private async void OnCoursesSubmit()
        {
            await Service.DialogService.DisplayAlertAsync("Coaching", "Coming Soon", AppResources.Button_Ok);
            return;
        }

        private async void OnPoadCastSubmit()
        {
            try
            {
                // await Browser.OpenAsync("https://vimeo.com/613352159/29e0119f75", BrowserLaunchMode.SystemPreferred);
                var navigationParams = new PageNavigationParams
                {
                    DateTime = DateTime.Today,
                    PageName = ViewName.StreamingPage
                };

                _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private async void OnBlogSubmit()
        {
            try
            {
                await Browser.OpenAsync("https://miboko.com/blog/", BrowserLaunchMode.SystemPreferred);
            }
            catch (Exception ex)
            {
                // An unexpected error occured. No browser may be installed on the device.
            }
        }

        private bool CanSubmitInterest()
        {
            return !string.IsNullOrEmpty(Email);
        }

        public CoachingType Coaching
        {
            get => _coaching;
            set => SetProperty(ref _coaching, value);
        }

        public ObservableCollection<object> SelectedCoachingOptions
        {
            get => _coachingSelectedOptions;
            set => SetProperty(ref _coachingSelectedOptions, value);
        }

        public ObservableCollection<string> CoachingOptions
        {
            get => _coachingOptions;
            set => SetProperty(ref _coachingOptions, value);
        }

        public string IconUnicode
        {
            get => _iconUnicode;
            set => SetProperty(ref _iconUnicode, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public DelegateCommand SubmitInterestCommand { get; }
        public DelegateCommand BlogCommand { get; }
        public DelegateCommand PoadCastCommand { get; }
        public DelegateCommand CoursesCommand { get; }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            IsBusy = true;
            var navParams = NavigationParametersHelper.GetNavigationParameters<PageNavigationParams>(parameters);

            if (navParams is PageNavigationParams<CoachingType> pnp)
            {
                Coaching = pnp.Payload;
                if (_coaching == CoachingType.Nutrition)
                {
                    Title = AppResources.Coaching_Nutrition;
                    IconUnicode = "\uf2e7";
                }
                else if (_coaching == CoachingType.Personal)
                {
                    Title = AppResources.Coaching_Personal;
                    IconUnicode = "\uf460";
                }
                else
                {
                    Title = AppResources.Coaching_Mental;
                    IconUnicode = "\uf5dc";
                }
            }

            Email = await Provider.GetCurrentUserEmailAsync(_cts.Token);
            IsBusy = false;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(Email))
            {
                SubmitInterestCommand.RaiseCanExecuteChanged();
            }
        }

        private async void OnSubmitInterest()
        {
            if (_coachingSelectedOptions.Count == 0)
            {
                await Service.DialogService.DisplayAlertAsync("Coaching", "Please select atleast one option.", AppResources.Button_Ok);
                return;
            }
            IsBusy = true;
            var userId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);

            var list = new List<string>();
            foreach (var s in _coachingSelectedOptions)
            {
                list.Add(s.ToString());
            }
            var result = await Provider.SubmitInterestAsync(userId, Email, _coaching, list, _cts.Token);

            var message = result ? AppResources.Coaching_Success : AppResources.Coaching_Failure;
            await Service.DialogService.DisplayAlertAsync("Coaching", message, AppResources.Button_Ok);
            await Service.NavigationService.GoBackAsync();
            IsBusy = false;

            //var navParams = new ResultNavParams
            //{
            //    Result = result,
            //    Message = message
            //};

            //var pageNavigationParams = new PageNavigationParams<ResultNavParams>
            //{
            //    PageName = ViewName.ResultPage,
            //    Payload = navParams
            //};
            //_eventAggregator.GetEvent<PageNavigationEvent>().Publish(pageNavigationParams);
        }
    }
}