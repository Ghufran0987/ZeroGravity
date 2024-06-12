using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System.Linq;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models.Dto;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.ViewModels
{
    public class StreamingPageViewModel : VmBase<IStreamingPage, IStreamingPageVmProvider, StreamingPageViewModel>
    {
        private readonly ILogger _logger;
        private CancellationTokenSource _cts;
        private ObservableCollection<StreamContentProxy> bookInfoCollection;
        public Command<object> SelectedMediaVideoItemCommand { get; set; }
        private readonly IEventAggregator _eventAggregator;
        public DelegateCommand GoToVideoUploadPageCommand { get; }

        public StreamingPageViewModel(IVmCommonService service, IStreamingPageVmProvider provider,
            ILoggerFactory loggerFactory, IEventAggregator eventAggregator) : base(service, provider, loggerFactory)
        {
            _logger = loggerFactory?.CreateLogger<StreamingPageViewModel>() ??
                      new NullLogger<StreamingPageViewModel>();

            _eventAggregator = eventAggregator;
            StreamingImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Streaming.png");
            ButtonImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.add_plus.png");
            SelectedMediaVideoItemCommand = new Command<object>(SelectedMediaVideoItemExecute);
            GoToVideoUploadPageCommand = new DelegateCommand(GoToVideoUploadPageExecute);
        }

        public ObservableCollection<StreamContentProxy> BookInfoCollection
        {
            get => bookInfoCollection;
            set => SetProperty(ref bookInfoCollection, value);
        }

        private StreamContentProxy _selectedMediaVideoItem;

        public StreamContentProxy SelectedMediaVideoItem
        {
            get => _selectedMediaVideoItem;
            private set => SetProperty(ref _selectedMediaVideoItem, value);
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
                    StreamContent = BookInfoCollection.ToList(),
                    StreamContentType = Shared.Enums.StreamContentType.Stream
                };
                _eventAggregator.GetEvent<MeditataionDetailEvent>().Publish(mediaDetailEvent);
            }
            catch (Exception e)
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, AppResources.Common_Error_Unknown, AppResources.Button_Ok);
            }
        }

        private ImageSource _buttonImageSource;

        public ImageSource ButtonImageSource
        {
            get => _buttonImageSource;
            set => SetProperty(ref _buttonImageSource, value);
        }

        private ImageSource _streamingImageSource;

        public ImageSource StreamingImageSource
        {
            get => _streamingImageSource;
            set => SetProperty(ref _streamingImageSource, value);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            _cts = new CancellationTokenSource();
            IsBusy = true;

            //  var result = await
            Provider.GetAvailableStreamContentAsync(_cts.Token).ContinueWith(async result =>
            {
                if (result.Result.Success)
                {
                    var list = new List<StreamContentDto>(result.Result.Value);

                    BookInfoCollection = GetStreamContentProxy(list);
                }

                IsBusy = false;
            });
        }

        protected override void OnCustomCloseOverlay()
        {
            base.OnCustomCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowAgain(HoldingPageType.Streaming);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            CancelPendingRequest(_cts);
        }

        private ObservableCollection<StreamContentProxy> GetStreamContentProxy(List<StreamContentDto> streamContentDtos)
        {
            var result = new ObservableCollection<StreamContentProxy>();

            foreach (var item in streamContentDtos)
            {
                var sContentProxy = new StreamContentProxy
                {
                    Id = item.Id,
                    Description = item.Description,
                    Categories = item.Categories,
                    ThumbnailUrl = item.ThumbnailUrl,
                    Title = item.Title,
                    VideoId = item.VideoId,
                    VideoPlayerId = item.VideoPlayerId,
                    VideoUrl = item.VideoUrl
                };
                result.Add(sContentProxy);
            }
            return result;
        }

        private async void GoToVideoUploadPageExecute()
        {
            await Service.NavigationService.NavigateAsync(ViewName.VideoUploadPage);
        }
    }
}