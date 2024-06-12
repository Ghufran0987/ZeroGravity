using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Syncfusion.ListView.XForms;
using Xamarin.Essentials;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.ViewModels
{
    public class MeditationAreaPageViewModel : VmBase<IMeditationAreaPage, IMeditationAreaPageVmProvider, MeditationAreaPageViewModel>
    {

        private TimeSpan _initialTime;
        private TimeSpan _timeElapsed;
        private StreamContentProxy _selectedMediaVideoItem;
        private List<StreamContentProxy> _streamContent;
        private ObservableCollection<StreamContentProxy> bookInfoCollection;
        private CancellationTokenSource _cts;
        private DateTime _actualDateTime;
        private double _videoHeight;
        private readonly IEventAggregator _eventAggregator;

        public DelegateCommand PlayerFrameCloseButton { get; }

        public Command<object> SelectedMediaVideoItemCommand { get; set; }



        public MeditationAreaPageViewModel(
            IVmCommonService service,
            IMeditationAreaPageVmProvider provider,
            ILoggerFactory loggerFactory,
            IApiService apiService,
            IEventAggregator eventAggregator,
            bool checkInternet = true) : base(service, provider, loggerFactory, apiService, checkInternet)
        {
            _initialTime = new TimeSpan();
            _timeElapsed = new TimeSpan();
            _eventAggregator = eventAggregator;
            var displayWidth = DeviceDisplay.MainDisplayInfo.Width;
            var displayHeight = DeviceDisplay.MainDisplayInfo.Height;

            var width = displayWidth / DeviceDisplay.MainDisplayInfo.Density;
            var height = (displayHeight / DeviceDisplay.MainDisplayInfo.Density) - 130; //subtract margin for navbar

            var videoHeight = width / 9 * 16;
            if (videoHeight > height)
            {
                videoHeight = height;
            }
            VideoHeight = videoHeight;

            MeditationImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Meditation.png");
            SelectedMediaVideoItemCommand = new Command<object>(SelectedMediaVideoItemExecute);
            PlayerFrameCloseButton = new DelegateCommand(PlayerFrameCloseButtonExecute);

            ShowWebView = false;
        }


        private void PlayerFrameCloseButtonExecute()
        {
            ShowWebView = !ShowWebView;
        }

        private async void SelectedMediaVideoItemExecute(object obj)
        {
            try
            {
                var itemEventArgs = obj as Syncfusion.ListView.XForms.ItemTappedEventArgs;
                var selectedStream = itemEventArgs.ItemData as StreamContentProxy;
                var mediaDetailEvent = new MeditataionDetailEvent
                {
                    DateTime = DateTime.Now,
                    PageName = ViewName.MediaElementViewPage,
                    SelectedMediaVideoItem = selectedStream,
                    StreamContent = StreamContent,
                    StreamContentType = Shared.Enums.StreamContentType.Meditation
                };
                _eventAggregator.GetEvent<MeditataionDetailEvent>().Publish(mediaDetailEvent);
            }
            catch (Exception e)
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, AppResources.Common_Error_Unknown, AppResources.Button_Ok);
            }
        }

        public ObservableCollection<StreamContentProxy> BookInfoCollection
        {
            get => bookInfoCollection;
            set => SetProperty(ref bookInfoCollection, value);
        }

        private bool _showWebView;

        public bool ShowWebView
        {
            get => _showWebView;
            set => SetProperty(ref _showWebView, value);
        }

        private ImageSource _meditationImageSource;

        public ImageSource MeditationImageSource
        {
            get => _meditationImageSource;
            set => SetProperty(ref _meditationImageSource, value);
        }

        public StreamContentProxy SelectedMediaVideoItem
        {
            get => _selectedMediaVideoItem;
            private set => SetProperty(ref _selectedMediaVideoItem, value);
        }

        public TimeSpan TimeElapsed
        {
            get => _timeElapsed;
            private set => SetProperty(ref _timeElapsed, value);
        }

        public List<StreamContentProxy> StreamContent
        {
            get => _streamContent;
            private set => SetProperty(ref _streamContent, value);
        }

        public double VideoHeight
        {
            get => _videoHeight;
            set => SetProperty(ref _videoHeight, value);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            IsBusy = true;
            base.OnNavigatedTo(parameters);

            RegisterForEvents();

            _cts = new CancellationTokenSource();

            var navParams = NavigationParametersHelper.GetNavigationParameters<PageNavigationParams>(parameters);
            if (navParams == null)
            {
                _actualDateTime = DateTime.Now;
            }
            else
            {
                _actualDateTime = navParams.DateTime;
                _actualDateTime = _actualDateTime.Add(DateTime.Now.TimeOfDay);
            }

            Title = DateTimeHelper.ToLocalDateZeroGravityFormat(_actualDateTime);
            Provider.GetAvailableStreamContentAsync(_cts.Token).ContinueWith(async result =>
            {
                if (result.Result.Success)
                {
                    var list = new List<StreamContentDto>(result.Result.Value);
                    StreamContent = GetStreamContentProxy(list);
                    BookInfoCollection = new ObservableCollection<StreamContentProxy>(StreamContent);
                    // TODO to check video server performance
                    ///bookInfoCollection[bookInfoCollection.Count - 1].VideoUrl = @"https://www.rmp-streaming.com/media/big-buck-bunny-360p.mp4";
                }

                CreateMeditationProxy(_actualDateTime);

                Provider.GetMeditationDurationForDateAsync(_actualDateTime, _cts.Token).ContinueWith(async apiCallResult =>
                {
                    if (apiCallResult.Result.Success)
                    {
                        _initialTime = apiCallResult.Result.Value;
                    }
                    else
                    {
                        IsBusy = false;

                        if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                        {
                            return;
                        }

                        _initialTime = new TimeSpan();

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(AppResources.MeditationArea_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                        });
                    }

                    TimeElapsed = new TimeSpan(_initialTime.Ticks);

                    IsBusy = false;

                });
            });
        }

        protected override void OnCustomCloseOverlay()
        {
            base.OnCustomCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowAgain(HoldingPageType.Meditation);
        }

        private void CreateMeditationProxy(DateTime dateTime)
        {
            MeditationData = new MeditationDataProxy
            {
                Duration = new TimeSpan(),
                CreateDateTime = dateTime
            };
        }

        public override async void OnNavigatedFrom(INavigationParameters parameters)
        {
            CancelPendingRequest(_cts);

            base.OnNavigatedFrom(parameters);
        }

        public override void OnResume()
        {
            base.OnResume();
            Task.Run(async () =>
            {
                TimeElapsed = await Provider.LoadTimeFromLocalStorageAsync();
            }).Wait(); //on resume will not wait for completion, so synchronous call needed
                       // StartTimer();
        }

        public override void OnSleep()
        {
            base.OnSleep();
        }

        private MeditationDataProxy _meditationData;

        public MeditationDataProxy MeditationData
        {
            get { return _meditationData; }
            set { SetProperty(ref _meditationData, value); }
        }


        private void RegisterForEvents()
        {
            DeRegisterFromEvents();
            _eventAggregator.GetEvent<MediaDetailEvent>().Subscribe(OnMediaDetailEvent);
        }

        private void DeRegisterFromEvents()
        {
            _eventAggregator.GetEvent<MediaDetailEvent>().Unsubscribe(OnMediaDetailEvent);
        }


        private async void OnMediaDetailEvent(MediaDetailEvent mediaDetailEvent)
        {
            MeditationData.Duration = new TimeSpan(mediaDetailEvent.MediaPlayTime.Hours, mediaDetailEvent.MediaPlayTime.Minutes, mediaDetailEvent.MediaPlayTime.Seconds);
            MeditationData.CreateDateTime = DateTime.Now;
            await Provider.SaveMeditationDataAsync(MeditationData);
        }

        private List<StreamContentProxy> GetStreamContentProxy(List<StreamContentDto> streamContentDtos)
        {
            var result = new List<StreamContentProxy>();

            foreach (var item in streamContentDtos)
            {
                var sContentProxy = new StreamContentProxy();
                sContentProxy.Id = item.Id;
                sContentProxy.Description = item.Description;
                sContentProxy.Categories = item.Categories;
                sContentProxy.ThumbnailUrl = item.ThumbnailUrl;
                sContentProxy.Title = item.Title;
                sContentProxy.VideoId = item.VideoId;
                sContentProxy.VideoPlayerId = item.VideoPlayerId;
                sContentProxy.VideoUrl = item.VideoUrl;
                result.Add(sContentProxy);
            }
            return result;
        }
    }
}