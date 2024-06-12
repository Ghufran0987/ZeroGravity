using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class WellbeingDataPageViewModel : VmBase<IWellbeingDataPage, IWellbeingDataPageVmProvider, WellbeingDataPageViewModel>
    {
        private CancellationTokenSource _cts;
        private WellbeingNavParams _navParams;
        private IEducationalInfoProvider educationalInfoProvider;
        private WellbeingDataProxy _wellbeingDataProxy;

        public WellbeingDataPageViewModel(IVmCommonService service, IWellbeingDataPageVmProvider provider, IEducationalInfoProvider _educationalInfoProvider,
            ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            SaveWellbeingDataCommand = new DelegateCommand(SaveWellbeingData);
            educationalInfoProvider = _educationalInfoProvider;
            WellbeingTypeList = new List<RadioButtonItemProxy>();

            MinRating = (int)WellbeingType.VeryBad;
            MaxRating = (int)WellbeingType.Fantastic;
            WellbeingImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Wellbeing.png");

            WellbeingTypeVeryBad = new RadioButtonItemProxy
            {
                Key = (int)WellbeingType.VeryBad,
                // IsChecked = true
            };

            WellbeingTypeBad = new RadioButtonItemProxy
            {
                Key = (int)WellbeingType.Bad
            };

            WellbeingTypeNotSo = new RadioButtonItemProxy
            {
                Key = (int)WellbeingType.NotSoGreat
            };

            WellbeingTypeGreat = new RadioButtonItemProxy
            {
                Key = (int)WellbeingType.Great
            };

            WellbeingTypeFantastic = new RadioButtonItemProxy
            {
                Key = (int)WellbeingType.Fantastic
            };

            WellbeingTypeList.AddRange(new[]
            {
                WellbeingTypeVeryBad, WellbeingTypeBad, WellbeingTypeNotSo, WellbeingTypeGreat, WellbeingTypeFantastic
            });
        }

        private List<RadioButtonItemProxy> _wellbeingTypeList;

        public List<RadioButtonItemProxy> WellbeingTypeList
        {
            get => _wellbeingTypeList;
            set => SetProperty(ref _wellbeingTypeList, value);
        }

        private RadioButtonItemProxy _wellbeingTypeVeryBad;

        public RadioButtonItemProxy WellbeingTypeVeryBad
        {
            get => _wellbeingTypeVeryBad;
            set => SetProperty(ref _wellbeingTypeVeryBad, value);
        }

        private RadioButtonItemProxy _wellbeingTypeBad;

        public RadioButtonItemProxy WellbeingTypeBad
        {
            get => _wellbeingTypeBad;
            set => SetProperty(ref _wellbeingTypeBad, value);
        }

        private RadioButtonItemProxy _wellbeingTypeNotSo;

        public RadioButtonItemProxy WellbeingTypeNotSo
        {
            get => _wellbeingTypeNotSo;
            set => SetProperty(ref _wellbeingTypeNotSo, value);
        }

        private RadioButtonItemProxy _wellbeingTypeGreat;

        public RadioButtonItemProxy WellbeingTypeGreat
        {
            get => _wellbeingTypeGreat;
            set => SetProperty(ref _wellbeingTypeGreat, value);
        }

        private RadioButtonItemProxy _wellbeingTypeFantastic;

        public RadioButtonItemProxy WellbeingTypeFantastic
        {
            get => _wellbeingTypeFantastic;
            set => SetProperty(ref _wellbeingTypeFantastic, value);
        }

        private ImageSource _wellbeingImageSource;

        public ImageSource WellbeingImageSource
        {
            get => _wellbeingImageSource;
            set => SetProperty(ref _wellbeingImageSource, value);
        }

        public DelegateCommand SaveWellbeingDataCommand { get; }

        public WellbeingDataProxy WellbeingDataProxy
        {
            get => _wellbeingDataProxy;
            set => SetProperty(ref _wellbeingDataProxy, value);
        }

        private int _minRating;

        public int MinRating
        {
            get => _minRating;
            set => SetProperty(ref _minRating, value);
        }

        private int _maxRating;

        public int MaxRating
        {
            get => _maxRating;
            set => SetProperty(ref _maxRating, value);
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            var pageNavigationParams =
                NavigationParametersHelper.GetNavigationParameters<WellbeingNavParams>(parameters);

            if (pageNavigationParams != null)
            {
                _navParams = pageNavigationParams;

                Title = DateTimeHelper.ToLocalDateZeroGravityFormat(_navParams.TargetDateTime);

                //var isAuthorized = await ValidateToken();
                //if (isAuthorized)

                GetWellbeingDataAsync(_navParams.TargetDateTime);
            }
            else
            {
                WellbeingDataProxy = new WellbeingDataProxy
                {
                    Created = DateTime.Today
                };
            }

            var showOverlay = Service.HoldingPagesSettingsService.ShouldDailyShow(HoldingPageType.Wellbeing);
            if (showOverlay)
            {
                IsLoadingImageBusy = true;
                OpenOverlay();
                educationalInfoProvider.GetEducationalInfoByIdAsync(_cts.Token, StorageFolderConstants.Wellbeing).ContinueWith(async apiCallEducation =>
                {
                    if (apiCallEducation.Result.Success && apiCallEducation.Result.Value != null)
                    {
                        var educationResult = apiCallEducation.Result;
                        ProductImage = new UriImageSource { Uri = new Uri(educationResult.Value.ImageUrl) };
                    }
                    IsLoadingImageBusy = false;
                });
            }
        }

        protected override void OnCustomCloseOverlay()
        {
            base.OnCustomCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowAgain(HoldingPageType.Wellbeing);
        }

        internal override void OnDailyCloseOverlay()
        {
            base.OnDailyCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowToday(HoldingPageType.Wellbeing);
        }

        private async void SaveWellbeingData()
        {
            try
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await CreateWellbeingData();
            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, e.Message,
                    AppResources.Button_Ok);
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

        private async Task UpdateWellbeingData()
        {
            IsBusy = true;

            var selectedDietItem = _wellbeingTypeList.FirstOrDefault(_ => _.IsChecked);

            if (selectedDietItem != null) WellbeingDataProxy.Rating = selectedDietItem.Key;

            WellbeingDataProxy.Created = DateTime.Now; // update creation timestamp

            var apiCallResult = await Provider.UpdateWellbeingDataAsnyc(WellbeingDataProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"WellbeingData for Account: {WellbeingDataProxy.AccountId} successfully updated.");

                WellbeingDataProxy = apiCallResult.Value;

                await Service.NavigationService.GoBackAsync();
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.WellbeingData_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async Task CreateWellbeingData()
        {
            IsBusy = true;
            var selectedDietItem = _wellbeingTypeList.FirstOrDefault(_ => _.IsChecked);

            if (selectedDietItem != null) WellbeingDataProxy.Rating = selectedDietItem.Key;
            WellbeingDataProxy.Created = DateTime.Now;

            // TODO to Allow multiple adding
            WellbeingDataProxy.Id = 0;

            var apiCallResult = await Provider.CreatWellbeingDataAsnyc(WellbeingDataProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation(
                    $"WellbeingData for Account: {WellbeingDataProxy.AccountId} successfully created.");

                WellbeingDataProxy = apiCallResult.Value;

                await Service.NavigationService.GoBackAsync();
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.WellbeingData_Title,
                    apiCallResult.ErrorMessage,
                    AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async void GetWellbeingDataAsync(DateTime targetDateTime)
        {
            //  IsBusy = true;
            ShowProgress = true;
            Provider.GetWellbeingDataByDateAsnyc(targetDateTime, _cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    Logger.LogInformation(
                        $" WellbeingData for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");

                    WellbeingDataProxy = apiCallResult.Result.Value;

                    Device.BeginInvokeOnMainThread(() =>
                    {
                        var rating = (WellbeingType)WellbeingDataProxy.Rating;
                        switch (rating)
                        {
                            case WellbeingType.Bad:
                                this.WellbeingTypeBad.IsChecked = true;
                                break;

                            case WellbeingType.VeryBad:
                                this.WellbeingTypeVeryBad.IsChecked = true;
                                break;

                            case WellbeingType.NotSoGreat:
                                this.WellbeingTypeNotSo.IsChecked = true;
                                break;

                            case WellbeingType.Great:
                                this.WellbeingTypeGreat.IsChecked = true;
                                break;

                            case WellbeingType.Fantastic:
                                this.WellbeingTypeFantastic.IsChecked = true;
                                break;
                        }
                    });
                    ShowProgress = false;
                }
                else
                {
                    if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                    {
                        ShowProgress = false;
                        return;
                    }

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Service.DialogService.DisplayAlertAsync(AppResources.WellbeingData_Title,
                        apiCallResult.Result.ErrorMessage,
                        AppResources.Button_Ok);
                    });

                    ShowProgress = false;
                    return;
                }
                ShowProgress = false;
            });
        }
    }
}