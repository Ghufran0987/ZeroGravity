using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Navigation;
using Syncfusion.XForms.ProgressBar;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using UnitsNet;
using UnitsNet.Units;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Providers;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Constants;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.ViewModels
{
    public class WizardNewPageViewModel : VmBase<IWizardNewPage, IWizardNewPageVmProvider, WizardNewPageViewModel>
    {
        private IPersonalDataPageVmProvider _personalDataProvider;
        private IMedicalPreConditionsPageVmProvider _medicalConditionProvider;
        private IMyWeightPageVmProvider _myWeightPageVmProvider;

        public WizardNewPageViewModel(IVmCommonService service, IWizardNewPageVmProvider provider,
            IPersonalDataPageVmProvider personalDataProvider,
            IMedicalPreConditionsPageVmProvider medicalConditionProvider,
            ISecureStorageService secureStorageService,
            IMyWeightPageVmProvider myWeightPageVmProvider, ILoggerFactory loggerFactory, bool checkInternet = true) : base(service, provider, loggerFactory, checkInternet)
        {
            NextCommand = new DelegateCommand(NextStepExecute);
            PreviousCommand = new DelegateCommand(PreviousStepExecute);
            _secureStorageService = secureStorageService;
            _personalDataProvider = personalDataProvider;
            _medicalConditionProvider = medicalConditionProvider;
            _myWeightPageVmProvider = myWeightPageVmProvider;
            InitSteps();
        }

        #region "Fields and Properties"

        public bool IsEntry5Focused { get; set; }
        public bool IsEntry9Focused { get; set; }

        private CancellationTokenSource _cts;
        private PersonalDataProxy _personalDataProxy;
        private ObservableCollection<QuestionProxy> _questions;
        private QuestionProxy _selectedQuestion;
        private int _selectedQuestionIndex;
        private OnboardingStatus _step1Progress;
        private OnboardingStatus _step2Progress;
        private OnboardingStatus _step3Progress;
        private MedicalPreconditionsProxy _medicalConditionProxy;
        private bool IsFromBack;

        public DelegateCommand NextCommand { get; }
        public DelegateCommand PreviousCommand { get; }

        private readonly ISecureStorageService _secureStorageService;

        public PersonalDataProxy PersonalDataProxy
        {
            get => _personalDataProxy;
            set => SetProperty(ref _personalDataProxy, value);
        }

        public ObservableCollection<QuestionProxy> Questions
        {
            get => _questions;
            set => SetProperty(ref _questions, value);
        }

        public QuestionProxy SelectedQuestion
        {
            get => _selectedQuestion;
            set => SetProperty(ref _selectedQuestion, value);
        }

        public int SelectedQuestionIndex
        {
            get => _selectedQuestionIndex;
            set
            {
                if (value < Questions?.Count || value >= 0)
                {
                    SetProperty(ref _selectedQuestionIndex, value);
                    CalulateSteps();
                }
            }
        }

        public OnboardingStatus Step1Progress
        {
            get => _step1Progress;
            set => SetProperty(ref _step1Progress, value);
        }

        public OnboardingStatus Step2Progress
        {
            get => _step2Progress;
            set => SetProperty(ref _step2Progress, value);
        }

        public OnboardingStatus Step3Progress
        {
            get => _step3Progress;
            set => SetProperty(ref _step3Progress, value);
        }

        public OnboardingStatus Step4Progress
        {
            get => _step4Progress;
            set => SetProperty(ref _step4Progress, value);
        }

        public OnboardingStatus Step5Progress
        {
            get => _step5Progress;
            set => SetProperty(ref _step5Progress, value);
        }

        public OnboardingStatus Step6Progress
        {
            get => _step6Progress;
            set => SetProperty(ref _step6Progress, value);
        }

        private WeightItemDetailsProxy _weightItemDetailsProxy;
        private int userId;
        private OnboardingStatus _step6Progress;
        private OnboardingStatus _step5Progress;
        private OnboardingStatus _step4Progress;

        public WeightItemDetailsProxy WeightItemDetailsProxy
        {
            get => _weightItemDetailsProxy;
            set => SetProperty(ref _weightItemDetailsProxy, value);
        }

        #endregion "Fields and Properties"

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            try
            {
                IsBusy = true;
                userId = await _secureStorageService.LoadValue<int>(SecureStorageKey.UserId);
                if (parameters == null)
                {
                    IsFromBack = true;
                }
                else
                {
                    var param = NavigationParametersHelper.GetNavigationParameters<BooleanNavParams>(parameters);

                    if (param == null)
                    {
                        IsFromBack = true;
                    }
                    else
                    {
                        IsFromBack = false;
                    }
                }

                base.OnNavigatedTo(parameters);
                GetPersonalDataAsync();
                LoadMedicalConditions();
                GetWeightTrackerAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception  while OnNavigatedTo " + ex.Message + " stacktarce: " + ex.StackTrace);
            }
            finally
            {

            }
        }

        private async Task GetWeightTrackerAsync()
        {
            try
            {
                _cts = new CancellationTokenSource();

                await _myWeightPageVmProvider.GetCurrentWeightTrackerAsync(_cts.Token).ContinueWith(apiCallResult =>
                 {
                     try
                     {

                         if (apiCallResult.Result.Success)
                         {
                             //  Logger.LogInformation($"PersonalData for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");

                             if (apiCallResult.Result.Value == null)
                             {
                                 // New User WeightItemDetailsProxy will be null
                                 WeightItemDetailsProxy = new WeightItemDetailsProxy
                                 {
                                     AccountId = userId,
                                     Created = DateTime.UtcNow
                                 };
                             }
                             else
                             {
                                 WeightItemDetailsProxy = apiCallResult.Result.Value;
                             }
                             IsBusy = false;
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
                                 await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                             });
                             IsBusy = false;
                         }
                     }
                     catch (Exception ex)
                     {
                         Debug.WriteLine("Exception  in GetWeightTrackerAsync " + ex.Message + " stacktarce: " + ex.StackTrace);
                     }
                 });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception  while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);
                IsBusy = false;
            }
        }

        private async Task GetPersonalDataAsync()
        {
            try
            {
                _cts = new CancellationTokenSource();

                _personalDataProvider.GetPersonalDataAsnyc(_cts.Token).ContinueWith(async apiCallResult =>
                {
                    try
                    {
                        if (apiCallResult.Result.Success)
                        {
                            if (apiCallResult.Result.Value == null)
                            {
                                // New User PersonalDataProxy will be null
                                PersonalDataProxy = new PersonalDataProxy();
                                PersonalDataProxy.AccountId = userId;
                            }
                            else
                            {
                                PersonalDataProxy = apiCallResult.Result.Value;
                                Logger.LogInformation($"PersonalData for Account: {apiCallResult.Result.Value.AccountId} successfully loaded.");
                            }
                            await GetQuestionsDataAsync();
                            IsBusy = false;
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
                                await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                            });
                            IsBusy = false;
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Exception in GetPersonalDataAsync " + ex.Message + " stacktarce: " + ex.StackTrace);
                    }
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception  while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);
                IsBusy = false;
            }
        }

        private async Task GetQuestionsDataAsync()
        {
            try
            {
                _cts = new CancellationTokenSource();
                ShowProgress = true;
                await Provider.GetQuestionsAsync("all", true, _cts.Token).ContinueWith(apiCallResult =>
                 {
                     if (apiCallResult.Result.Success)
                     {
                         Logger.LogInformation($"Questions Account: {apiCallResult.Result.Value} successfully loaded.");
                         var results = apiCallResult.Result.Value.Where(x => x.IsActive).OrderBy(x => x.Number).ToList();
                         Questions = new ObservableCollection<QuestionProxy>(results);

                         MergeSelectedAnswersToQuestion();
                         if (Questions != null)
                         {
                             if (Questions.Count > 0)
                             {
                                 //If the navigation is coming from back bottom from goals
                                 if (IsFromBack)
                                 {
                                     this.SelectedQuestionIndex = Questions.Count - 1;
                                 }
                                 else
                                 {
                                     //Setting the last answered index
                                     if (_personalDataProxy.QuestionAnswers == null)
                                     {
                                         SelectedQuestionIndex = 0;
                                     }
                                     else if (_personalDataProxy.QuestionAnswers.Count <= 0)
                                     {
                                         SelectedQuestionIndex = 0;
                                     }
                                     else
                                     {
                                         int index = 0; //  GetLastAnswredMaxQuestionId();
                                         if (index < 0)
                                         {
                                             SelectedQuestionIndex = 0;
                                         }
                                         else if (index > Questions.Count - 1)
                                         {
                                             SelectedQuestionIndex = Questions.Count - 1;
                                         }
                                         else if (index >= 0 && index <= Questions.Count - 1)
                                         {
                                             if (index + 1 > Questions.Count - 1)
                                             {
                                                 SelectedQuestionIndex = Questions.Count - 1;
                                             }
                                             else
                                             {
                                                 SelectedQuestionIndex = index + 1;
                                             }
                                         }
                                         else
                                         {
                                             SelectedQuestionIndex = Questions.Count - 1;
                                         }
                                     }
                                 }
                             }
                         }

                         UpdateInitialStepAfterQuestionsLoaded();
                         CalulateSteps();
                         Device.BeginInvokeOnMainThread(() =>
                        {
                            ShowProgress = false;
                            IsBusy = false;
                        });
                     }
                     else
                     {
                         if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                         {
                             ShowProgress = false;
                             IsBusy = false;
                             return;
                         }

                         Device.BeginInvokeOnMainThread(async () =>
                         {
                             await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                         });
                         ShowProgress = false;
                         IsBusy = false;
                     }
                 });
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception  while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);
            }

            int GetLastAnswredMaxQuestionId()
            {
                int index = 0;
                double max = 0;
                int qId = 0;
                foreach (var pqa in _personalDataProxy.QuestionAnswers)
                {
                    var qa = Questions.FirstOrDefault(o => o.Id == pqa.QuestionId);
                    var number = qa.Number;
                    if (number > max)
                    {
                        max = number;
                        qId = qa.Id;
                    }
                }
                index = Questions.ToList().FindIndex(x => x.Id == qId);
                return index;
            }
        }

        private async Task LoadMedicalConditions()
        {
            await _medicalConditionProvider.GetMedicalPreConditionsAsnyc(_cts.Token).ContinueWith(apiCallResult =>
            {
                try
                {
                    if (apiCallResult.Result.Success)
                    {
                        Logger.LogInformation($"MedicalPreconditions for Account: {apiCallResult.Result?.Value?.AccountId} successfully loaded.");
                        this._medicalConditionProxy = apiCallResult.Result.Value;
                    }
                    else
                    {
                        if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
                        {
                            return;
                        }

                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(AppResources.MedicalPreconditions_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
                        });
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    Logger.LogError(ex.Message, ex);
                }
            });
        }

        //private async Task GetMedicalConditionDataAsync()
        //{
        //    _cts = new CancellationTokenSource();

        //    _medicalConditionProvider.GetMedicalPreConditionsAsnyc( _cts.Token).ContinueWith( apiCallResult =>
        //    {
        //        if (apiCallResult.Result.Success)
        //        {
        //            Logger.LogInformation($"Questions Account: {apiCallResult.Result.Value} successfully loaded.");

        //            _medicalConditionProxy = apiCallResult.Result.Value;

        //            MergeMedicalConditionsToQuestion();

        //            // TODO Set from previous left over screen
        //            this.SelectedQuestionIndex = 0;
        //        }
        //        else
        //        {
        //            if (apiCallResult.Result.ErrorReason == ErrorReason.TaskCancelledByUserOperation || apiCallResult.Result.ErrorReason == ErrorReason.TimeOut)
        //            {
        //                return;
        //            }

        //            Device.BeginInvokeOnMainThread(async () =>
        //            {
        //                await Service.DialogService.DisplayAlertAsync(AppResources.PersonalData_Title, apiCallResult.Result.ErrorMessage, AppResources.Button_Ok);
        //            });
        //        }
        //    });
        //}

        private void MergeSelectedAnswersToQuestion()
        {
            try
            {
                if (PersonalDataProxy != null && this.Questions != null)
                {
                    if (PersonalDataProxy?.QuestionAnswers?.Count > 0)
                    {
                        foreach (var quest in this.Questions)
                        {
                            switch (quest.Type)
                            {
                                case QuestionType.SingleChoice:
                                case QuestionType.ManualEntry:
                                    var question = PersonalDataProxy.QuestionAnswers.FirstOrDefault(x => x.QuestionId == quest.Id);
                                    if (question != null)
                                    {
                                        quest.SelectedAnswer = quest.Answers.FirstOrDefault(x => x.Id == question.AnswerId);
                                        if (quest.SelectedAnswer != null)
                                        {
                                            quest.SelectedAnswer.Value = question.Value;
                                            quest.SelectedTag = question.Tag1;
                                            quest.SelectedAnswer.Id = question.AnswerId;
                                        }
                                    }
                                    break;

                                case QuestionType.MultipleChoice:
                                    var questionAndAnswers = PersonalDataProxy.QuestionAnswers.FindAll(x => x.QuestionId == quest.Id);
                                    if (questionAndAnswers != null)
                                    {
                                        if (questionAndAnswers.Count > 0)
                                        {
                                            foreach (var qa in questionAndAnswers)
                                            {
                                                var ansOption = quest.Answers.FirstOrDefault(x => x.Id == qa.AnswerId);
                                                quest.SelectedAnswers.Add(ansOption);
                                            }
                                        }
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
        }

        public void UpdateQuestionandAnswerData()
        {
            //Copying values from Questions selected answers to perdondata Questiona nd answers
            try
            {

                if (PersonalDataProxy != null && this.Questions != null)
                {
                    if (Questions.Count > 0)
                    {
                        foreach (var quest in this.Questions)
                        {
                            if (quest.SelectedAnswers != null)
                            {
                                if (quest.SelectedAnswers.Count > 0)
                                {
                                    //Check if Person data contains the questionand anser for the question
                                    var result = PersonalDataProxy.QuestionAnswers.FindAll(x => x.QuestionId == quest.Id);

                                    if (result.Count > 0)
                                    {
                                        bool added = false;
                                        //Proxy already have the question and answer so up date the same

                                        foreach (var ans in quest.SelectedAnswers)
                                        {
                                            foreach (var proxyans in result)
                                            {
                                                if (((AnswerOptionProxy)ans).Id == proxyans.AnswerId)
                                                {
                                                    added = true;
                                                }
                                            }

                                            if (!added)
                                            {
                                                //New ans need to be added
                                                var newQA = new QuestionAnswersProxy()
                                                {
                                                    AnswerId = ((AnswerOptionProxy)ans).Id,
                                                    QuestionId = quest.Id,
                                                    Tag1 = quest.SelectedTag
                                                };
                                                PersonalDataProxy.QuestionAnswers.Add(newQA);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        //For removing items which are not in this.Questions but there in proxy

                        foreach (var quest in this.Questions)
                        {
                            if (quest.SelectedAnswers == null)
                            {
                                var result = PersonalDataProxy.QuestionAnswers.FindAll(x => x.QuestionId == quest.Id);
                                if (result.Count > 0)
                                {
                                    //Clear all of this as its not there in questions
                                    foreach (var item in result)
                                    {
                                        PersonalDataProxy.QuestionAnswers.Remove(item);
                                    }
                                }
                            }
                            else
                            {
                                if (quest.SelectedAnswers.Count > 0)
                                {
                                    var result = PersonalDataProxy.QuestionAnswers.FindAll(x => x.QuestionId == quest.Id);

                                    QuestionAnswersProxy removeitem = null;
                                    foreach (var ans in quest.SelectedAnswers)
                                    {
                                        bool found = true;
                                        foreach (var qa in result)
                                        {
                                            removeitem = qa;
                                            if (qa.AnswerId == ((AnswerOptionProxy)ans).Id)
                                            {
                                                found = true;
                                                removeitem = null;
                                                break;
                                            }
                                        }

                                        if (!found && removeitem != null)
                                        {
                                            PersonalDataProxy.QuestionAnswers.Remove(removeitem);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
            }
            //  savePersonalData();
        }

        //private void saveMedicalConditionData()
        //{
        //    try
        //    {
        //        var cancellationToken = new CancellationTokenSource().Token;
        //        if (_personalDataProxy.Id <= 0)
        //        {
        //            _medicalConditionProvider.CreateMedicalConditionsAsnyc (this._medicalConditionProxy, cancellationToken);
        //        }
        //        else
        //        {
        //            _medicalConditionProvider.UpdateMedicalConditionsAsnyc(this._medicalConditionProxy, cancellationToken);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Debug.WriteLine("Exception  while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);
        //    }
        //}

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            //Saving created personal data proxy and Medical condition proxy.
            base.OnNavigatedFrom(parameters);
            if (_cts != null)
                CancelPendingRequest(_cts);
        }

        #region "Previous"

        private async void PreviousStepExecute()
        {
            try
            {
                if (SelectedQuestionIndex == 0)
                {
                    await Service.NavigationService.GoBackAsync();
                }
                else
                {

                    SelectedQuestionIndex -= 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in PreviousStepExecute:  " + ex.Message + " stacktarce: " + ex.StackTrace);
            }
        }

        #endregion "Previous"

        #region "Next"

        private async void NextStepExecute()
        {
            try
            {
                if (Questions?.Count <= 0)
                    return;
                if (SelectedQuestionIndex >= 0 || SelectedQuestionIndex <= Questions.Count - 1)
                {
                    ShowProgress = true;
                    IsBusy = true;
                    SelectedQuestion = Questions[this.SelectedQuestionIndex];
                    if (SelectedQuestion != null)
                    {
                        // If Selected Question Type is Information Move Next
                        if (SelectedQuestion.Type == QuestionType.Information)
                        {
                            await MoveNext();
                        }
                        else // else Update Selected Ans to Proxy.QAs and Move Next
                        {
                            // Validate
                            if (ValidateSelectedAnswer(SelectedQuestion))
                            {
                                // Update
                                if (UpdateQuestionData(SelectedQuestion))
                                {
                                    if (SelectedQuestion.DataFieldType == DataFieldType.HealthCondition)
                                    {
                                        //Save Medical condition proxy
                                        if (await SaveMedicalConditionDataAsync())
                                        {
                                            await MoveNext();
                                        }
                                        else
                                        {
                                            // TODO Show Msg to User
                                        }
                                    }
                                    else if (SelectedQuestion.DataFieldType == DataFieldType.DesiredWeight)
                                    {
                                        if (await SaveWeightTrackerDataAsync())
                                        {
                                            await MoveNext();
                                        }
                                        else
                                        {
                                            // TODO Show Msg to User
                                        }
                                    }
                                    else
                                    {
                                        // Save
                                        if (await SavePersonalDataAsync())
                                        {
                                            // Move Next
                                            await MoveNext();
                                        }
                                        else
                                        {
                                            // TODO Show Msg to User
                                        }
                                    }
                                }
                            }
                        }
                    }
                    ShowProgress = false;
                    IsBusy = false;

                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in NextStepExecute:  " + ex.Message + " stacktarce: " + ex.StackTrace);

            }
            finally
            {
                ShowProgress = false;
                IsBusy = false;
            }
        }

        private async Task MoveNext()
        {
            try
            {
                if (SelectedQuestionIndex == Questions.Count - 1)
                {
                    // last question move to goals
                    await Service.NavigationService.NavigateAsync(ViewName.WizardFinishSetupPage);
                }
                else
                {
                    SelectedQuestionIndex += 1;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception in MoveNext:  " + ex.Message + " stacktarce: " + ex.StackTrace);

            }
        }

        public bool UpdateQuestionData(QuestionProxy selectedQuestion)
        {
            if (selectedQuestion == null)
            {
                return false;
            }
            try
            {
                bool isChecked = false;
                switch (selectedQuestion.Type)
                {
                    case QuestionType.Information:
                        isChecked = true;
                        break;

                    case QuestionType.SingleChoice:
                    case QuestionType.ManualEntry:
                        isChecked = UpdateFromSingleAnswer(selectedQuestion);
                        break;

                    case QuestionType.MultipleChoice:
                        isChecked = UpdateFromMultipleAnswers(selectedQuestion);
                        break;
                }
                return isChecked;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("UpdateQuestionData: Exception while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);
                return false;
            }
        }

        private bool ValidateSelectedAnswer(QuestionProxy selectedQuestion)
        {
            if (selectedQuestion == null) // This case should not happen
            {
                return false;
            }

            bool isValid = false;
            switch (selectedQuestion.Type)
            {
                case QuestionType.Information:
                    isValid = true;
                    break;

                case QuestionType.SingleChoice:
                    if (selectedQuestion.SelectedAnswer == null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(
                                 "On-boarding",
                                 "Please select an answer to proceed.",
                                  AppResources.Button_Ok);
                        });
                    }
                    else if (string.IsNullOrEmpty(selectedQuestion.SelectedAnswer.Value) || string.IsNullOrWhiteSpace(selectedQuestion.SelectedAnswer.Value))
                    {
                        isValid = false;
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(
                                 "On-boarding",
                                 "Please select an answer to proceed.",
                                  AppResources.Button_Ok);
                        });
                    }
                    else
                    {
                        isValid = true;
                    }
                    break;

                case QuestionType.ManualEntry:
                    if (selectedQuestion.SelectedAnswer == null)
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(
                                 "On-boarding",
                                 "Please enter your answer to proceed.",
                                  AppResources.Button_Ok);
                        });
                    }
                    else if (string.IsNullOrEmpty(selectedQuestion.SelectedAnswer.Value) || string.IsNullOrWhiteSpace(selectedQuestion.SelectedAnswer.Value))
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(
                                 "On-boarding",
                                 "Please enter your answer to proceed.",
                                  AppResources.Button_Ok);
                        });
                    }
                    else if (selectedQuestion.TagOptions?.Count > 0 && string.IsNullOrEmpty(selectedQuestion.SelectedTag))
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(
                                 "On-boarding",
                                 "Please select unit.",
                                  AppResources.Button_Ok);
                        });
                    }
                    else
                    {
                        // Validate Data Range for selectedQuestion
                        isValid = ValidateDataRange(selectedQuestion);
                    }
                    break;

                case QuestionType.MultipleChoice:
                    if (selectedQuestion?.SelectedAnswers?.Count > 0)
                    {
                        isValid = true;
                    }
                    else
                    {
                        Device.BeginInvokeOnMainThread(async () =>
                        {
                            await Service.DialogService.DisplayAlertAsync(
                                 "On-boarding",
                                 "Please select at least one answer to proceed.",
                                  AppResources.Button_Ok);
                        });
                    }
                    break;
            }
            return isValid;
        }

        private bool ValidateDataRange(QuestionProxy selectedQuestion)
        {
            bool isValid = false;
            if (selectedQuestion?.Type == QuestionType.ManualEntry)
            {
                switch (SelectedQuestion.DataFieldType)
                {
                    case DataFieldType.Country:
                        isValid = true;
                        break;

                    case DataFieldType.DesiredWeight:
                        isValid = ValidateDesiredWeight();
                        break;

                    case DataFieldType.DeviceType:
                        isValid = true;
                        break;

                    case DataFieldType.Diabetes:
                        isValid = true;
                        break;

                    case DataFieldType.DiabetesType:
                        isValid = true;
                        break;

                    case DataFieldType.DietType:
                        isValid = true;
                        break;

                    case DataFieldType.Ethnicity:
                        isValid = true;
                        break;

                    case DataFieldType.Gender:
                        isValid = true;

                        break;

                    case DataFieldType.PhoneNumber:
                        isValid = ValidatePhoneNumber();

                        break;

                    case DataFieldType.Height:
                        isValid = ValidateHeight();
                        break;

                    case DataFieldType.HipDiameter:
                        isValid = ValidateHipDiameter();
                        break;

                    case DataFieldType.Hypertension:
                        isValid = true;
                        break;

                    case DataFieldType.NeckDiameter:
                        isValid = ValidateNeckDiameter();
                        break;

                    case DataFieldType.WaistDiameter:
                        isValid = ValidateWaistDiameter();
                        break;

                    case DataFieldType.TimeZone:
                        isValid = true;
                        break;

                    case DataFieldType.Others:
                        isValid = true;
                        break;

                    case DataFieldType.Weight:
                        isValid = ValidateWeight();
                        break;

                    case DataFieldType.YearOfBirth:
                        isValid = true;
                        break;

                    case DataFieldType.Salutation:
                        isValid = true;
                        break;

                    case DataFieldType.FirstName:
                        isValid = true;
                        break;

                    case DataFieldType.LastName:
                        isValid = true;
                        break;

                    case DataFieldType.Arthritis:
                        isValid = true;
                        break;

                    case DataFieldType.HealthCondition:
                        isValid = true;
                        break;

                    case DataFieldType.FastingGoal:
                        isValid = true;
                        break;
                }
            }
            return isValid;
        }



        private bool ValidatePhoneNumber()
        {
            bool isValid;
            var ph = SelectedQuestion.SelectedAnswer.Value;
            if (!string.IsNullOrEmpty(ph))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
            }
            return isValid;
        }

        private bool ValidateHipDiameter()
        {
            bool isValid;
            var minHip = PersonalDataConstants.MinWaistDiametertCm;
            var maxHip = PersonalDataConstants.MaxWaistDiametertCm;

            var Hip = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);

            // Convert height to respective unit and then validate
            switch (SelectedQuestion.SelectedTag)
            {
                case "In.":
                    minHip = Convert.ToDouble(new Length(minHip, LengthUnit.Centimeter).Inches);
                    maxHip = Convert.ToDouble(new Length(maxHip, LengthUnit.Centimeter).Inches);
                    break;

                case "Cm.":
                    break;
            }

            if (Hip >= minHip && Hip <= maxHip)
            {
                isValid = true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Service.DialogService.DisplayAlertAsync(
                         "On-boarding",
                         "Hip Diameter should be between " + minHip.ToString("F1") + " and " + maxHip.ToString("F1"),
                          AppResources.Button_Ok);
                });
                isValid = false;
            }

            return isValid;
        }

        private bool ValidateNeckDiameter()
        {
            bool isValid;
            var minNeck = PersonalDataConstants.MinNeckDiametertCm;
            var maxNeck = PersonalDataConstants.MaxNeckDiametertCm;

            var Neck = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);

            // Convert height to respective unit and then validate
            switch (SelectedQuestion.SelectedTag)
            {
                case "In.":
                    minNeck = Convert.ToDouble(new Length(minNeck, LengthUnit.Centimeter).Inches);
                    maxNeck = Convert.ToDouble(new Length(maxNeck, LengthUnit.Centimeter).Inches);
                    break;

                case "Cm.":
                    break;
            }

            if (Neck >= minNeck && Neck <= maxNeck)
            {
                isValid = true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Service.DialogService.DisplayAlertAsync(
                         "On-boarding",
                         "Neck Diameter should be between " + minNeck.ToString("F1") + " and " + maxNeck.ToString("F1"),
                          AppResources.Button_Ok);
                });
                isValid = false;
            }

            return isValid;
        }

        private bool ValidateWaistDiameter()
        {
            bool isValid;
            var minWaist = PersonalDataConstants.MinWaistDiametertCm;
            var maxWaist = PersonalDataConstants.MaxWaistDiametertCm;

            var waist = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);

            // Convert height to respective unit and then validate
            switch (SelectedQuestion.SelectedTag)
            {
                case "In.":
                    minWaist = Convert.ToDouble(new Length(minWaist, LengthUnit.Centimeter).Inches);
                    maxWaist = Convert.ToDouble(new Length(maxWaist, LengthUnit.Centimeter).Inches);
                    break;

                case "Cm.":
                    break;
            }

            if (waist >= minWaist && waist <= maxWaist)
            {
                isValid = true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Service.DialogService.DisplayAlertAsync(
                         "On-boarding",
                         "Waist Diameter should be between " + minWaist.ToString("F1") + " and " + maxWaist.ToString("F1"),
                          AppResources.Button_Ok);
                });
                isValid = false;
            }

            return isValid;
        }

        private bool ValidateDesiredWeight()
        {
            bool isValid;
            var minWeight = PersonalDataConstants.MinDesiredWeightKg;

            var weight = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);

            // Convert height to respective unit and then validate
            switch (SelectedQuestion.SelectedTag)
            {
                case "St.":
                    minWeight = Convert.ToDouble(new Mass(minWeight, MassUnit.Kilogram).Stone);
                    break;

                case "Lb.":
                    minWeight = Convert.ToDouble(new Mass(minWeight, MassUnit.Kilogram).Pounds);
                    break;

                case "Kg.":
                    break;
            }

            if (weight > minWeight)
            {
                isValid = true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Service.DialogService.DisplayAlertAsync(
                         "On-boarding",
                         "Our clinical experts do not recommend this is a healthy weight for you, based on the inputs so far.",
                          AppResources.Button_Ok);
                });
                isValid = false;
            }

            return isValid;
        }

        private bool ValidateWeight()
        {
            bool isValid;
            var minWeight = PersonalDataConstants.MinWeightKg;
            var maxWeight = PersonalDataConstants.MaxWeightKg;

            var weight = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);

            // Convert height to respective unit and then validate
            switch (SelectedQuestion.SelectedTag)
            {
                case "St.":
                    minWeight = Convert.ToDouble(new Mass(minWeight, MassUnit.Kilogram).Stone);
                    maxWeight = Convert.ToDouble(new Mass(maxWeight, MassUnit.Kilogram).Stone);
                    break;

                case "Lb.":
                    minWeight = Convert.ToDouble(new Mass(minWeight, MassUnit.Kilogram).Pounds);
                    maxWeight = Convert.ToDouble(new Mass(maxWeight, MassUnit.Kilogram).Pounds);
                    break;

                case "Kg.":
                    break;
            }

            if (weight >= minWeight && weight <= maxWeight)
            {
                isValid = true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Service.DialogService.DisplayAlertAsync(
                         "On-boarding",
                         "Weight should be between " + minWeight.ToString("F1") + " and " + maxWeight.ToString("F1"),
                          AppResources.Button_Ok);
                });
                isValid = false;
            }

            return isValid;
        }

        private bool ValidateHeight()
        {
            bool isValid;
            var maxHeight = PersonalDataConstants.MaxHeightCm;
            var minHieght = PersonalDataConstants.MinHeightCm;

            var hieght = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);

            // Convert height to respective unit and then validate
            switch (SelectedQuestion.SelectedTag)
            {
                case "Ft.":
                    maxHeight = Convert.ToDouble(new Length(maxHeight, LengthUnit.Centimeter).Feet);
                    minHieght = Convert.ToDouble(new Length(minHieght, LengthUnit.Centimeter).Feet);
                    break;

                case "In.":
                    maxHeight = Convert.ToDouble(new Length(maxHeight, LengthUnit.Centimeter).Inches);
                    minHieght = Convert.ToDouble(new Length(minHieght, LengthUnit.Centimeter).Inches);
                    break;

                case "Cm.":
                    break;
            }

            if (hieght >= minHieght && hieght <= maxHeight)
            {
                isValid = true;
            }
            else
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Service.DialogService.DisplayAlertAsync(
                         "On-boarding",
                         "Height should be between " + minHieght.ToString("F1") + " and " + maxHeight.ToString("F1"),
                          AppResources.Button_Ok);
                });
                isValid = false;
            }

            return isValid;
        }

        private bool UpdateFromSingleAnswer(QuestionProxy selectedQuestion)
        {
            if (selectedQuestion == null)
            {
                return false;
            }

            try
            {
                var questionAnswer = PersonalDataProxy?.QuestionAnswers.FirstOrDefault(x => x.QuestionId == selectedQuestion.Id);
                if (questionAnswer != null)
                {
                    questionAnswer.AnswerId = selectedQuestion.SelectedAnswer.Id;
                    questionAnswer.Value = selectedQuestion.SelectedAnswer.Value;
                    questionAnswer.Tag1 = selectedQuestion.SelectedTag;
                    questionAnswer.AccountId = PersonalDataProxy.AccountId;
                    questionAnswer.PersonDataId = PersonalDataProxy.Id;
                }
                else
                {
                    PersonalDataProxy.QuestionAnswers.Add(new QuestionAnswersProxy
                    {
                        QuestionId = selectedQuestion.Id,
                        AnswerId = selectedQuestion.SelectedAnswer.Id,
                        Value = selectedQuestion.SelectedAnswer.Value,
                        Tag1 = selectedQuestion.SelectedTag,
                        AccountId = PersonalDataProxy.AccountId,
                        PersonDataId = PersonalDataProxy.Id
                    });
                }
                return AssignToPersonalDataProxy();
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception  while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);
            }
            return false;
        }

        private bool AssignToPersonalDataProxy()
        {
            switch (SelectedQuestion.DataFieldType)
            {
                case DataFieldType.Country:
                    PersonalDataProxy.Country = SelectedQuestion.SelectedAnswer.Value;
                    break;

                case DataFieldType.DeviceType:
                    break;

                case DataFieldType.Diabetes:
                    //if (_medicalConditionProxy != null)
                    //   _medicalConditionProxy.HasDiabetes = Convert.ToBoolean(Convert.ToInt32(SelectedQuestion.SelectedAnswer.Value));
                    break;

                case DataFieldType.DiabetesType:
                    break;

                case DataFieldType.DietType:
                    break;

                case DataFieldType.HealthCondition:
                    if (_medicalConditionProxy != null)
                    {
                        if (SelectedQuestion.SelectedAnswers.Count > 0)
                        {
                            foreach (var sel in SelectedQuestion.SelectedAnswers)
                            {
                                AnswerOptionProxy ans = sel as AnswerOptionProxy;
                                HealthCondition healthCondition = (HealthCondition)Convert.ToInt32(ans.Value);
                                switch (healthCondition)
                                {
                                    case HealthCondition.Depression:
                                        break;

                                    case HealthCondition.Diabetes:
                                        _medicalConditionProxy.HasDiabetes = true;
                                        break;

                                    case HealthCondition.HeartDisease:
                                        _medicalConditionProxy.HasCardiacCondition = true;
                                        break;

                                    case HealthCondition.HighBloodPressure:
                                        _medicalConditionProxy.HasHypertension = true;
                                        break;

                                    case HealthCondition.Osteoarthrities:
                                        _medicalConditionProxy.HasArthritis = true;
                                        break;

                                    case HealthCondition.DontWantToSayNow:
                                        _medicalConditionProxy.DontWantToSayNow = true;
                                        break;

                                    case HealthCondition.Other:
                                        _medicalConditionProxy.Others = true;
                                        break;
                                }
                            }
                        }
                    }
                    break;

                case DataFieldType.Ethnicity:
                    PersonalDataProxy.Ethnicity = SelectedQuestion.SelectedAnswer.Value;
                    break;

                case DataFieldType.Gender:
                    PersonalDataProxy.Gender = (GenderBiologicalType)Convert.ToInt32(SelectedQuestion.SelectedAnswer.Value);
                    UpdateGenderImage();
                    break;

                case DataFieldType.PhoneNumber:
                    break;

                case DataFieldType.FastingGoal:
                    break;

                case DataFieldType.DesiredWeight:

                    double targetwt = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);
                    switch (SelectedQuestion.SelectedTag)
                    {
                        case "St.":
                            WeightItemDetailsProxy.TargetWeight = Convert.ToDouble(new Mass(targetwt, MassUnit.Stone).Kilograms);
                            break;

                        case "Lb.":
                            WeightItemDetailsProxy.TargetWeight = Convert.ToDouble(new Mass(targetwt, MassUnit.Pound).Kilograms);
                            break;

                        case "Kg.":
                            WeightItemDetailsProxy.TargetWeight = targetwt;
                            break;
                    }

                    WeightItemDetailsProxy.CurrentWeight = Math.Round(PersonalDataProxy.Weight, 2);
                    WeightItemDetailsProxy.InitialWeight = Math.Round(PersonalDataProxy.Weight, 2);

                    break;

                case DataFieldType.Weight:
                    // Convert weight to respective unit and then validate
                    double wt = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);
                    switch (SelectedQuestion.SelectedTag)
                    {
                        case "St.":
                            PersonalDataProxy.Weight = Convert.ToDouble(new Mass(wt, MassUnit.Stone).Kilograms);
                            break;

                        case "Lb.":
                            PersonalDataProxy.Weight = Convert.ToDouble(new Mass(wt, MassUnit.Pound).Kilograms);
                            break;

                        case "Kg.":
                            PersonalDataProxy.Weight = wt;
                            break;
                    }

                    PersonalDataProxy.Weight = Math.Round(PersonalDataProxy.Weight, 2);
                    break;

                case DataFieldType.Height:
                    var hieght = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);

                    // Convert height to respective unit and then validate
                    switch (SelectedQuestion.SelectedTag)
                    {
                        case "Ft.":
                            PersonalDataProxy.Height = Convert.ToDouble(new Length(hieght, LengthUnit.Foot).Centimeters);
                            break;

                        case "In.":
                            PersonalDataProxy.Height = Convert.ToDouble(new Length(hieght, LengthUnit.Inch).Centimeters);
                            break;

                        case "Cm.":
                            PersonalDataProxy.Height = hieght;
                            break;
                    }
                    PersonalDataProxy.Height = Math.Round(PersonalDataProxy.Height, 2);
                    break;

                case DataFieldType.HipDiameter:
                    var hip = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);

                    // Convert hip to respective unit and then validate
                    switch (SelectedQuestion.SelectedTag)
                    {
                        case "In.":
                            PersonalDataProxy.HipDiameter = Convert.ToDouble(new Length(hip, LengthUnit.Inch).Centimeters);
                            break;

                        case "Cm.":
                            PersonalDataProxy.HipDiameter = hip;
                            break;
                    }
                    PersonalDataProxy.HipDiameter = Math.Round(PersonalDataProxy.HipDiameter, 2);

                    break;

                case DataFieldType.NeckDiameter:
                    var neck = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);
                    // Convert hip to respective unit and then validate
                    switch (SelectedQuestion.SelectedTag)
                    {
                        case "In.":
                            PersonalDataProxy.NeckDiameter = Convert.ToDouble(new Length(neck, LengthUnit.Inch).Centimeters);
                            break;

                        case "Cm.":
                            PersonalDataProxy.NeckDiameter = neck;
                            break;
                    }
                    PersonalDataProxy.NeckDiameter = Math.Round(PersonalDataProxy.NeckDiameter, 2);

                    break;

                case DataFieldType.WaistDiameter:
                    var waist = Convert.ToDouble(SelectedQuestion.SelectedAnswer.Value);
                    // Convert hip to respective unit and then validate
                    switch (SelectedQuestion.SelectedTag)
                    {
                        case "In.":
                            PersonalDataProxy.WaistDiameter = Convert.ToDouble(new Length(waist, LengthUnit.Inch).Centimeters);
                            break;

                        case "Cm.":
                            PersonalDataProxy.WaistDiameter = waist;
                            break;
                    }
                    PersonalDataProxy.WaistDiameter = Math.Round(PersonalDataProxy.WaistDiameter, 2);

                    break;

                case DataFieldType.TimeZone:
                    break;

                case DataFieldType.Others:
                    break;

                case DataFieldType.YearOfBirth:
                    int age = Convert.ToInt32(SelectedQuestion.SelectedAnswer.Value);
                    PersonalDataProxy.YearOfBirth = DateTime.Now.AddYears(-age).Year;
                    break;

                case DataFieldType.Salutation:
                    PersonalDataProxy.Salutation = (SalutationType)Convert.ToInt32(SelectedQuestion.SelectedAnswer.Value);
                    break;

                case DataFieldType.FirstName:
                    PersonalDataProxy.FirstName = SelectedQuestion.SelectedAnswer.Value;
                    break;

                case DataFieldType.LastName:
                    PersonalDataProxy.LastName = SelectedQuestion.SelectedAnswer.Value;
                    break;
            }
            return true;
        }

        private void UpdateGenderImage()
        {
            var imageName = "";
            string image = "";

            switch (PersonalDataProxy.Gender)
            {
                case GenderBiologicalType.Male:
                case GenderBiologicalType.Female:
                    var neck = this.Questions.Where(x => x.DataFieldType == DataFieldType.NeckDiameter).FirstOrDefault();
                    if (neck != null)
                    {
                        imageName = "basic-bg-neck.png";
                        image = string.Format("ZeroGravity.Mobile.Resources.Images.{0}", imageName);
                        neck.BackgroundImageSource = ImageSource.FromResource(image);
                    }

                    var waist = this.Questions.Where(x => x.DataFieldType == DataFieldType.WaistDiameter).FirstOrDefault();
                    if (waist != null)
                    {
                        imageName = "basic-bg-waist.png";
                        image = string.Format("ZeroGravity.Mobile.Resources.Images.{0}", imageName);
                        waist.BackgroundImageSource = ImageSource.FromResource(image);
                    }

                    var hips = this.Questions.Where(x => x.DataFieldType == DataFieldType.HipDiameter).FirstOrDefault();
                    if (hips != null)
                    {
                        imageName = "basic-bg-hips.png";
                        image = string.Format("ZeroGravity.Mobile.Resources.Images.{0}", imageName);
                        hips.BackgroundImageSource = ImageSource.FromResource(image);
                    }
                    break;

                case GenderBiologicalType.NonBinary:
                case GenderBiologicalType.Undefined:

                    var neck1 = this.Questions.Where(x => x.DataFieldType == DataFieldType.NeckDiameter).FirstOrDefault();
                    if (neck1 != null)
                    {
                        imageName = "neutral_gender_neck.png";
                        image = string.Format("ZeroGravity.Mobile.Resources.Images.{0}", imageName);
                        neck1.BackgroundImageSource = ImageSource.FromResource(image);
                    }

                    var waist1 = this.Questions.Where(x => x.DataFieldType == DataFieldType.WaistDiameter).FirstOrDefault();
                    if (waist1 != null)
                    {
                        imageName = "neutral_gender_waist.png";
                        image = string.Format("ZeroGravity.Mobile.Resources.Images.{0}", imageName);
                        waist1.BackgroundImageSource = ImageSource.FromResource(image);
                    }

                    var hips1 = this.Questions.Where(x => x.DataFieldType == DataFieldType.HipDiameter).FirstOrDefault();
                    if (hips1 != null)
                    {
                        imageName = "neutral_gender_hips.png";
                        image = string.Format("ZeroGravity.Mobile.Resources.Images.{0}", imageName);
                        hips1.BackgroundImageSource = ImageSource.FromResource(image);
                    }
                    break;
            }
        }

        private bool UpdateFromMultipleAnswers(QuestionProxy selectedQuestion)
        {
            if (selectedQuestion == null)
            {
                return false;
            }
            try
            {
                // Delete QA from PersonalDataProxy.QuestionAndAnswers that are not in the selectedQuestion.SelectedAnswers collection
                var list = PersonalDataProxy?.QuestionAnswers?.FindAll(x => x.QuestionId == selectedQuestion.Id).ToList();

                // if(list.Count== 0) Fresh question user answering first time
                for (int i = list.Count - 1; i >= 0; i--)
                {
                    var dbValue = list[i];
                    if (!selectedQuestion.SelectedAnswers.Any(s => ((AnswerOptionProxy)s).Id == dbValue.Id))
                    {
                        PersonalDataProxy.QuestionAnswers.Remove(dbValue);
                    }
                }

                foreach (var qa in selectedQuestion?.SelectedAnswers)
                {
                    AnswerOptionProxy newQA = qa as AnswerOptionProxy;
                    var dbValue = PersonalDataProxy?.QuestionAnswers?.SingleOrDefault(s => s.Id == newQA.Id);
                    if (dbValue != null)
                    {
                        // Update dbValue that are in the selectedQuestion.SelectedAnswers collection
                        dbValue.AnswerId = newQA.Id;
                        dbValue.Value = newQA.Value;
                        dbValue.QuestionId = selectedQuestion.Id;
                        dbValue.AccountId = PersonalDataProxy.AccountId;
                        dbValue.PersonDataId = PersonalDataProxy.Id;
                    }
                    else
                    {    // Insert newQaA into the database that are not in the dbValue.QuestionAnswers collection
                        PersonalDataProxy.QuestionAnswers.Add(new QuestionAnswersProxy()
                        {
                            AnswerId = newQA.Id,
                            Value = newQA.Value,
                            QuestionId = selectedQuestion.Id,
                            AccountId = PersonalDataProxy.AccountId,
                            PersonDataId = PersonalDataProxy.Id
                        });
                    }
                }

                // Assign Medical Condition if it is medical question
                return true;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception  while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);
            }
            return false;
        }

        #endregion "Next"

        #region "Save"

        private async Task<bool> SavePersonalDataAsync()
        {
            try
            {
                var cancellationToken = new CancellationTokenSource().Token;
                if (_personalDataProxy?.Id <= 0)
                {
                    var apiResult = await _personalDataProvider.CreatePersonalDataAsnyc(this._personalDataProxy, cancellationToken);
                    if (apiResult.Success)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            PersonalDataProxy = apiResult.Value;
                        });
                        return true;
                    }
                    else
                    {
                        // TODO Handle error case
                        return false;
                    }
                }
                else
                {
                    var apiResult = await _personalDataProvider.UpdatePersonalDataAsnyc(this._personalDataProxy, cancellationToken);
                    if (apiResult.Success)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            PersonalDataProxy = apiResult.Value;
                        });
                        return true;
                    }
                    else
                    {
                        // TODO Handle error case
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SavePersonalDataAsync: Exception while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);
                return false;
            }
        }

        private async Task<bool> SaveMedicalConditionDataAsync()
        {
            try
            {
                var cancellationToken = new CancellationTokenSource().Token;
                if (_medicalConditionProxy?.Id <= 0)
                {
                    var apiResult = await _medicalConditionProvider.CreateMedicalConditionsAsnyc(this._medicalConditionProxy, cancellationToken);
                    if (apiResult.Success)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _medicalConditionProxy = apiResult.Value;
                        });
                        return true;
                    }
                    else
                    {
                        // TODO Handle error case
                        return false;
                    }
                }
                else
                {
                    var apiResult = await _medicalConditionProvider.UpdateMedicalConditionsAsnyc(this._medicalConditionProxy, cancellationToken);
                    if (apiResult.Success)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _medicalConditionProxy = apiResult.Value;
                        });
                        return true;
                    }
                    else
                    {
                        // TODO Handle error case
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("SaveMedicalConditionDataAsync: Exception while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);
                return false;
            }
        }

        private async Task<bool> SaveWeightTrackerDataAsync()
        {
            try
            {
                _cts = new CancellationTokenSource();
                if (WeightItemDetailsProxy.Id <= 0)
                {
                    //get the data for weight

                    var apiResult = await _myWeightPageVmProvider.AddWeightDetailsDataAsync(WeightItemDetailsProxy, _cts.Token);
                    if (apiResult.Success)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _weightItemDetailsProxy = apiResult.Value;
                        });
                        return true;
                    }
                    else
                    {
                        // TODO Handle error case
                        return false;
                    }
                }
                else
                {
                    var apiResult = await _myWeightPageVmProvider.UpdateAsync(WeightItemDetailsProxy, _cts.Token);
                    if (apiResult.Success)
                    {
                        Device.BeginInvokeOnMainThread(() =>
                        {
                            _weightItemDetailsProxy = apiResult.Value;
                        });
                        return true;
                    }
                    else
                    {
                        // TODO Handle error case
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception  while saving Personal data in new wizard screen " + ex.Message + " stacktarce: " + ex.StackTrace);
                IsBusy = false;
                return false;
            }
        }

        #endregion "Save"

        #region "Step Progress"

        private void InitSteps()
        {
            Step1Progress = new OnboardingStatus()
            {
                Title = "",
                ProgressValue = 1,
                Status = StepStatus.InProgress,
                FillColor = "#FA9917"
            };
            Step2Progress = new OnboardingStatus()
            {
                Title = "1",
                ProgressValue = 0,
                Status = StepStatus.NotStarted,
                FillColor = "#BDBDBD"
            };
            Step3Progress = new OnboardingStatus()
            {
                Title = "2",
                ProgressValue = 0,
                Status = StepStatus.NotStarted,
                FillColor = "#BDBDBD"
            };
            Step4Progress = new OnboardingStatus()
            {
                Title = "3",
                ProgressValue = 0,
                Status = StepStatus.NotStarted,
                FillColor = "#BDBDBD"
            };
            Step5Progress = new OnboardingStatus()
            {
                Title = "4",
                ProgressValue = 0,
                Status = StepStatus.NotStarted,
                FillColor = "#BDBDBD"
            };
            Step6Progress = new OnboardingStatus()
            {
                Title = "5",
                ProgressValue = 0,
                Status = StepStatus.NotStarted,
                FillColor = "#BDBDBD"
            };
        }

        private void UpdateInitialStepAfterQuestionsLoaded()
        {
            Step1Progress.Total = this.Questions.Where(x => x.Category == "About You").Count();
            Step1Progress.StartIndex = 0;

            Step2Progress.Total = this.Questions.Where(x => x.Category == "Your Mindset").Count();
            Step2Progress.StartIndex = Step1Progress.Total;

            Step3Progress.Total = this.Questions.Where(x => x.Category == "Your Lifestyle").Count();
            Step3Progress.StartIndex = Step1Progress.Total + Step2Progress.Total;

            Step4Progress.Total = this.Questions.Where(x => x.Category == "Your Body").Count();
            Step4Progress.StartIndex = Step1Progress.Total + Step2Progress.Total + Step3Progress.Total;

            Step5Progress.Total = this.Questions.Where(x => x.Category == "Your Goals").Count();
            Step5Progress.StartIndex = Step1Progress.Total + Step2Progress.Total + Step3Progress.Total + Step4Progress.Total;

            Step6Progress.Total = this.Questions.Where(x => x.Category == "Your Journey").Count();
            Step6Progress.StartIndex = Step1Progress.Total + Step2Progress.Total + Step3Progress.Total + Step4Progress.Total + Step5Progress.Total;

        }

        private void CalulateSteps()
        {
            UpdateStepsStatus(Step1Progress, null);
            UpdateStepsStatus(Step2Progress, Step1Progress);
            UpdateStepsStatus(Step3Progress, Step2Progress);
            UpdateStepsStatus(Step4Progress, Step3Progress);
            UpdateStepsStatus(Step5Progress, Step4Progress);
            UpdateStepsStatus(Step6Progress, Step5Progress);

            UpdateStepColor(Step1Progress);
            UpdateStepColor(Step2Progress);
            UpdateStepColor(Step3Progress);
            UpdateStepColor(Step4Progress);
            UpdateStepColor(Step5Progress);
            UpdateStepColor(Step6Progress);
        }

        private void UpdateStepsStatus(OnboardingStatus step, OnboardingStatus previousStep)
        {
            if (step.Total > 0)
            {
                bool isInStep = false;
                if (SelectedQuestionIndex == step.StartIndex)
                {
                    step.Status = StepStatus.InProgress;
                    step.ProgressValue = 1;
                    isInStep = true;
                }

                if (SelectedQuestionIndex == (step.StartIndex + step.Total - 1))
                {
                    step.Status = StepStatus.Completed;
                    step.ProgressValue = 100;
                    isInStep = true;
                }

                if (SelectedQuestionIndex > step.StartIndex && SelectedQuestionIndex < (step.StartIndex + step.Total - 1))
                {
                    step.Status = StepStatus.InProgress;
                    step.ProgressValue = 100 * (SelectedQuestionIndex - step.StartIndex + 1) / step.Total;
                    isInStep = true;
                }
                // Always Previous Step is Completed
                if (previousStep != null && isInStep)
                {
                    previousStep.Status = StepStatus.Completed;
                    previousStep.ProgressValue = 100;
                }
            }
        }

        private static void UpdateStepColor(OnboardingStatus step)
        {
            switch (step.Status)
            {
                case StepStatus.InProgress:
                    step.FillColor = "#FA9917";
                    break;

                case StepStatus.NotStarted:
                    step.FillColor = "#BDBDBD";
                    break;

                case StepStatus.Completed:
                    step.FillColor = "#319C8A";
                    break;
            }
        }

        #endregion "Step Progress"
    }

    public class OnboardingStatus : ProxyBase
    {
        private string _title;
        private StepStatus _status;
        private int _progressValue;
        private int _total;
        private int _startIndex;
        private string _fillColor;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        public string FillColor
        {
            get => _fillColor;
            set => SetProperty(ref _fillColor, value);
        }

        public StepStatus Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        public int ProgressValue
        {
            get => _progressValue;
            set => SetProperty(ref _progressValue, value);
        }

        public int Total
        {
            get => _total;
            set => SetProperty(ref _total, value);
        }

        public int StartIndex
        {
            get => _startIndex;
            set => SetProperty(ref _startIndex, value);
        }
    }
}