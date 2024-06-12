using Prism.Commands;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Plugin.Media.Abstractions;
using Prism.Events;
using Prism.Navigation;
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
using ZeroGravity.Mobile.Resx;
using System.Diagnostics;

namespace ZeroGravity.Mobile.ViewModels
{
    public class ProfilePageViewModel : VmBase<IProfilePage, IProfilePageVmProvider, ProfilePageViewModel>
    {
        private readonly ISecureStorageService _secureStorageService;
        private readonly IEventAggregator _eventAggregator;
        private readonly ITokenService _tokenService;
        private CancellationTokenSource _cts;

        public DelegateCommand UploadPictureCommand { get; }
        public DelegateCommand LogoutCommand { get; }

        public ProfilePageViewModel(IVmCommonService service, IProfilePageVmProvider provider,
            ILoggerFactory loggerFactory, ISecureStorageService secureStorageService,
            IEventAggregator eventAggregator, IApiService apiService, ITokenService tokenService) : base(service, provider, loggerFactory, apiService)
        {
            _secureStorageService = secureStorageService;
            _eventAggregator = eventAggregator;
            _tokenService = tokenService;
            UploadPictureCommand = new DelegateCommand(UploadPictureExecute);
            LogoutCommand = new DelegateCommand(LogoutExecute);

            Title = AppResources.Profile_Title;
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            // IsBusy = true;

            bool wasTimeOut = false;

            //if (await ValidateToken())
            //{
            // get personal data from server
            Provider.GetAccountDataAsync(_cts.Token).ContinueWith(async accountDataApiCallResult =>
            {
                if (accountDataApiCallResult.Result.Success)
                {
                    var accountData = accountDataApiCallResult.Result.Value;
                    FirstName = accountData.FirstName;
                    LastName = accountData.LastName;
                    var memberSince = accountData.Created;

                    // ToDo: format date to "universal format" when JH discussed with Faz
                    //var formattedDate = memberSince.ToString("dddd, dd MMMM yyyy");
                    var formattedDate = memberSince.ToString("MMMM dd, yyyy");
                    MemberSince = AppResources.MemberSince + " " + formattedDate;
                }
                else
                {
                    IsBusy = false;

                    if (accountDataApiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                    {
                        wasTimeOut = true;
                    }

                    if (accountDataApiCallResult.Result.ErrorReason != ErrorReason.TaskCancelledByUserOperation)
                    {
                        Logger.LogInformation("Failed to load PersonalData");
                        await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error,
                            accountDataApiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                    }
                }

                if (!wasTimeOut)
                {
                    // get image from server
                    Provider.GetProfilePictureAsync(_cts.Token).ContinueWith(async profilePictureApiCallResult =>
                    {
                        if (profilePictureApiCallResult.Result.Success)
                        {
                            if (profilePictureApiCallResult.Result.Value != null)
                            {
                                Logger.LogInformation("ProfileImage loaded successfully");
                                ProfileImage = ImageSource.FromStream(() => new MemoryStream(profilePictureApiCallResult.Result.Value));
                            }
                        }
                        else
                        {
                            IsBusy = false;
                            if (profilePictureApiCallResult.Result.ErrorReason != ErrorReason.TaskCancelledByUserOperation)
                            {
                                Logger.LogInformation("Failed to load ProfileImage");

                                Device.BeginInvokeOnMainThread(async () =>
                                {
                                    await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error,
                                    profilePictureApiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                                });
                            }
                        }

                        // ToDo: await CheckProfileImageOnDevice();
                        //  }

                        IsBusy = false;
                    });
                }
            });
        }

        private async void UploadPictureExecute()
        {
            try
            {
                if (!Provider.IsPickPhotoSupported)
                {
                    // Device does not support picking images
                    await Service.DialogService.DisplayAlertAsync(AppResources.Unsuported_Title,
                        AppResources.PickPhotoUnsupported_Message, AppResources.Button_Ok);
                }
                else
                {
                    var options = new PickMediaOptions();
                    options.PhotoSize = PhotoSize.Small; // => Resize to 25% of original
                    // or custom percentage:
                    //options.PhotoSize = PhotoSize.Custom;
                    //options.CustomPhotoSize = 10; // => Resize to 10% of original

                    var pickedImage = await Provider.PickPhotoAsync(options);

                    IsBusy = true;

                    if (pickedImage != null)
                    {
                        ProfileImage = ImageSource.FromStream(() => pickedImage.GetStream());

                        byte[] pickedImageBytes = new byte[pickedImage.GetStream().Length];
                        using (var stream = new MemoryStream())
                        {
                            await pickedImage.GetStream().CopyToAsync(stream);
                            pickedImageBytes = stream.ToArray();
                        }

                        Console.WriteLine($"Picked image size: {pickedImageBytes.Length}\n");

                        await _secureStorageService.SaveObject("ProfileImage", pickedImageBytes);

                        await ValidateToken();
                        var apiCallResult = await Provider.UploadProfilePictureAsync(pickedImageBytes, _cts.Token);
                        if (!apiCallResult.Success)
                        {
                            if (apiCallResult.ErrorReason != ErrorReason.TaskCancelledByUserOperation)
                            {
                                Logger.LogInformation("Failed to load ProfileImage");
                                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error,
                                    apiCallResult.ErrorMessage, AppResources.Button_Ok);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LogoutExecute()
        {
            try
            {
                IsBusy = true;
                await _tokenService.RemoveJwt();
                await _tokenService.RemoveRefreshToken();

                await _secureStorageService.Remove(SecureStorageKey.UserEmail);

                var navParams = NavigationParametersHelper.CreateNavigationParameter(new LogoutNavParams { IsLogoutRequested = true });

                //await Service.NavigationService.NavigateAsync("/" + ViewName.GetStarted, navParams);
                await Service.NavigationService.NavigateAsync("/" + ViewName.ContentShellPage, navParams);
            }
            catch (Exception e)
            {
                await Service.DialogService.DisplayAlertAsync("Error", e.Message, AppResources.Button_Ok);
                Logger.LogError(e, e.Message);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async Task CheckProfileImageOnDevice()
        {
            // check SecureStorage for image
            var imageBytes = await _secureStorageService.LoadObject<byte[]>("ProfileImage"); // ToDo: get from cache, NOT from SecureStorage

            if (imageBytes != null)
            {
                var image = ImageSource.FromStream(() => new MemoryStream(imageBytes));

                if (image != null)
                {
                    // set image
                    ProfileImage = image;
                    Console.WriteLine($"Loaded image of size {imageBytes.Length}\n");
                }
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            try
            {
                var logoutNavParams = NavigationParametersHelper.GetNavigationParameters<LogoutNavParams>(parameters);

                if (logoutNavParams == null)
                {
                    // if user navigated back and did not press logout button
                    _eventAggregator.GetEvent<ProfileImageChangedEvent>().Publish();
                }
                CancelPendingRequest(_cts);
                base.OnNavigatedFrom(parameters);
            }
            catch(Exception ex)
            {
                Debug.WriteLine("ProfilePage NavigatedFrom" + ex.Message + ex.StackTrace);
            }
            
        }

        private ImageSource _profileImage;

        public ImageSource ProfileImage
        {
            get { return _profileImage; }
            set { SetProperty(ref _profileImage, value); }
        }

        private string _firstName;

        public string FirstName
        {
            get { return _firstName; }
            set { SetProperty(ref _firstName, value); }
        }

        private string _lastName;

        public string LastName
        {
            get { return _lastName; }
            set { SetProperty(ref _lastName, value); }
        }

        private string _memberSince;

        public string MemberSince
        {
            get { return _memberSince; }
            set { SetProperty(ref _memberSince, value); }
        }
    }
}