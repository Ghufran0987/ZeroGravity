using MediaManager;
using MediaManager.Library;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.CommunityToolkit.UI.Views;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models.Dto;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class MediaElementViewPageViewModel : VmBase<IMediaElementViewPage>
    {
        private readonly IEventAggregator _eventAggregator;
        private StreamContentProxy _selectedMediaVideoItem;
        private List<StreamContentProxy> _streamContent;
        public StreamContentType StreamContentType;

        public StreamContentProxy SelectedMediaVideoItem
        {
            get => _selectedMediaVideoItem;
            private set => SetProperty(ref _selectedMediaVideoItem, value);
        }

        public List<StreamContentProxy> StreamContent
        {
            get => _streamContent;
            private set => SetProperty(ref _streamContent, value);
        }

        public MediaElementViewPageViewModel(IVmCommonService service, IEventAggregator eventAggregator, bool checkInternet = true) : base(service, checkInternet)
        {
            _eventAggregator = eventAggregator;
        }

        private DateTime _startTime { get; set; }

        private DateTime _endTime { get; set; }

        public void OnMediaOpned()
        {
            _startTime = DateTime.Now;
        }

        public void OnMediaEnded()
        {
            PlayNextMedia();
        }

        public void PlayNextMedia()
        {
            try
            {
                _endTime = DateTime.Now;
                var diffTime = _endTime.Subtract(_startTime);

                var index = _streamContent.IndexOf(SelectedMediaVideoItem);
                if (index < (StreamContent.Count - 1))
                {
                    SelectedMediaVideoItem = StreamContent[index++];
                }
                else
                {
                    SelectedMediaVideoItem = StreamContent[0];
                }
                _eventAggregator.GetEvent<MediaDetailEvent>().Publish(new MediaDetailEvent() { MediaPlayTime = diffTime });
                _startTime = DateTime.Now;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            var navData = parameters.GetValue<MeditataionDetailEvent>("ZeroGravity.Mobile.Events.MeditataionDetailEvent");
            if (navData != null)
            {
                try
                {
                    StreamContent = navData.StreamContent;
                    SelectedMediaVideoItem = navData.SelectedMediaVideoItem;
                    StreamContentType = navData.StreamContentType;
                    var items = LoadMediaItems(StreamContent, SelectedMediaVideoItem);
                    CrossMediaManager.Current.Queue.Clear();
                    await CrossMediaManager.Current.Play(items);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        private List<IMediaItem> LoadMediaItems(List<StreamContentProxy> streamContentProxies, StreamContentProxy streamContentProxy)
        {
            var resultList = new List<IMediaItem>();
            try
            {
                var selectedIndex = streamContentProxies.IndexOf(streamContentProxy);
                var totalCount = streamContentProxies.Count;
                for (int i = 0; i < streamContentProxies.Count; i++)
                {
                    if (totalCount > selectedIndex)
                    {
                        IMediaItem mediaItem = new MediaItem
                        {
                            MediaUri = streamContentProxies[selectedIndex].VideoUrl,
                            ImageUri = streamContentProxies[selectedIndex].ThumbnailUrl,
                            DisplayTitle = streamContentProxies[selectedIndex].Description,
                            Extras = streamContentProxies[selectedIndex].Id
                        };
                        resultList.Add(mediaItem);
                        selectedIndex++;
                    }
                    else
                    {
                        var index = selectedIndex - totalCount;
                        IMediaItem mediaItem = new MediaItem
                        {
                            MediaUri = streamContentProxies[index].VideoUrl,
                            ImageUri = streamContentProxies[index].ThumbnailUrl,
                            DisplayTitle = streamContentProxies[index].Description,
                            Extras = streamContentProxies[index].Id
                        };
                        resultList.Add(mediaItem);
                        selectedIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return resultList;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        public void UpdateOnNaviagetFrom()
        {
            try
            {
                _endTime = DateTime.Now;
                if (_startTime != DateTime.MinValue && _endTime > _startTime)
                {
                    var diffTime = _endTime.Subtract(_startTime);
                    _eventAggregator.GetEvent<MediaDetailEvent>().Publish(new MediaDetailEvent()
                    {
                        MediaPlayTime = new TimeSpan(diffTime.Hours, diffTime.Minutes, diffTime.Seconds)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}