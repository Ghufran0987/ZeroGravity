using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;
using System.Linq;
using System.Collections.Generic;
using ZeroGravity.Mobile.Services;

namespace ZeroGravity.Mobile.ViewModels
{
    public class MyWeightPageViewModel : VmBase<IMyWeightPage, IMyWeightPageVmProvider, MyWeightPageViewModel>
    {

        private string _weightSubHeader;

        public string WeightSubHeader
        {
            get { return _weightSubHeader; }
            set { SetProperty(ref _weightSubHeader, value); }
        }


        private bool _isAddNewGoalActive;

        public bool IsAddNewGoalActive
        {
            get { return _isAddNewGoalActive; }
            set { SetProperty(ref _isAddNewGoalActive, value); }
        }


        private bool _isWeightGainProgram;

        public bool IsWeightGainProgram
        {
            get { return _isWeightGainProgram; }
            set { SetProperty(ref _isWeightGainProgram, value); }
        }


        private bool _isTargetAchieve;

        public bool IsTargetAchieve
        {
            get { return _isTargetAchieve; }
            set { SetProperty(ref _isTargetAchieve, value); }
        }


        private WeightItemDetailsProxy _myWeightItemDetails;

        public WeightItemDetailsProxy MyWeightItemDetails
        {
            get { return _myWeightItemDetails; }
            set { SetProperty(ref _myWeightItemDetails, value); }
        }

        private IWizardFinishSetupPageVmProvider _wizardFinishSetupPageVmProvider;
        #region Constructor

        public MyWeightPageViewModel(IVmCommonService service, IMyWeightPageVmProvider provider,
            ILoggerFactory loggerFactory, IApiService apiService, IWizardFinishSetupPageVmProvider wizardFinishSetupPageVmProvider) : base(service, provider, loggerFactory, apiService)
        {
            _wizardFinishSetupPageVmProvider = wizardFinishSetupPageVmProvider;
            AddWeightCommand = new DelegateCommand<object>(OnWeightCommand, CanWeightOpen);
            SaveWeightCommand = new DelegateCommand(OnSaveCommand);
            ReminderWeightCommand = new DelegateCommand(OnReminderWeight);
            AchiveWeightCommand = new DelegateCommand(OnAchiveWeight);
            CloseCommand = new DelegateCommand(OnClose);
            AddNewGoalCommand = new DelegateCommand(OnAddNewGoal);
            OpenAddNewGoalCommand = new DelegateCommand(OnOpenOpenAddNewGoal);
            Weight = new WeightItemProxy();
        }



        private void OnClose()
        {
            UpdateWeightPage(WeightPage.WeightActive);
        }

        private void OnAchiveWeight()
        {
            UpdateWeightPage(WeightPage.WeightActive);
            UpdateDetails(false);
        }

        private void OnReminderWeight()
        {
            UpdateWeightPage(WeightPage.WeightActive);
        }

        private void OnWeightCommand(object obj)
        {
            if (IsWeightGainProgram)
            {
                if (MyWeightItemDetails.TargetWeight <= MyWeightItemDetails.CurrentWeight)
                {
                    UpdateWeightPage(WeightPage.WeightAchive);
                    return;
                }
            }
            else
            {
                if (MyWeightItemDetails.CurrentWeight <= MyWeightItemDetails.TargetWeight)
                {
                    UpdateWeightPage(WeightPage.WeightAchive);
                    return;
                }
            }


            UpdateWeightPage(WeightPage.WeightSave);

        }

        private bool CanWeightOpen(object obj)
        {
            return true;
        }


        private string _achiveMessage;

        public string AchiveMessage
        {
            get { return _achiveMessage; }
            set { SetProperty(ref _achiveMessage, value); }
        }





        #endregion Constructor

        #region private Property

        private WeightItemProxy _weight;

        private ObservableCollection<WeightItemProxy> _weightsItemSource;
        private CancellationTokenSource _cts;

        #endregion private Property

        #region Public Property

        private WeightItemDetailsProxy _weightItemDetail;

        public WeightItemDetailsProxy WeightItemDetail
        {
            get { return _weightItemDetail; }
            set { SetProperty(ref _weightItemDetail, value); }
        }


        public WeightItemProxy Weight
        {
            get { return _weight; }
            set { _weight = value; }
        }

        public ObservableCollection<WeightItemProxy> WeightsItemSource
        {
            get { return _weightsItemSource; }
            set { SetProperty(ref _weightsItemSource, value); }
        }

        #endregion Public Property

        #region Command

        public DelegateCommand SaveWeightCommand { get; }

        public DelegateCommand<object> AddWeightCommand { get; private set; }

        public DelegateCommand ReminderWeightCommand { get; }

        public DelegateCommand AchiveWeightCommand { get; }

        public DelegateCommand CloseCommand { get; }

        public DelegateCommand AddNewGoalCommand { get; }

