using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ProfileImagePageViewModel : VmBase<IProfileImagePage, IProfileImagePageVmProvider, ProfileImagePageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly ISecureStorageService _secureStorageService;
        private readonly INotificationService _notificationService;
        private CancellationTokenSource _cts;

        public DelegateCommand ProfilePageCommand { get; }
        public DelegateCommand NotificationPageCommand { get; }

        public ProfileImagePageViewModel(IVmCommonService service, IProfileImagePageVmProvider provider,
            ILoggerFactory loggerFactory, IEventAggregator eventAggregator,
            ISecureStorageService secureStorageService, IApiService apiService,
            INotificationService notificationService) : base(service, provider, loggerFactory, apiService)
        {
            _eventAggregator = eventAggregator;
            _secureStorageService = secureStorageService;
            _notificationService = notificationService;

            ProfilePageCommand = new DelegateCommand(ProfilePageExecute);
            NotificationPageCommand = new DelegateCommand(NotificationPageExecute);

            //BadgeText = " "; // Bobbel anzeigen
            BadgeText = ""; // Bobbel ausblenden
        }

        private void NotificationPageExecute()
        {
            // publish event here - subscribe in ContentShellPageViewModel
            _eventAggregator.GetEvent<NotificationPageEvent>().Publish();
        }

        private void ProfilePageExecute()
        {
            // publish event here - subscribe in ContentShellPageViewModel
            _eventAggregator.GetEvent<ProfilePageEvent>().Publish();
        }

        private void SubscribeToEvents()
        {
            UnsubscribeFromEvents();

            _eventAggregator.GetEvent<ProfileImageChangedEvent>().Subscribe(OnProfileImageChangedEvent);
            _eventAggregator.GetEvent<NotificationsReadEvent>().Subscribe(OnNotificationsReadEvent);
        }

        private async void OnNotificationsReadEvent()
        {
            //await CheckNotificationReadStatus();
        }

        private void UnsubscribeFromEvents()
        {
            _eventAggregator.GetEvent<ProfileImageChangedEvent>().Unsubscribe(OnProfileImageChangedEvent);
            _eventAggregator.GetEvent<NotificationsReadEvent>().Unsubscribe(OnNotificationsReadEvent);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            UnsubscribeFromEvents();

            CancelPendingRequest(_cts);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            IsBusy = true;

            SubscribeToEvents();

            if (await ValidateToken())
            {
                await LoadProfileImage();
                //await CheckNotificationReadStatus();
            }

            IsBusy = false;

            // ToDo:
            //await CheckProfileImageOnDevice();
        }

        private async Task CheckNotificationReadStatus()
        {
            var notifications = await Provider.GetNotificationsAsync(_cts.Token);

            if (notifications.Any())
            {
                if (notifications.Exists(n => n.HasBeenRead == false))
                {
                    // Bobbel anzeigen
                    BadgeText = " ";
                }
                else
                {
                    // Bobbel ausblenden
                    BadgeText = "";
                }
            }
        }

        private async Task LoadProfileImage()
        {
            var apiCallResult = await Provider.GetProfilePictureAsync(_cts.Token);

            if (apiCallResult.Success)
            {
                if (apiCallResult.Value != null)
                {
                    Image = ImageSource.FromStream(() => new MemoryStream(apiCallResult.Value));
                }
            }
            else
            {
                Logger.LogInformation("Failed to load ProfileImage");
                //await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error,
                //    apiCallResult.ErrorMessage, AppResources.Button_Ok);
            }
        }

        private async void OnProfileImageChangedEvent()
        {
            await LoadProfileImage();
            //await CheckProfileImageOnDevice();
        }

        private async Task CheckProfileImageOnDevice()
        {
            // check SecureStorage for image
            var imageBytes = await _secureStorageService.LoadObject<byte[]>("ProfileImage");

            if (imageBytes != null)
            {
                var image = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                if (image != null)
                {
                    // set image
                    Image = image;
                    Console.WriteLine($"Loaded image of size {imageBytes.Length}\n");
                }
            }
        }

        private ImageSource _profileImage;
        public ImageSource Image
        {
            get { return _profileImage; }
            set { SetProperty(ref _profileImage, value); }
        }

        private string _badgeText;

        public string BadgeText
        {
            get { return _badgeText; }
            set { SetProperty(ref _badgeText, value); }
        }


    }
}