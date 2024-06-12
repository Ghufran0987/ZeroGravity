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
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class MedicalPreConditionsPageViewModel : VmBase<IMedicalPreConditionsPage, IMedicalPreConditionsPageVmProvider, MedicalPreConditionsPageViewModel>
    {
        public DelegateCommand SelectCardiacConditionCommand { get; }
        public DelegateCommand SelectArthritisCommand { get; }
        public DelegateCommand SelectHypertensionCommand { get; }
        public DelegateCommand SelectDiabetesCommand { get; }
        public DelegateCommand SelectOtherCommand { get; }
        public DelegateCommand SelectDontwanttoSayCommand { get; }

        public DelegateCommand SaveMedicalConditionsDelegateCommand { get; }

        public MedicalPreConditionsPageViewModel(IVmCommonService service, IMedicalPreConditionsPageVmProvider provider, ILoggerFactory loggerFactory) : base(service, provider, loggerFactory)
        {
            SelectDiabetesCommand = new DelegateCommand(SelectDiabetes);
            SelectCardiacConditionCommand = new DelegateCommand(SelectCardiacCondition);
            SelectArthritisCommand = new DelegateCommand(SelectArthritis);
            SelectHypertensionCommand = new DelegateCommand(SelectHypertension);

            SelectOtherCommand = new DelegateCommand(SelectOther);
            SelectDontwanttoSayCommand = new DelegateCommand(SelectDontWantToSay);

            DiabetesItemList = new List<RadioButtonItemProxy>();

            SaveMedicalConditionsDelegateCommand = new DelegateCommand(SaveMedicalConditionsExecute);

            DiabetesType1 = new RadioButtonItemProxy
            {
                Key = (int)DiabetesType.Type1,
                IsChecked = true
            };

            DiabetesType2Insulin = new RadioButtonItemProxy
            {
                Key = (int)DiabetesType.Type2Insulin
            };

            DiabetesType2Diet = new RadioButtonItemProxy
            {
                Key = (int)DiabetesType.Type2DietControl
            };

            DiabetesType2OralMedication = new RadioButtonItemProxy
            {
                Key = (int)DiabetesType.Type2OralMedication
            };

            DiabetesPreDiabetic = new RadioButtonItemProxy
            {
                Key = (int)DiabetesType.PreDiabetic
            };

            DiabetesItemList.AddRange(new[]
            {
                DiabetesType1, DiabetesType2Insulin, DiabetesType2OralMedication, DiabetesType2Diet, DiabetesPreDiabetic
            });
        }

       
        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            try
            {
                //var isAuthorized = await ValidateToken();
                //if (isAuthorized) await

                LoadMedicalConditions();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task LoadMedicalConditions()
        {
            if (!HasInternetConnection)
            {
                return;
            }

            //IsBusy = true;

            // await ValidateToken();
            MedicalPreconditionsProxy = new MedicalPreconditionsProxy();
            Provider.GetMedicalPreConditionsAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
            {
                if (apiCallResult.Result.Success)
                {
                    Logger.LogInformation($"MedicalPreconditions for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");

                    MedicalPreconditionsProxy = apiCallResult.Result.Value;

                    var itemToSelect = _diabetesItemList.FirstOrDefault(_ => _.Key == (int)MedicalPreconditionsProxy.DiabetesType);

                    if (itemToSelect != null) itemToSelect.IsChecked = true;
                }
                else
                {
                    if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                    {
                        IsBusy = false;
                        return;
                    }

                    Device.BeginInvokeOnMainThread(async () =>
                    {
                        await Service.DialogService.DisplayAlertAsync(AppResources.MedicalPreconditions_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                    });

                    IsBusy = false;
                    return;
                }

                IsBusy = false;
            });
        }

        private async void SaveMedicalConditionsExecute()
        {
            await SaveMedicalConditions();
        }

        private async Task SaveMedicalConditions()
        {
            if (MedicalPreconditionsProxy.Id != 0)
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await UpdateMedicalConditions();
            }
            else
            {
                var isAuthorized = await ValidateToken();
                if (isAuthorized) await CreateMedicalConditions();
            }
        }

        private async Task UpdateMedicalConditions()
        {
            if (!HasInternetConnection)
            {
                return;
            }

            IsBusy = true;

            await ValidateToken();

            var selectedDietItem = _diabetesItemList.FirstOrDefault(_ => _.IsChecked);

            if (selectedDietItem != null) MedicalPreconditionsProxy.DiabetesType = (DiabetesType)selectedDietItem.Key;

            var apiCallResult = await Provider.UpdateMedicalConditionsAsnyc(MedicalPreconditionsProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"MedicalPreconditions for Account: {MedicalPreconditionsProxy.AccountId} successfully updated.");
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.MedicalPreconditions_Title, apiCallResult.ErrorMessage, AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        private async Task CreateMedicalConditions()
        {
            if (!HasInternetConnection)
            {
                return;
            }

            IsBusy = true;

            await ValidateToken();

            var selectedDietItem = _diabetesItemList.FirstOrDefault(_ => _.IsChecked);

            if (selectedDietItem != null) MedicalPreconditionsProxy.DiabetesType = (DiabetesType)selectedDietItem.Key;

            var apiCallResult = await Provider.CreateMedicalConditionsAsnyc(MedicalPreconditionsProxy, _cts.Token);

            if (apiCallResult.Success)
            {
                Logger.LogInformation($"MedicalPreconditions for Account: {MedicalPreconditionsProxy.AccountId} successfully created.");
            }
            else
            {
                if (apiCallResult.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.ErrorReason == ErrorReason.TimeOut)
                {
                    IsBusy = false;
                    return;
                }

                await Service.DialogService.DisplayAlertAsync(AppResources.MedicalPreconditions_Title, apiCallResult.ErrorMessage, AppResources.Button_Ok);

                IsBusy = false;
                return;
            }

            IsBusy = false;
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
        }

        private MedicalPreconditionsProxy _medicalPreconditionsProxy;

        public MedicalPreconditionsProxy MedicalPreconditionsProxy
        {
            get => _medicalPreconditionsProxy;
            set => SetProperty(ref _medicalPreconditionsProxy, value);
        }

        private List<RadioButtonItemProxy> _diabetesItemList;

        public List<RadioButtonItemProxy> DiabetesItemList
        {
            get => _diabetesItemList;
            set => SetProperty(ref _diabetesItemList, value);
        }

        private RadioButtonItemProxy _diabetesType1;

        public RadioButtonItemProxy DiabetesType1
        {
            get => _diabetesType1;
            set => SetProperty(ref _diabetesType1, value);
        }

        private RadioButtonItemProxy _diabetesType2Insulin;

        public RadioButtonItemProxy DiabetesType2Insulin
        {
            get => _diabetesType2Insulin;
            set => SetProperty(ref _diabetesType2Insulin, value);
        }

        private RadioButtonItemProxy _diabetesType2OralMedication;

        public RadioButtonItemProxy DiabetesType2OralMedication
        {
            get => _diabetesType2OralMedication;
            set => SetProperty(ref _diabetesType2OralMedication, value);
        }

        private RadioButtonItemProxy _diabetesType2Diet;

        public RadioButtonItemProxy DiabetesType2Diet
        {
            get => _diabetesType2Diet;
            set => SetProperty(ref _diabetesType2Diet, value);
        }

        private RadioButtonItemProxy _diabetesPreDiabetic;

        public RadioButtonItemProxy DiabetesPreDiabetic
        {
            get => _diabetesPreDiabetic;
            set => SetProperty(ref _diabetesPreDiabetic, value);
        }

        private void SelectDiabetes()
        {
            MedicalPreconditionsProxy.HasDiabetes = !MedicalPreconditionsProxy.HasDiabetes;
        }

        private void SelectHypertension()
        {
            MedicalPreconditionsProxy.HasHypertension = !MedicalPreconditionsProxy.HasHypertension;
        }

        private void SelectArthritis()
        {
            MedicalPreconditionsProxy.HasArthritis = !MedicalPreconditionsProxy.HasArthritis;
        }

        private void SelectCardiacCondition()
        {
            MedicalPreconditionsProxy.HasCardiacCondition = !MedicalPreconditionsProxy.HasCardiacCondition;
        }

        private void SelectOther()
        {
            MedicalPreconditionsProxy.Others = !MedicalPreconditionsProxy.Others;
        }

        private void SelectDontWantToSay()
        {
            MedicalPreconditionsProxy.DontWantToSayNow = !MedicalPreconditionsProxy.DontWantToSayNow;

        }


        private CancellationTokenSource _cts;
    }
}