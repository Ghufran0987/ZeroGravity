using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Mobile.Contract;
using Prism.Events;
using ZeroGravity.Mobile.Events;

namespace ZeroGravity.Mobile.ViewModels
{
    public class MetabolicHealthPageViewModel : VmBase<IMetabolicHealthPage, IMetabolicHealthPageVmProvider, MetabolicHealthPageViewModel>
    {
        private CancellationTokenSource _cts;
        private DateTime _actualDateTime;
        private IEnumerable<ZgButtonGroupItem> _groupItems;
        private readonly IEventAggregator _eventAggregator;
        public DelegateCommand ManualCommand { get; }
        public DelegateCommand SugarBeatCommand { get; }
        public DelegateCommand SaveCommand { get; }

        public MetabolicHealthPageViewModel(IVmCommonService service, IMetabolicHealthPageVmProvider provider, ILoggerFactory loggerFactory, IEventAggregator eventAggregator) : base(service, provider, loggerFactory)
        {

            _eventAggregator = eventAggregator;
                
            ManualCommand = new DelegateCommand(() =>
            {
                IsManual = true;
                IsSugarBeat = !IsManual;
            });

            SugarBeatCommand = new DelegateCommand(() =>
            {
                IsManual = false;
                IsSugarBeat = !IsManual;

                var navigationParams = new PageNavigationParams
                {
                   // DateTime = CurrentDateTime,
                    //PageName = ViewName.SugarBeatMainPage // placeholder page

                    PageName = ViewName.SugarBeatScanPage
                };

                _eventAggregator.GetEvent<PageNavigationEvent>().Publish(navigationParams);

            });

            SaveCommand = new DelegateCommand(SaveExecute);

            PlaceholderImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.glucose.png");
        }



        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            // temporary
            var navParamsTemp = NavigationParametersHelper.GetNavigationParameters<MetabolicHealthNavParams>(parameters);
            if (navParamsTemp == null)
            {
                _actualDateTime = DateTime.Now;
            }
            else
            {
                _actualDateTime = navParamsTemp.TargetDateTime.Date;
                _actualDateTime = _actualDateTime.Add(DateTime.Now.TimeOfDay);
            }

            Title = DateTimeHelper.ToLocalDateZeroGravityFormat(_actualDateTime.Date);

            //move the overlay logic to last position after removing the return statement below
            //var showOverlay = Service.HoldingPagesSettingsService.ShouldShow(HoldingPageType.MetabolicHealth);
            //if (showOverlay)
            //{
            //    OpenOverlay();
            //} 

            //return; // hide current feature

          //  IsBusy = true;

            //if (!await ValidateToken())
            //{
            //    return;
            //}

            var navParams = NavigationParametersHelper.GetNavigationParameters<MetabolicHealthNavParams>(parameters);
            if (navParams == null)
            {
                _actualDateTime = DateTime.Now;
            }
            else
            {
                _actualDateTime = navParams.TargetDateTime.Date;
                _actualDateTime = _actualDateTime.Add(DateTime.Now.TimeOfDay);
            }

            Title = DateTimeHelper.ToLocalDateZeroGravityFormat(_actualDateTime.Date);

            InitSwitchButton();

            IsManual = true;
            IsSugarBeat = !IsManual;

            CreateManualGlucoseProxy(_actualDateTime);

            IsBusy = false;
        }

        protected override void OnCustomCloseOverlay()
        {
            base.OnCustomCloseOverlay();
            Service.HoldingPagesSettingsService.DoNotShowAgain(HoldingPageType.MetabolicHealth);
        }

        private void InitSwitchButton()
        {
            var buttonManual = new ZgButtonGroupItem { Command = ManualCommand, Label = AppResources.Button_Manual };
            var buttonSugarBeat = new ZgButtonGroupItem { Command = SugarBeatCommand, Label = "SugarBEAT" };

            GroupItems = new List<ZgButtonGroupItem>
            {
                buttonManual,
                buttonSugarBeat
            };

            SelectedGroupItem = buttonManual;
        }

        private void CreateManualGlucoseProxy(DateTime dateTime)
        {
            MinGlucoseValue = GlucoseConstants.MinGlucoseValue;
            MaxGlucoseValue = GlucoseConstants.MaxGlucoseValue;

            GlucoseManualProxy = new GlucoseManualProxy
            {
                DateTime = dateTime.Date,
                MeasurementTime = dateTime.TimeOfDay,
            };

            GlucoseValue = 5.5;
        }

        private async void SaveExecute()
        {
            IsBusy = true;

            _cts = new CancellationTokenSource();

            if (!await ValidateToken())
            {
                return;
            }

            var glucoseValue = GlucoseValue;
            var glucoseValueRounded = Math.Round(glucoseValue, 1); // runden auf eine Nachkommastelle

            GlucoseManualProxy.Glucose = glucoseValueRounded;

            var result = await Provider.SaveGlucoseAsync(GlucoseManualProxy, _cts.Token);

            //await CreateSugarBeatSampleData(DateTime.Now); // ToDo: Löschen wenn nicht mehr benötigt

            IsBusy = false;

            if (!result.Success)
            {
                Logger.LogError(result.ErrorMessage);

                if (result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || result.ErrorReason == ErrorReason.TimeOut)
                {
                    // don't show any Dialog
                    return;
                }
                
                await Service.DialogService.DisplayAlertAsync(AppResources.Activities_Title, result.ErrorMessage, AppResources.Button_Ok);
            }
            else
            {
                // back to "My Day"
                await Service.NavigationService.GoBackAsync();
            }
        }

        // Beispiel für später
        private async Task CreateSugarBeatSampleData(DateTime dateTime)
        {
            var sugarBeatGlucoseProxy = new SugarBeatGlucoseProxy
            {
                DateTime = dateTime.Date,
                Glucose = 5.7,
                Battery = 15
            };

            var result = await Provider.SaveGlucoseAsync(sugarBeatGlucoseProxy, _cts.Token);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
            CancelPendingRequest(_cts);
        }

        private bool _isManual;
        
        public bool IsManual
        {
            get => _isManual;
            set => SetProperty(ref _isManual, value);
        }

        private bool _isSugarBeat;
        public bool IsSugarBeat
        {
            get => _isSugarBeat;
            set => SetProperty(ref _isSugarBeat, value);
        }

        
        private double _minGlucoseValue;
        public double MinGlucoseValue
        {
            get => _minGlucoseValue;
            set => SetProperty(ref _minGlucoseValue, value);
        }

        private double _maxGlucoseValue;
        public double MaxGlucoseValue
        {
            get => _maxGlucoseValue;
            set => SetProperty(ref _maxGlucoseValue, value);
        }

        private double _glucoseValue;

        public double GlucoseValue
        {
            get => _glucoseValue;
            set => SetProperty(ref _glucoseValue, value);
        }

        private GlucoseManualProxy _glucoseManualProxy;

        public GlucoseManualProxy GlucoseManualProxy
        {
            get => _glucoseManualProxy;
            set => SetProperty(ref _glucoseManualProxy, value);
        }

        public IEnumerable<ZgButtonGroupItem> GroupItems
        {
            get => _groupItems;
            set => SetProperty(ref _groupItems, value);
        }

        private ZgButtonGroupItem _selectedGroupItem;
        public ZgButtonGroupItem SelectedGroupItem
        {
            get => _selectedGroupItem;
            set => SetProperty(ref _selectedGroupItem, value);
        }

        private ImageSource _placeholderImageSource;
        public ImageSource PlaceholderImageSource
        {
            get => _placeholderImageSource;
            set => SetProperty(ref _placeholderImageSource, value);
        }

    }
}
