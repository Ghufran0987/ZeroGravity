using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage;
using Azure.Storage.Blobs;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.ViewModels
{
    public class VideoUploadPageViewModel : VmBase<IVideoUploadPage, IVideoUploadPageVmProvider, VideoUploadPageViewModel>
    {
        public DelegateCommand SelectVideoCommand { get; }
        public DelegateCommand StartUploadCommand { get; }
        public DelegateCommand CancelUploadCommand { get; }
        public DelegateCommand GoBackToStreamCommand { get; }

        public ObservableCollection<ComboBoxItem> LanguageList { get; set; }

        private Stream _videoFileStream;
        private string _originalFileName;
        private string _fileType = string.Empty;
        private long _sizeInBytes;

        private Progress<long> _uploadProgress;

        private CancellationTokenSource _cts;

        private CancellationTokenSource _uploadCts;

        public VideoUploadPageViewModel(IVmCommonService service, IVideoUploadPageVmProvider provider, ILoggerFactory loggerFactory, bool checkInternet = false) : base(service, provider, loggerFactory, checkInternet)
        {
            InitComboBox();

            SelectVideoCommand = new DelegateCommand(SelectVideoExecute);
            StartUploadCommand = new DelegateCommand(StartUploadExecute);
            CancelUploadCommand = new DelegateCommand(CancelUploadExecute);
            GoBackToStreamCommand = new DelegateCommand(GoBackToStreamExecute);
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            //Title = AppResources.VideoUploadPageTitle;

            InitializeVideoUiControls();
            InitComboBox();

            _uploadProgress = new Progress<long>();
            _uploadProgress.ProgressChanged += OnUploadProgressChanged;
        }

        private void InitComboBox()
        {
            LanguageList = new ObservableCollection<ComboBoxItem>();
            LanguageList.Add(new ComboBoxItem { Id = 0, Text = AppResources.Language_English });
            LanguageList.Add(new ComboBoxItem { Id = 1, Text = AppResources.Language_German });
            LanguageList.Add(new ComboBoxItem { Id = 2, Text = AppResources.Language_French });
            LanguageList.Add(new ComboBoxItem { Id = 3, Text = AppResources.Language_Other });

            LanguageIndex = -1;
        }

        private void InitializeVideoUiControls()
        {
            UploadStatus = VideoUploadStatus.Idle;

            UploadDescriptionText = AppResources.VideoUpload_SelectVideo;

            VideoTitle = string.Empty;
            VideoDescription = string.Empty;
            LanguageIndex = -1;
        }

        private async void SelectVideoExecute()
        {
            InitializeVideoUiControls();
            _videoFileStream?.Close();
            _videoFileStream = null;

            _cts = new CancellationTokenSource();

            var pressedButton = await Service.DialogService.DisplayActionSheetAsync(AppResources.VideoUpload_SelectSource_Message, AppResources.Button_Cancel, null, AppResources.VideoUpload_DeviceStorageSource, AppResources.VideoUpload_CameraSource);

            if (pressedButton.Equals(AppResources.VideoUpload_DeviceStorageSource))
            {
                await PickVideoAsync();
            }
            else if (pressedButton.Equals(AppResources.VideoUpload_CameraSource))
            {
                await TakeVideoAsync();
            }
        }

        private async Task PickVideoAsync()
        {
            try
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    var permissionStatus = await Provider.CheckAndRequestPermissionAsync(new Permissions.StorageRead());
                    if (permissionStatus != PermissionStatus.Granted)
                    {
                        return;
                    }

                    var pickedVideo = await MediaPicker.PickVideoAsync();

                    if (pickedVideo != null)
                    {
                        _originalFileName = pickedVideo.FileName;

                        UploadDescriptionText = _originalFileName;

                        _fileType = GetFileType(_originalFileName);
                        _videoFileStream = await pickedVideo.OpenReadAsync();

#if DEBUG
                        var sizeInBytes = _videoFileStream.Length;
                        double sizeInMegaBytes = CalculateSizeInMb(sizeInBytes);
                        var sizeInMbRounded = Math.Round(sizeInMegaBytes, 2);
                        System.Diagnostics.Debug.WriteLine($"Picked video {_originalFileName} of size: {sizeInBytes} bytes ({sizeInMbRounded} MB).{Environment.NewLine}");
#endif

                        await CheckFileSizeAndAlertIfTooLarge();
                    }
                });
            }
            catch (TaskCanceledException ex)
            {
                if (_cts.IsCancellationRequested)
                {
                    Logger.LogInformation("User cancelled video picking.", ex, ex.Message);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("An error occurred when trying to pick a video.", e, e.Message);
                await Service.DialogService.DisplayAlertAsync(AppResources.VideoUploadPageTitle, AppResources.Common_Error_Unknown, AppResources.Button_Ok);
            }
        }

        private async Task TakeVideoAsync()
        {
            try
            {
                var permissionStatusStorageWrite = await Provider.CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
                if (permissionStatusStorageWrite != PermissionStatus.Granted)
                {
                    return;
                }

                var permissionStatusCamera = await Provider.CheckAndRequestPermissionAsync(new Permissions.Camera());
                if (permissionStatusCamera != PermissionStatus.Granted)
                {
                    return;
                }

                var permissionStatusMicrophone = await Provider.CheckAndRequestPermissionAsync(new Permissions.Microphone());
                if (permissionStatusMicrophone != PermissionStatus.Granted)
                {
                    return;
                }

                var recordedVideo = await MediaPicker.CaptureVideoAsync();

                if (recordedVideo != null)
                {
                    _originalFileName = recordedVideo.FileName;

                    UploadDescriptionText = _originalFileName;

                    _fileType = GetFileType(_originalFileName);
                    _videoFileStream = await recordedVideo.OpenReadAsync();

#if DEBUG
                    var sizeInBytes = _videoFileStream.Length;
                    double sizeInMegaBytes = CalculateSizeInMb(sizeInBytes);
                    var sizeInMbRounded = Math.Round(sizeInMegaBytes, 2);
                    System.Diagnostics.Debug.WriteLine($"Recorded video {_originalFileName} of size: {sizeInBytes} bytes ({sizeInMbRounded} MB).{Environment.NewLine}");
#endif

                    await CheckFileSizeAndAlertIfTooLarge();
                }
            }
            catch (TaskCanceledException ex)
            {
                if (_cts.IsCancellationRequested)
                {
                    Logger.LogInformation("User cancelled video recording.", ex, ex.Message);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("An error occured when trying to record a video.", e, e.Message);
                await Service.DialogService.DisplayAlertAsync(AppResources.VideoUploadPageTitle, AppResources.Common_Error_Unknown, AppResources.Button_Ok);
            }
        }

        private async void StartUploadExecute()
        {
            if (_videoFileStream != null)
            {
                await UploadToAzureBlobStorage(_videoFileStream, _originalFileName);
            }
        }

        private async Task UploadToAzureBlobStorage(Stream fileStream, string originalFileName)
        {
            if (fileStream == null)
            {
                await Service.DialogService.DisplayAlertAsync(AppResources.VideoUploadPageTitle, AppResources.VideoUpload_NothingToUpload, AppResources.Button_Ok);
                return;
            }

            if (!HasInternetConnection)
            {
                return;
            }

            try
            {
                if (string.IsNullOrEmpty(VideoTitle))
                {
                    await Service.DialogService.DisplayAlertAsync(AppResources.VideoUploadPageTitle, AppResources.VideoUpload_VideoTitleMissing_Message, AppResources.Button_Ok);
                    return;
                }

                IsBusy = true;

                BlobContainerClient blobContainerClient = new BlobContainerClient(VideoUploadSettings.AzureBlobStorageConnectionString, VideoUploadSettings.AzureBlobStorageContainerName);
                await blobContainerClient.CreateIfNotExistsAsync();
                string blobName = Guid.NewGuid() + _fileType; // keep user's file names secret

                var blobClient = blobContainerClient.GetBlobClient(blobName);
                fileStream.Position = 0;

                System.Diagnostics.Debug.WriteLine($"Upload started for video {blobName}.{Environment.NewLine}");

                var languageItem = LanguageList.FirstOrDefault(x => x.Id == LanguageIndex);
                var videoLanguage = languageItem != null ? languageItem.Text : LanguageList.Last().Text;

                var trimmedVideoTitle = GetMetaDataStringWhichMatchesAzureRequirements(VideoTitle);
                var trimmedVideoDescription = GetMetaDataStringWhichMatchesAzureRequirements(VideoDescription);

                IDictionary<string, string> metaData = new Dictionary<string, string>();
                metaData.Add(VideoUploadSettings.MetaDataVideoTitleKey, trimmedVideoTitle);
                metaData.Add(VideoUploadSettings.MetaDataVideoDescriptionKey, trimmedVideoDescription);
                metaData.Add(VideoUploadSettings.MetaDataVideoLanguageKey, videoLanguage);

                _sizeInBytes = fileStream.Length;
                UploadStatus = VideoUploadStatus.Uploading;

                UploadProgressInPercent = "0";

                _uploadCts = new CancellationTokenSource();

                var response = await blobClient.UploadAsync(fileStream, null, metaData, null, _uploadProgress, null, new StorageTransferOptions(), _uploadCts.Token);

                var rawResponse = response.GetRawResponse();

                UploadDescriptionText = originalFileName;

                UploadStatus = rawResponse.Status == 201 ? VideoUploadStatus.Finished : VideoUploadStatus.Other;

#if DEBUG
                System.Diagnostics.Debug.WriteLine($"Uploaded video {blobName}.{Environment.NewLine}");
                await Service.DialogService.DisplayAlertAsync(AppResources.VideoUploadPageTitle, $"Status code: {rawResponse.Status}\nReason phrase: {rawResponse.ReasonPhrase}", AppResources.Button_Ok);
#endif
            }
            catch (TaskCanceledException ex)
            {
                if (_uploadCts.IsCancellationRequested)
                {
                    UploadStatus = VideoUploadStatus.Idle;
                    Logger.LogInformation("User cancelled video upload.", ex, ex.Message);
                }
            }
            catch (Exception e)
            {
                UploadStatus = VideoUploadStatus.Error;
                Logger.LogError("An error occured when trying to upload a video.", e, e.Message);

                await Service.DialogService.DisplayAlertAsync(AppResources.VideoUploadPageTitle, AppResources.Common_Error_Unknown, AppResources.Button_Ok);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void OnUploadProgressChanged(object sender, long e)
        {
            IsBusy = false;

            var progress = (e * 100) / _sizeInBytes;

            UploadProgressInPercent = progress.ToString();
        }

        private void CancelUploadExecute()
        {
            _uploadCts?.Cancel();
        }

        private async void GoBackToStreamExecute()
        {
            await Service.NavigationService.GoBackAsync();
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            _videoFileStream?.Close();
            _uploadProgress.ProgressChanged -= OnUploadProgressChanged;

            CancelPendingRequest(_cts);
        }

        /// <summary>
        /// Removes all non allowed characters from a provided string.
        /// Info: Azure's blob meta data does not accept strings with whitespace in the end or line break characters or non ASCII characters.
        /// </summary>
        /// <param name="metaDataString"></param>
        /// <returns>A modified string without illegal characters in the provided string.</returns>
        private string GetMetaDataStringWhichMatchesAzureRequirements(string metaDataString)
        {
            // remove whitespace in the end
            var trimmed = metaDataString.TrimEnd();
            // remove line breaks
            var trimmedAndReplaced = trimmed.Replace(Environment.NewLine, string.Empty);
            // remove all non ASCII characters
            var finalString = Regex.Replace(trimmedAndReplaced, @"[^\u0000-\u007F]+", string.Empty);
            return finalString;
        }

        private string GetFileType(string fileName)
        {
            int posOfLastDot = fileName.LastIndexOf(".");
            var fileType = fileName.Substring(posOfLastDot);

            return fileType;
        }

        private async Task CheckFileSizeAndAlertIfTooLarge()
        {
            var sizeInBytes = _videoFileStream.Length;
            var sizeInMb = CalculateSizeInMb(sizeInBytes);

            if (sizeInMb > VideoUploadSettings.MaxVideoUploadSizeInMb)
            {
                var sizeInMbRounded = Math.Round(sizeInMb, 2);
                var msg = string.Format(AppResources.VideoUpload_RecordedVideoTooLarge, sizeInMbRounded, VideoUploadSettings.MaxVideoUploadSizeInMb);

                await Service.DialogService.DisplayAlertAsync(AppResources.VideoUploadPageTitle, msg, AppResources.Button_Ok);

                _videoFileStream?.Close();
            }
        }

        private double CalculateSizeInMb(long sizeInBytes)
        {
            var sizeInMb = (double)sizeInBytes / (1024 * 1024);
            return sizeInMb;
        }

        private VideoUploadStatus _uploadStatus;

        public VideoUploadStatus UploadStatus
        {
            get => _uploadStatus;
            set
            {
                SetProperty(ref _uploadStatus, value);

                UpdateButtonVisibility(value);
            }
        }

        private async void UpdateButtonVisibility(VideoUploadStatus status)
        {
            if (status == VideoUploadStatus.Idle || status == VideoUploadStatus.Error || status == VideoUploadStatus.Other)
            {
                IsUploading = false;

                IsUploadButtonVisible = true;
                IsCancelButtonVisible = false;
                IsGoBackToStreamButtonVisible = false;
                return;
            }

            if (status == VideoUploadStatus.Uploading)
            {
                IsUploading = true;

                IsUploadButtonVisible = false;
                IsCancelButtonVisible = true;
                IsGoBackToStreamButtonVisible = false;
                return;
            }

            if (status == VideoUploadStatus.Finished)
            {
                IsUploading = false;

                IsUploadButtonVisible = false;
                IsCancelButtonVisible = false;
                IsGoBackToStreamButtonVisible = true;

                await Service.DialogService.DisplayAlertAsync(AppResources.VideoUploadPageTitle, "Thanks for uploading. Your video will be in the media play list after our review team approves", AppResources.Button_Ok);
                return;
            }
        }

        private string _uploadProgressInPercent;

        public string UploadProgressInPercent
        {
            get => _uploadProgressInPercent;
            set => SetProperty(ref _uploadProgressInPercent, value);
        }

        private string _uploadDescriptionText;

        public string UploadDescriptionText
        {
            get => _uploadDescriptionText;
            set => SetProperty(ref _uploadDescriptionText, value);
        }

        private int _languageIndex;

        public int LanguageIndex
        {
            get => _languageIndex;
            set => SetProperty(ref _languageIndex, value);
        }

        private string _videoTitle;

        public string VideoTitle
        {
            get => _videoTitle;
            set => SetProperty(ref _videoTitle, value);
        }

        private string _videoDescription;

        public string VideoDescription
        {
            get => _videoDescription;
            set => SetProperty(ref _videoDescription, value);
        }

        private bool _isUploading;

        public bool IsUploading
        {
            get => _isUploading;
            set => SetProperty(ref _isUploading, value);
        }

        private bool _isUploadButtonVisible;

        public bool IsUploadButtonVisible
        {
            get => _isUploadButtonVisible;
            set => SetProperty(ref _isUploadButtonVisible, value);
        }

        private bool _isCancelButtonVisible;

        public bool IsCancelButtonVisible
        {
            get => _isCancelButtonVisible;
            set => SetProperty(ref _isCancelButtonVisible, value);
        }

        private bool _isGoBackToStreamButtonVisible;

        public bool IsGoBackToStreamButtonVisible
        {
            get => _isGoBackToStreamButtonVisible;
            set => SetProperty(ref _isGoBackToStreamButtonVisible, value);
        }
    }
}