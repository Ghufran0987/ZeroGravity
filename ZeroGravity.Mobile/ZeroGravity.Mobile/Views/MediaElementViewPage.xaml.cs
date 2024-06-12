using MediaManager;
using MediaManager.Forms;
using MediaManager.Library;
using MediaManager.Media;
using MediaManager.Playback;
using MediaManager.Video;
using System;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MediaElementViewPage : IMediaElementViewPage
    {
        public MediaElementViewPageViewModel _mediaElementViewPageViewModel;
        public MediaElementViewPage()
        {
            InitializeComponent();
            CrossMediaManager.Current.MediaPlayer.VideoView = this.mediaElement as IVideoView;
        }

        protected override void OnBindingContextChanged()
        {

            base.OnBindingContextChanged();
            _mediaElementViewPageViewModel = this.BindingContext as MediaElementViewPageViewModel;
        }



        private void SubscribeMediaEvent()
        {
            CrossMediaManager.Current.StateChanged += OnMediaStateChange;
        }

        private void UnSubscribeMediaEvent()
        {
            CrossMediaManager.Current.StateChanged -= OnMediaStateChange;
        }



        private void OnMediaStateChange(object sender, StateChangedEventArgs e)
        {
            if (_mediaElementViewPageViewModel != null)
            {
                if (_mediaElementViewPageViewModel.StreamContentType == Shared.Enums.StreamContentType.Stream)
                    return;
            }

            try
            {
                switch (e.State)
                {
                    case MediaManager.Player.MediaPlayerState.Stopped:

                        _mediaElementViewPageViewModel.UpdateOnNaviagetFrom();
                        break;
                    case MediaManager.Player.MediaPlayerState.Loading:
                        break;
                    case MediaManager.Player.MediaPlayerState.Buffering:
                        break;
                    case MediaManager.Player.MediaPlayerState.Playing:
                        _mediaElementViewPageViewModel.OnMediaOpned();
                        break;
                    case MediaManager.Player.MediaPlayerState.Paused:
                        _mediaElementViewPageViewModel.UpdateOnNaviagetFrom();
                        break;
                    case MediaManager.Player.MediaPlayerState.Failed:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {


            }
        }

        private async void OnDisappearing(object sender, System.EventArgs e)
        {
            try
            {
                await CrossMediaManager.Current.Stop();
            }
            catch (Exception ex)
            {


            }
            UnSubscribeMediaEvent();
        }

        private void OnAppearing(object sender, EventArgs e)
        {
            SubscribeMediaEvent();
        }

        void OnSwiped(object sender, SwipedEventArgs e)
        {
            switch (e.Direction)
            {
                case SwipeDirection.Left:
                    // Handle the swipe
                    break;
                case SwipeDirection.Right:
                    // Handle the swipe
                    break;
                case SwipeDirection.Up:
                    // Handle the swipe
                    CrossMediaManager.Current.PlayNext();
                    break;
                case SwipeDirection.Down:
                    // Handle the swipe
                    CrossMediaManager.Current.PlayPrevious();
                    break;
            }
        }
    }
}