        public DelegateCommand OpenAddNewGoalCommand { get; }

        public DelegateCommand HistoryCommand { get; }
        #endregion Command

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);
            UpdateWeightPage(WeightPage.WeightActive);
            if (!HasInternetConnection)
            {
                return;
            }

            _cts = new CancellationTokenSource();
            UpdateDetails();
        }

        private GenderBiologicalType UserType { get; set; }

        private async void UpdateDetails(bool isfromSave = true, bool isfromSetGoal = false)
        {
            ShowProgress = true;
            await _wizardFinishSetupPageVmProvider.GetPersonalDataAsnyc(_cts.Token).ContinueWith(personal =>
             {
                 try
                 {
                     if (personal.Result.Success && personal.Result.Value != null)
                     {
                         UserType = personal.Result.Value.Gender;
                     }
                 }
                 catch (Exception ex)
                 {
                     Logger.LogError(ex.Message, ex);
                 }
             });

            if (UserType == GenderBiologicalType.Male)
            {
                WeightReminderImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_Reminder.png");
                WeightAchiveImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_Achive.png");
            }
            else
            {
                WeightReminderImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Reminder.png");
                WeightAchiveImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Achive.png");
            }


            Provider.GetCurrentWeightTrackerAsync(_cts.Token).ContinueWith(result =>
            {
                try
                {
                    if (result.Result.Success)
                    {
                        if (result.Result.Value == null)
                        {
                            MyWeightItemDetails = new WeightItemDetailsProxy();
                            if (UserType == GenderBiologicalType.Male)
                            {

                                WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_1.png");
                            }
                            else
                            {
                                WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_1.png");
                            }

                        }
                        else
                        {
                            MyWeightItemDetails = result.Result.Value;
                            WeightsItemSource = MyWeightItemDetails?.WeightItemProxies;
                            if (MyWeightItemDetails != null)
                            {
                                if (MyWeightItemDetails.TargetWeight > MyWeightItemDetails.InitialWeight)
                                {
                                    IsWeightGainProgram = true;
                                    WeightSubHeader = "Let's track your weight gain progress";
                                    ConvertWeightToWeightGainMilestone(MyWeightItemDetails.CurrentWeight, MyWeightItemDetails.TargetWeight, MyWeightItemDetails.InitialWeight, isfromSetGoal);
                                    CurrentWeightMilestoneProxy = WeightMilestoneProxies.FirstOrDefault(x => x.Status == MilestoneStatus.Active);

                                    if (CurrentWeightMilestoneProxy == null)
                                    {
                                        UpdateWeightGainImage(MyWeightItemDetails.CurrentWeight, MyWeightItemDetails.TargetWeight, MyWeightItemDetails.InitialWeight);
                                        IsAddNewGoalActive = true;
                                    }
                                    else
                                    {
                                        UpdateWeightGainImage(MyWeightItemDetails.CurrentWeight, CurrentWeightMilestoneProxy.MileStoneTargetWeight, CurrentWeightMilestoneProxy.MileStoneStartingWeight);
                                        IsAddNewGoalActive = false;
                                    }
                                }
                                else
                                {
                                    IsWeightGainProgram = false;
                                    WeightSubHeader = "Let's track your weight loss progress";
                                    ConvertweightToWeightLossMilestone(MyWeightItemDetails.CurrentWeight, MyWeightItemDetails.TargetWeight, MyWeightItemDetails.InitialWeight, isfromSetGoal);

                                    CurrentWeightMilestoneProxy = WeightMilestoneProxies.FirstOrDefault(x => x.Status == MilestoneStatus.Active);

                                    if (CurrentWeightMilestoneProxy == null)
                                    {
                                        UpdateWeightLossImage(MyWeightItemDetails.CurrentWeight, MyWeightItemDetails.TargetWeight, MyWeightItemDetails.InitialWeight);
                                        IsAddNewGoalActive = true;
                                    }
                                    else
                                    {
                                        UpdateWeightLossImage(MyWeightItemDetails.CurrentWeight, CurrentWeightMilestoneProxy.MileStoneTargetWeight, CurrentWeightMilestoneProxy.MileStoneStartingWeight);
                                        IsAddNewGoalActive = false;
                                    }
                                }
                            }

                            var maxWeightUpdateDate = MyWeightItemDetails.WeightItemProxies?.Max(x => x.Created);

                            if (maxWeightUpdateDate != null && maxWeightUpdateDate.Value < DateTime.Today.AddDays(-2) && isfromSave)
                            {
                                UpdateWeightPage(WeightPage.WeightReminder);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    ShowProgress = false;
                }
            });
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
        }

        #region Method


        private async void OnSaveCommand()
        {
            try
            {
                double weight = 0;
                if (Weight.GetUnit == "Lbs")
                {
                    weight = DisplayConversionService.ConvertPoundToKg(Weight.Value);
                }
                else
                {
                    weight = Weight.Value;
                }

                if (weight < 18.5)
                {
                    await Service.DialogService.DisplayAlertAsync(
                             "Weight Tracker",
                             "Our clinical experts do not recommend this is a healthy weight for you, based on the inputs so far",
                              AppResources.Button_Ok);
                    return;
                }

                IsBusy = true;
                _cts = new CancellationTokenSource();
                var isupdate = MyWeightItemDetails.WeightItemProxies.Any(x => x.Created.Date == DateTime.Today);
                var proxy = new WeightItemProxy
                {
                    AccountId = MyWeightItemDetails.AccountId,
                    Value = weight,
                    WeightTrackerId = MyWeightItemDetails.Id
                };



                if (isupdate)
                {
                    var updateproxy = MyWeightItemDetails.WeightItemProxies.FirstOrDefault(x => x.Created.Date == DateTime.Today);

                    if (Weight.GetUnit == "Lbs")
                    {
                        updateproxy.Value = DisplayConversionService.ConvertPoundToKg(Weight.Value);
                    }
                    else
                    {
                        updateproxy.Value = Weight.Value;
                    }

                    var apiCallResult = await Provider.AddWeightDataAsync(updateproxy, _cts.Token);

                    if (apiCallResult.Success)
                    {
                        Weight.Value = updateproxy.Value;
                        if (!IsWeightGainProgram && CurrentWeightMilestoneProxy?.MileStoneTargetWeight >= Weight.Value)
                        {
                            var currentMileStone = new WeightMilestoneProxy();
                            var runningWeightmileSton = WeightMilestoneProxies.FirstOrDefault(x => Weight.Value >= x.MileStoneTargetWeight && Weight.Value <= x.MileStoneStartingWeight);

                            if (runningWeightmileSton != null)
                            {
                                if (runningWeightmileSton.MileStoneTargetWeight == Weight.Value)
                                {
                                    currentMileStone = runningWeightmileSton;
                                }
                                else
                                {
                                    currentMileStone = WeightMilestoneProxies.FirstOrDefault(x => x.MileStoneNo == runningWeightmileSton.MileStoneNo - 1);
                                }
                            }

                            UpdateWeightPage(WeightPage.WeightAchive, currentMileStone.MileStoneNo == 0 ? null : currentMileStone);
                        }
                        else if (IsWeightGainProgram && CurrentWeightMilestoneProxy?.MileStoneTargetWeight <= Weight.Value)
                        {
                            var currentMileStone = new WeightMilestoneProxy();
                            var runningWeightmileSton = WeightMilestoneProxies.FirstOrDefault(x => Weight.Value <= x.MileStoneTargetWeight && Weight.Value >= x.MileStoneStartingWeight);

                            if (runningWeightmileSton != null)
                            {
                                if (runningWeightmileSton.MileStoneTargetWeight == Weight.Value)
                                {
                                    currentMileStone = runningWeightmileSton;
                                }
                                else
                                {
                                    currentMileStone = WeightMilestoneProxies.FirstOrDefault(x => x.MileStoneNo == runningWeightmileSton.MileStoneNo - 1);
                                }
                            }

                            UpdateWeightPage(WeightPage.WeightAchive, currentMileStone.MileStoneNo == 0 ? null : currentMileStone);
                        }
                        else
                        {
                            UpdateWeightPage(WeightPage.WeightActive);
                            UpdateDetails(false);
                        }
                        Weight.Value = 0;
                    }
                    else
                    {
                        if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                            apiCallResult.ErrorReason == ErrorReason.TimeOut)
                            return;
                        IsBusy = false;
                    }
                }
                else
                {
                    proxy.Created = DateTime.Now;
                    proxy.Id = 0;
                    var apiCallResult = await Provider.AddWeightDataAsync(proxy, _cts.Token);
                    if (apiCallResult.Success)
                    {
                        Weight.Value = proxy.Value;
                        if (!IsWeightGainProgram && CurrentWeightMilestoneProxy?.MileStoneTargetWeight >= Weight.Value)
                        {
                            UpdateWeightPage(WeightPage.WeightAchive, CurrentWeightMilestoneProxy);
                        }
                        else if (IsWeightGainProgram && CurrentWeightMilestoneProxy?.MileStoneTargetWeight <= Weight.Value)
                        {
                            UpdateWeightPage(WeightPage.WeightAchive, CurrentWeightMilestoneProxy);
                        }
                        else
                        {
                            UpdateWeightPage(WeightPage.WeightActive);
                            UpdateDetails(false);
                        }
                        Weight.Value = 0;
                    }
                    else
                    {
                        if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation ||
                            apiCallResult.ErrorReason == ErrorReason.TimeOut)
                            return;
                        IsBusy = false;
                    }
                }
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

        private ImageSource _weightImageSource;

        public ImageSource WeightImageSource
        {
            get => _weightImageSource;
            set => SetProperty(ref _weightImageSource, value);
        }



        private ImageSource _weightReminderImageSource;

        public ImageSource WeightReminderImageSource
        {
            get => _weightReminderImageSource;
            set => SetProperty(ref _weightReminderImageSource, value);
        }

        private ImageSource _weightAchiveImageSource;

        public ImageSource WeightAchiveImageSource
        {
            get => _weightAchiveImageSource;
            set => SetProperty(ref _weightAchiveImageSource, value);
        }


        private void UpdateWeightLossImage(double currentWeight, double targetWeight, double startingWeight)
        {
            if (targetWeight > currentWeight)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_6.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_6.png");
                return;
            }

            var diffWeight = startingWeight - targetWeight;
            var divation = diffWeight / 5;

            if (currentWeight >= startingWeight)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_1.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_1.png");
            }
            else if (currentWeight < startingWeight && currentWeight >= startingWeight - divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_1.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_1.png");
            }
            else if (currentWeight < startingWeight - divation && currentWeight >= startingWeight - 2 * divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_2.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_2.png");
            }
            else if (currentWeight < startingWeight - 2 * divation && currentWeight >= startingWeight - 3 * divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_3.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_3.png");
            }
            else if (currentWeight < startingWeight - 3 * divation && currentWeight >= startingWeight - 4 * divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_4.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_4.png");
            }
            else if (currentWeight < startingWeight - 4 * divation && currentWeight >= startingWeight - 5 * divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_5.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_5.png");
            }
            else if (currentWeight >= startingWeight - 5 * divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_6.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_6.png");
            }
            else
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Male_6.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_6.png");
            }
        }

        private ObservableCollection<WeightMilestoneProxy> _weightMilestoneProxies;

        public ObservableCollection<WeightMilestoneProxy> WeightMilestoneProxies
        {
            get => _weightMilestoneProxies;
            set => SetProperty(ref _weightMilestoneProxies, value);
        }

        private WeightMilestoneProxy _currentWeightMilestoneProxy;

        public WeightMilestoneProxy CurrentWeightMilestoneProxy
        {
            get => _currentWeightMilestoneProxy;
            set => SetProperty(ref _currentWeightMilestoneProxy, value);
        }



        private void ConvertweightToWeightLossMilestone(double currentWeight, double targetWeight, double startingWeight, bool isfromSetGoal)
        {
            var deviation = startingWeight - targetWeight;
            var no = deviation % 3;
            var count = deviation / 3;
            if (WeightMilestoneProxies == null || isfromSetGoal)
            {
                WeightMilestoneProxies = new ObservableCollection<WeightMilestoneProxy>();
                var weightMilestoneProxieList = new List<WeightMilestoneProxy>();
                if (no == 0)
                {
                    var mileStoneDeviation = deviation / count;
                    var startingMileStone = startingWeight;
                    for (int i = 1; i <= count; i++)
                    {
                        var weightMilestoneProxy = new WeightMilestoneProxy();
                        weightMilestoneProxy.MileStoneNo = i;
                        weightMilestoneProxy.MileStoneStartingWeight = startingMileStone;
                        weightMilestoneProxy.MileStoneTargetWeight = startingMileStone - mileStoneDeviation;
                        startingMileStone = startingMileStone - mileStoneDeviation;

                        if (weightMilestoneProxy.MileStoneNo == 1)
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 25, 0, 0);
                        }
                        else if (weightMilestoneProxy.MileStoneNo == count)
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 0, 0, 60);
                        }
                        else
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 0, 0, 0);
                        }

                        UpdateLossMilestoneStatusAtEqualDeviation(currentWeight, count, mileStoneDeviation, weightMilestoneProxy);
                        weightMilestoneProxieList.Add(weightMilestoneProxy);
                    }
                }
                else
                {
                    var mileStoneDeviation = deviation / ((int)count + 1);
                    var startingMileStone = startingWeight;
                    for (int i = 1; i <= count + 1; i++)
                    {
                        var weightMilestoneProxy = new WeightMilestoneProxy();
                        weightMilestoneProxy.MileStoneNo = i;
                        weightMilestoneProxy.MileStoneStartingWeight = startingMileStone;
                        weightMilestoneProxy.MileStoneTargetWeight = startingMileStone - mileStoneDeviation;
                        startingMileStone = startingMileStone - mileStoneDeviation;
                        if (weightMilestoneProxy.MileStoneNo == 1)
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 25, 0, 0);
                        }
                        else if (weightMilestoneProxy.MileStoneNo < count + 1 && weightMilestoneProxy.MileStoneNo > count)
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 0, 0, 60);
                        }
                        else
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 0, 0, 0);
                        }

                        UpdateLossMilestoneStatusAtNotEqualDeviation(currentWeight, count, mileStoneDeviation, weightMilestoneProxy);
                        weightMilestoneProxieList.Add(weightMilestoneProxy);
                    }
                }
                WeightMilestoneProxies = new ObservableCollection<WeightMilestoneProxy>(weightMilestoneProxieList.OrderBy(x => x.MileStoneNo).ToList());
            }
            else
            {
                if (no == 0)
                {
                    var mileStoneDeviation = deviation / count;
                    foreach (var item in WeightMilestoneProxies)
                    {
                        UpdateLossMilestoneStatusAtEqualDeviation(currentWeight, count, mileStoneDeviation, item);
                    }
                }
                else
                {
                    var mileStoneDeviation = deviation / ((int)count + 1);
                    foreach (var item in WeightMilestoneProxies)
                    {
                        UpdateLossMilestoneStatusAtNotEqualDeviation(currentWeight, count, mileStoneDeviation, item);
                    }
                }
            }
        }

        private void UpdateLossMilestoneStatusAtNotEqualDeviation(double currentWeight, double count, double mileStoneDeviation, WeightMilestoneProxy weightMilestoneProxy)
        {
            if (weightMilestoneProxy.MileStoneTargetWeight >= currentWeight)
            {
                weightMilestoneProxy.Status = MilestoneStatus.Completed;
                weightMilestoneProxy.IsMileStoneCompletedVisible = true;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = false;


                weightMilestoneProxy.MilestoneCongMesg = "Well Done.";
                if (weightMilestoneProxy.MileStoneNo < count + 1 && weightMilestoneProxy.MileStoneNo > count)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Congratulations.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#319C8A");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've lost " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit + " and achiveved your target weight.";
                    weightMilestoneProxy.MileStoneHeight = new GridLength(70);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_Completed.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStoneCompleted.png");
                    }
                }
                else if (weightMilestoneProxy.MileStoneNo < count && weightMilestoneProxy.MileStoneNo > count - 1)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Almost there.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've lost " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_3.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone3.png");
                    }
                }
                else if (weightMilestoneProxy.MileStoneNo < count - 1 && weightMilestoneProxy.MileStoneNo > count - 2)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Great job.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've lost " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_2.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone2.png");
                    }
                }
                else
                {
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've lost " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_1.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone1.png");
                    }
                }

            }
            else if (currentWeight > weightMilestoneProxy.MileStoneTargetWeight && currentWeight <= weightMilestoneProxy.MileStoneStartingWeight)
            {
                weightMilestoneProxy.Status = MilestoneStatus.Active;
                weightMilestoneProxy.IsMileStoneCompletedVisible = false;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = true;
                weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                weightMilestoneProxy.MilestoneWeightMesg = "Goal is to lose " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
            }
            else
            {
                weightMilestoneProxy.Status = MilestoneStatus.InActive;
                weightMilestoneProxy.IsMileStoneCompletedVisible = false;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = true;
                weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                weightMilestoneProxy.MilestoneWeightMesg = "Goal is to lose " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
            }
        }

        private void UpdateLossMilestoneStatusAtEqualDeviation(double currentWeight, double count, double mileStoneDeviation, WeightMilestoneProxy weightMilestoneProxy)
        {
            if (weightMilestoneProxy.MileStoneTargetWeight >= currentWeight)
            {
                weightMilestoneProxy.Status = MilestoneStatus.Completed;
                weightMilestoneProxy.IsMileStoneCompletedVisible = true;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = false;


                weightMilestoneProxy.MilestoneCongMesg = "Well Done.";
                if (weightMilestoneProxy.MileStoneNo == count)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Congratulations.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#319C8A");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've lost " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit + " and achiveved your target weight!";
                    weightMilestoneProxy.MileStoneHeight = new GridLength(70);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_Completed.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStoneCompleted.png");
                    }
                }
                else if (weightMilestoneProxy.MileStoneNo == count - 1)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Almost there.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've lost " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_3.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone3.png");
                    }
                }
                else if (weightMilestoneProxy.MileStoneNo == count - 2)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Great job.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've lost " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_2.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone2.png");
                    }
                }
                else
                {
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've lost " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_1.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone1.png");
                    }
                }

            }
            else if (currentWeight > weightMilestoneProxy.MileStoneTargetWeight && currentWeight <= weightMilestoneProxy.MileStoneStartingWeight)
            {
                weightMilestoneProxy.Status = MilestoneStatus.Active;
                weightMilestoneProxy.IsMileStoneCompletedVisible = false;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = true;
                weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                weightMilestoneProxy.MilestoneWeightMesg = "Goal is to lose " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
            }
            else
            {
                weightMilestoneProxy.Status = MilestoneStatus.InActive;
                weightMilestoneProxy.IsMileStoneCompletedVisible = false;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = true;
                weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                weightMilestoneProxy.MilestoneWeightMesg = "Goal is to lose " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
            }
        }

        private string GetMileStoneDeviation(double mileStone)
        {

            if (MyWeightItemDetails?.DisplayUnit == "Pound")
            {
                return DisplayConversionService.ConvertKgToPound(mileStone).ToString("0.0");
            }
            else
            {
                return mileStone.ToString("0.0");
            }

        }
        private void UpdateWeightPage(WeightPage weightPage, WeightMilestoneProxy weightMilestoneProxy = null)
        {
            switch (weightPage)
            {
                case WeightPage.WeightActive:
                    IsWeightActive = true;
                    IsWeightSave = false;
                    IsWeightAchive = false;
                    IsWeightReminder = false;
                    IsWeightAddActive = false;
                    break;

                case WeightPage.WeightSave:
                    IsWeightActive = false;
                    IsWeightSave = true;
                    IsWeightAchive = false;
                    IsWeightReminder = false;
                    IsWeightAddActive = false;
                    break;

                case WeightPage.WeightAchive:
                    IsWeightActive = false;
                    IsWeightSave = false;
                    IsWeightReminder = false;
                    IsWeightAddActive = false;
                    IsWeightAchive = true;
                    if (weightMilestoneProxy != null && weightMilestoneProxy.MileStoneNo != WeightMilestoneProxies?.Max(x => x.MileStoneNo))
                    {
                        AchiveMessage = string.Format("Well done - Milestone{0} completed", weightMilestoneProxy.MileStoneNo);
                    }
                    else
                    {
                        AchiveMessage = "Well done - keep up the good work";
                    }
                    break;

                case WeightPage.WeightReminder:
                    IsWeightActive = false;
                    IsWeightSave = false;
                    IsWeightAchive = false;
                    IsWeightAddActive = false;
                    IsWeightReminder = true;
                    break;
                case WeightPage.WeightAdd:
                    IsWeightAddActive = true;
                    IsWeightActive = false;
                    IsWeightSave = false;
                    IsWeightAchive = false;
                    IsWeightReminder = false;
                    WeightItemDetail = new WeightItemDetailsProxy();
                    WeightItemDetail.InitialWeight = MyWeightItemDetails.DisplayUnit == "Pound" ? Math.Round(DisplayConversionService.ConvertKgToPound(MyWeightItemDetails.CurrentWeight), 2) : Math.Round(MyWeightItemDetails.CurrentWeight, 2);
                    break;
            }
        }

        #endregion Method

        protected override void OnCustomCloseOverlay()
        {
            base.OnCustomCloseOverlay();
        }

        internal override void OnDailyCloseOverlay()
        {
            base.OnDailyCloseOverlay();
        }

        private bool _isWeightActive;
        private bool _isWeightSave;
        private bool _isWeightAchieve;
        private bool _isWeightReminder;
        private bool _isWeightAddActive;

        public bool IsWeightAddActive
        {
            get { return _isWeightAddActive; }
            set { SetProperty(ref _isWeightAddActive, value); }
        }

        public bool IsWeightActive
        {
            get { return _isWeightActive; }
            set { SetProperty(ref _isWeightActive, value); }
        }
        public bool IsWeightSave
        {
            get { return _isWeightSave; }
            set { SetProperty(ref _isWeightSave, value); }
        }

        public bool IsWeightAchive
        {
            get { return _isWeightAchieve; }
            set { SetProperty(ref _isWeightAchieve, value); }
        }
        public bool IsWeightReminder
        {
            get { return _isWeightReminder; }
            set { SetProperty(ref _isWeightReminder, value); }
        }



        private void ConvertWeightToWeightGainMilestone(double currentWeight, double targetWeight, double startingWeight, bool isfromSetGoal)
        {
            var deviation = targetWeight - startingWeight;
            var no = deviation % 3;
            var count = deviation / 3;

            if (WeightMilestoneProxies == null || isfromSetGoal)
            {
                WeightMilestoneProxies = new ObservableCollection<WeightMilestoneProxy>();
                var weightMilestoneProxieList = new List<WeightMilestoneProxy>();
                if (no == 0)
                {
                    var mileStoneDeviation = deviation / count;
                    var startingMileStone = startingWeight;
                    for (int i = 1; i <= count; i++)
                    {
                        var weightMilestoneProxy = new WeightMilestoneProxy();
                        weightMilestoneProxy.MileStoneNo = i;
                        weightMilestoneProxy.MileStoneStartingWeight = startingMileStone;
                        weightMilestoneProxy.MileStoneTargetWeight = startingMileStone + mileStoneDeviation;
                        startingMileStone = startingMileStone + mileStoneDeviation;

                        if (weightMilestoneProxy.MileStoneNo == 1)
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 25, 0, 0);
                        }
                        else if (weightMilestoneProxy.MileStoneNo == count)
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 0, 0, 60);
                        }
                        else
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 0, 0, 0);
                        }

                        UpdateGainMilestoneStatusAtEqualDeviation(currentWeight, count, mileStoneDeviation, weightMilestoneProxy);
                        weightMilestoneProxieList.Add(weightMilestoneProxy);
                    }
                }
                else
                {
                    var mileStoneDeviation = deviation / ((int)count + 1);
                    var startingMileStone = startingWeight;
                    for (int i = 1; i <= count + 1; i++)
                    {
                        var weightMilestoneProxy = new WeightMilestoneProxy();
                        weightMilestoneProxy.MileStoneNo = i;
                        weightMilestoneProxy.MileStoneStartingWeight = startingMileStone;
                        weightMilestoneProxy.MileStoneTargetWeight = startingMileStone + mileStoneDeviation;
                        startingMileStone = startingMileStone + mileStoneDeviation;
                        if (weightMilestoneProxy.MileStoneNo == 1)
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 25, 0, 0);
                        }
                        else if (weightMilestoneProxy.MileStoneNo < count + 1 && weightMilestoneProxy.MileStoneNo > count)
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 0, 0, 60);
                        }
                        else
                        {
                            weightMilestoneProxy.SideBorderThickness = new Thickness(0, 0, 0, 0);
                        }

                        UpdateGainMilestoneStatusAtNotEqualDeviation(currentWeight, count, mileStoneDeviation, weightMilestoneProxy);
                        weightMilestoneProxieList.Add(weightMilestoneProxy);
                    }
                }

                WeightMilestoneProxies = new ObservableCollection<WeightMilestoneProxy>(weightMilestoneProxieList.OrderBy(x => x.MileStoneNo).ToList());
            }
            else
            {
                if (no == 0)
                {
                    var mileStoneDeviation = deviation / count;
                    foreach (var item in WeightMilestoneProxies)
                    {
                        UpdateGainMilestoneStatusAtEqualDeviation(currentWeight, count, mileStoneDeviation, item);
                    }
                }
                else
                {
                    var mileStoneDeviation = deviation / ((int)count + 1);
                    foreach (var item in WeightMilestoneProxies)
                    {
                        UpdateGainMilestoneStatusAtNotEqualDeviation(currentWeight, count, mileStoneDeviation, item);
                    }
                }
            }
        }

        private void UpdateGainMilestoneStatusAtEqualDeviation(double currentWeight, double count, double mileStoneDeviation, WeightMilestoneProxy weightMilestoneProxy)
        {
            if (weightMilestoneProxy.MileStoneTargetWeight <= currentWeight)
            {
                weightMilestoneProxy.Status = MilestoneStatus.Completed;
                weightMilestoneProxy.IsMileStoneCompletedVisible = true;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = false;


                weightMilestoneProxy.MilestoneCongMesg = "Well Done.";
                if (weightMilestoneProxy.MileStoneNo == count)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Congratulations.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#319C8A");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit + " and achiveved your target weight!";
                    weightMilestoneProxy.MileStoneHeight = new GridLength(70);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_Completed.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStoneCompleted.png");
                    }
                }
                else if (weightMilestoneProxy.MileStoneNo == count - 1)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Almost there.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_3.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone3.png");
                    }
                }
                else if (weightMilestoneProxy.MileStoneNo == count - 2)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Great job.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_2.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone2.png");
                    }
                }
                else
                {
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_1.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone1.png");
                    }
                }

            }
            else if (currentWeight < weightMilestoneProxy.MileStoneTargetWeight && currentWeight >= weightMilestoneProxy.MileStoneStartingWeight)
            {
                weightMilestoneProxy.Status = MilestoneStatus.Active;
                weightMilestoneProxy.IsMileStoneCompletedVisible = false;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = true;
                weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                weightMilestoneProxy.MilestoneWeightMesg = "Goal is to gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
            }
            else
            {
                weightMilestoneProxy.Status = MilestoneStatus.InActive;
                weightMilestoneProxy.IsMileStoneCompletedVisible = false;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = true;
                weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                weightMilestoneProxy.MilestoneWeightMesg = "Goal is to gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
            }
        }

        private void UpdateGainMilestoneStatusAtNotEqualDeviation(double currentWeight, double count, double mileStoneDeviation, WeightMilestoneProxy weightMilestoneProxy)
        {
            if (weightMilestoneProxy.MileStoneTargetWeight <= currentWeight)
            {
                weightMilestoneProxy.Status = MilestoneStatus.Completed;
                weightMilestoneProxy.IsMileStoneCompletedVisible = true;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = false;


                weightMilestoneProxy.MilestoneCongMesg = "Well Done.";
                if (weightMilestoneProxy.MileStoneNo < count + 1 && weightMilestoneProxy.MileStoneNo > count)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Congratulations.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#319C8A");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit + " and achiveved your target weight.";
                    weightMilestoneProxy.MileStoneHeight = new GridLength(70);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_Completed.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStoneCompleted.png");
                    }
                }
                else if (weightMilestoneProxy.MileStoneNo < count && weightMilestoneProxy.MileStoneNo > count - 1)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Almost there.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_3.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone3.png");
                    }
                }
                else if (weightMilestoneProxy.MileStoneNo < count - 1 && weightMilestoneProxy.MileStoneNo > count - 2)
                {
                    weightMilestoneProxy.MilestoneCongMesg = "Great job.";
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_2.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone2.png");
                    }
                }
                else
                {
                    weightMilestoneProxy.MilestoneBackground = Color.FromHex("#39F");
                    weightMilestoneProxy.MilestoneWeightMesg = "you've gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
                    weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                    if (UserType == GenderBiologicalType.Male)
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone_male_1.png");
                    }
                    else
                    {
                        weightMilestoneProxy.MileStoneImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.MileStone1.png");
                    }
                }

            }
            else if (currentWeight < weightMilestoneProxy.MileStoneTargetWeight && currentWeight >= weightMilestoneProxy.MileStoneStartingWeight)
            {
                weightMilestoneProxy.Status = MilestoneStatus.Active;
                weightMilestoneProxy.IsMileStoneCompletedVisible = false;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = true;
                weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                weightMilestoneProxy.MilestoneWeightMesg = "Goal is to gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
            }
            else
            {
                weightMilestoneProxy.Status = MilestoneStatus.InActive;
                weightMilestoneProxy.IsMileStoneCompletedVisible = false;
                weightMilestoneProxy.IsMileStoneInCompletedVisible = true;
                weightMilestoneProxy.MileStoneHeight = new GridLength(50);
                weightMilestoneProxy.MilestoneWeightMesg = "Goal is to gain " + GetMileStoneDeviation(mileStoneDeviation) + MyWeightItemDetails.DisplayUnit;
            }
        }

        private void UpdateWeightGainImage(double currentWeight, double targetWeight, double startingWeight)
        {
            if (currentWeight > targetWeight)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Male_6.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Female_6.png");
                return;
            }

            var diffWeight = targetWeight - startingWeight;
            var divation = diffWeight / 5;

            if (currentWeight < startingWeight)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Male_1.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Female_1.png");
            }
            else if (currentWeight >= startingWeight && currentWeight < startingWeight + divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Male_1.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Female_1.png");
            }
            else if (currentWeight >= startingWeight + divation && currentWeight < startingWeight + 2 * divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Male_2.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Female_2.png");
            }
            else if (currentWeight >= startingWeight + 2 * divation && currentWeight < startingWeight + 3 * divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Male_3.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Female_3.png");
            }
            else if (currentWeight >= startingWeight + 3 * divation && currentWeight < startingWeight + 4 * divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Male_4.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Female_4.png");
            }
            else if (currentWeight >= startingWeight + 4 * divation && currentWeight < startingWeight + 5 * divation)
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Male_5.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Female_5.png");
            }
            else
            {
                if (UserType == GenderBiologicalType.Male)
                {
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Male_6.png");
                }
                else
                    WeightImageSource = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Weight_Gain_Female_6.png");
            }
        }

        private void OnOpenOpenAddNewGoal()
        {
            UpdateWeightPage(WeightPage.WeightAdd);
        }

        private async void OnAddNewGoal()
        {
            try
            {
                WeightItemDetail.AccountId = MyWeightItemDetails.AccountId;
                WeightItemDetail.DisplayUnit = MyWeightItemDetails.DisplayUnit;

                double initialWeight = 0;
                double targetWeight = 0;
                if (WeightItemDetail.DisplayUnit == "Pound")
                {
                    initialWeight = DisplayConversionService.ConvertPoundToKg(WeightItemDetail.InitialWeight);
                    targetWeight = DisplayConversionService.ConvertPoundToKg(WeightItemDetail.TargetWeight);
                }
                else
                {
                    initialWeight = WeightItemDetail.InitialWeight;
                    targetWeight = WeightItemDetail.TargetWeight;
                }
                WeightItemDetail.InitialWeight = initialWeight;
                WeightItemDetail.TargetWeight = targetWeight;
                WeightItemDetail.CurrentWeight = initialWeight;
                WeightItemDetail.Created = DateTime.Now;

                if (initialWeight < 18.5 || targetWeight < 18.5)
                {
                    await Service.DialogService.DisplayAlertAsync(
                             "Weight Tracker",
                             "Our clinical experts do not recommend this is a healthy weight for you, based on the inputs so far",
                              AppResources.Button_Ok);
                    return;
                }

                var apiResult = await Provider.AddWeightDetailsDataAsync(WeightItemDetail, _cts.Token);
                if (apiResult.Success)
                {
                    UpdateWeightPage(WeightPage.WeightActive);
                    UpdateDetails(false, true);
                }

            }
            catch (Exception e)
            {
                Logger.LogError(e, e.Message);
                await Service.DialogService.DisplayAlertAsync(AppResources.Common_Error, e.Message,
                    AppResources.Button_Ok);
            }

        }


    }
}