using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Plugin.Media.Abstractions;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    public class WizardStep3PageViewModel : VmBase<IWizardStep3Page, IWizardStep3PageVmProvider, WizardStep3PageViewModel>
    {
        public DelegateCommand WizardStep3SaveDataCommand { get; }
        public DelegateCommand WizardStep3PickPictureCommand { get; }

        public WizardStep3PageViewModel(IVmCommonService service, IWizardStep3PageVmProvider provider, ILoggerFactory loggerFactory, IEventAggregator eventAggregator, bool checkInternet = true) : base(service, provider, loggerFactory, checkInternet)
        {
            WizardStep3PickPictureCommand = new DelegateCommand(PickPictureExecute);
            WizardStep3SaveDataCommand = new DelegateCommand(SaveWizardStep3DataExecute);
            LogoImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.background-logo.png");
        }

        private ImageSource _logoImageSource;
        public ImageSource LogoImageSource
        {
            get => _logoImageSource;
            set => SetProperty(ref _logoImageSource, value);
        }


        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Title = AppResources.WizardStep_Page3Title;

            if (!HasInternetConnection)
            {
                return;
            }

            //  IsBusy = true;

            _cts = new CancellationTokenSource();

            LoadProfileImage();

            IsBusy = false;
        }

        private async Task LoadProfileImage()
        {
             Provider.GetProfilePictureAsync(_cts.Token).ContinueWith(async apiCallResult =>
             {
                 if (apiCallResult.Result.Success)
                 {
                     if (apiCallResult.Result.Value != null)
                     {
                         Logger.LogInformation("ProfileImage successfully loaded.");

                         ProfileImage = ImageSource.FromStream(() => new MemoryStream(apiCallResult.Result.Value));

                         ImageStreamBytes = apiCallResult.Result.Value;
                     }
                 }
                 else
                 {
                     Logger.LogInformation("Failed to load ProfileImage");
                 }
             });
        }

        private async void PickPictureExecute()
        {
            try
            {
                if (!Provider.IsPickPhotoSupported)
                {
                    // Device does not support picking images
                    await Service.DialogService.DisplayAlertAsync(AppResources.Unsuported_Title, AppResources.PickPhotoUnsupported_Message, AppResources.Button_Ok);
                }
                else
                {
                    var options = new PickMediaOptions();
                    options.PhotoSize = PhotoSize.Small; // => Resize to 25% of original

                    var pickedImage = await Provider.PickPhotoAsync(options);

                    IsBusy = true;

                    if (pickedImage != null)
                    {
                        ProfileImage = ImageSource.FromStream(() => pickedImage.GetStream());

                        using (var stream = new MemoryStream())
                        {
                            await pickedImage.GetStream().CopyToAsync(stream);
                            ImageStreamBytes = stream.ToArray();
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

        private async void SaveWizardStep3DataExecute()
        {
            await SaveProfileImageAsync();

            await Service.NavigationService.NavigateAsync(ViewName.WizardStep4Page);
        }

        private async Task SaveProfileImageAsync()
        {
            IsBusy = true;

            _cts = new CancellationTokenSource();

            try
            {
                if (ImageStreamBytes != null)
                {
                    if (await ValidateToken())
                    {
                        var apiCallResult = await Provider.UploadProfilePictureAsync(ImageStreamBytes, _cts.Token);
                        if (!apiCallResult.Success)
                        {
                            if (apiCallResult.ErrorReason != ErrorReason.TaskCancelledByUserOperation && apiCallResult.ErrorReason != ErrorReason.TimeOut)
                            {
                                Logger.LogInformation("Failed to upload ProfileImage");
                                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, apiCallResult.ErrorMessage, AppResources.Button_Ok);
                            }
                        }
                        else
                        {
                            Logger.LogInformation("ProfileImage successfully saved.");
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


        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }


        private ImageSource _profileImage;

        public ImageSource ProfileImage
        {
            get => _profileImage;
            set => SetProperty(ref _profileImage, value);
        }


        private byte[] _imageStreamBytes;
        public byte[] ImageStreamBytes
        {
            get => _imageStreamBytes;
            set => SetProperty(ref _imageStreamBytes, value);
        }


        private CancellationTokenSource _cts;
    }
}