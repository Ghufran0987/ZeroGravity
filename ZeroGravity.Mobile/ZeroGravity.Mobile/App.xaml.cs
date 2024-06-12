using MediaManager;
using Microsoft.Extensions.Logging;
using Prism;
using Prism.Events;
using Prism.Ioc;
using Syncfusion.Licensing;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Svg;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Base.Services;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Logging;
using ZeroGravity.Mobile.Providers;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Mobile.Services.Communication;
using ZeroGravity.Mobile.ViewModels;
using ZeroGravity.Mobile.Views;

namespace ZeroGravity.Mobile
{
    public partial class App
    {
        // dependency injection container to resolve objects
        public static IContainerProvider DiContainerProvider;

        public App(IPlatformInitializer initializer) : base(initializer)
        {
            DiContainerProvider = Container;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();

            RegisterViewsForNavigation(containerRegistry);
            RegisterProviders(containerRegistry);
            RegisterServices(containerRegistry);
        }

        private void RegisterViewsForNavigation(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginAlert, LoginAlertViewModel>(ViewName.LoginAlert);
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>(ViewName.Login);
            containerRegistry.RegisterForNavigation<GetStartedPage, GetStartedPageViewModel>(ViewName.GetStarted);
            containerRegistry.RegisterForNavigation<RegisterPage, RegisterPageViewModel>(ViewName.Register);
            containerRegistry.RegisterForNavigation<RegisterSuccessPage, RegisterSuccessPageViewModel>(ViewName.RegisterSuccessPage);
            containerRegistry.RegisterForNavigation<PasswordForgotSuccessPage, PasswordForgotSuccessPageViewModel>(ViewName.PasswordForgotSuccessPage);
            containerRegistry.RegisterForNavigation<ContentShellPage, ContentShellPageViewModel>();
            containerRegistry.RegisterForNavigation<MealsSnacksPage, MealsSnacksPageViewModel>(ViewName.MealsSnacksPage);
            containerRegistry.RegisterForNavigation<MealsSnacksBreakfastPage, MealsSnacksBreakfastPageViewModel>(ViewName.MealsSnacksBreakfastPage);
            containerRegistry.RegisterForNavigation<MealsSnacksLunchPage, MealsSnacksLunchPageViewModel>(ViewName.MealsSnacksLunchPage);
            containerRegistry.RegisterForNavigation<MealsSnacksHealthySnackPage, MealsSnacksHealthySnackPageViewModel>(ViewName.MealsSnacksHealthySnackPage);
            containerRegistry.RegisterForNavigation<MealsSnacksDinnerPage, MealsSnacksDinnerPageViewModel>(ViewName.MealsSnacksDinnerPage);
            containerRegistry.RegisterForNavigation<MealsSnacksUnhealthySnackPage, MealsSnacksUnhealthySnackPageVm>(ViewName.MealsSnacksUnhealthySnackPage);
            containerRegistry.RegisterForNavigation<PasswordForgotPage, PasswordForgotPageViewModel>(ViewName.PasswordForgot);
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>(ViewName.ProfilePage);
            containerRegistry.RegisterForNavigation<FirstUseWizardPage, FirstUseWizardPageViewModel>(ViewName.FirstUseWizard);
            containerRegistry.RegisterForNavigation<NotificationsPage, NotificationsPageViewModel>(ViewName.NotificationsPage);
            containerRegistry.RegisterForNavigation<AccountDeletePage, AccountDeletePageViewModel>(ViewName.AccountDeletePage);
            containerRegistry.RegisterForNavigation<DemoPage, DemoPageViewModel>(ViewName.DemoPage);
            containerRegistry.RegisterForNavigation<SugarBeatMainPage, SugarBeatMainPageViewModel>(ViewName.SugarBeatMainPage);
            containerRegistry.RegisterForNavigation<MetabolicHealthTrackingHistoryPage, MetabolicHealthTrackingHistoryPageViewModel>(ViewName.MetabolicHealthTrackingHistoryPage);

            containerRegistry.RegisterForNavigation<PasswordForgotPage, PasswordForgotPageViewModel>();
            containerRegistry.RegisterForNavigation<ProfilePage, ProfilePageViewModel>(ViewName.ProfilePage);
            containerRegistry.RegisterForNavigation<FirstUseWizardPage, FirstUseWizardPageViewModel>();
            containerRegistry.RegisterForNavigation<ActivitiesPage, ActivitiesPageViewModel>();
            containerRegistry.RegisterForNavigation<DayToDayPage, DayToDayPageViewModel>();
            containerRegistry.RegisterForNavigation<ExercisePage, ExercisePageViewModel>();
            containerRegistry.RegisterForNavigation<FastingDataPage, FastingDataPageViewModel>();
            containerRegistry.RegisterForNavigation<FastingSettingsPage, FastingSettingsPageViewModel>();
            containerRegistry.RegisterForNavigation<ChangeEmailPage, ChangeEmailPageViewModel>(ViewName.ChangeEmailPage);
            containerRegistry.RegisterForNavigation<ChangePasswordPage, ChangePasswordPageViewModel>(ViewName.ChangePasswordPage);
            containerRegistry.RegisterForNavigation<NoInternetConnectionPage, NoInternetConnectionPageViewModel>(ViewName.NoInternetConnectionPage);
            containerRegistry.RegisterForNavigation<CalorieDrinksAlcoholPage, CalorieDrinksAlcoholPageViewModel>(ViewName.CalorieDrinksAlcoholPage);
            containerRegistry.RegisterForNavigation<WaterIntakePage, WaterIntakePageViewModel>(ViewName.WaterIntakePage);
            containerRegistry.RegisterForNavigation<StepCountPage, StepCountPageViewModel>(ViewName.StepCountPage);
            containerRegistry.RegisterForNavigation<WellbeingDataPage, WellbeingDataPageViewModel>(ViewName.WellbeingDataPage);
            containerRegistry.RegisterForNavigation<SugarBeatScanPage, SugarBeatScanPageViewModel>(ViewName.SugarBeatScanPage);
            containerRegistry.RegisterForNavigation<IntegrationDetailPage, IntegrationDetailPageViewModel>(ViewName.IntegrationDetailPage);
            containerRegistry.RegisterForNavigation<ActivitySyncOverviewPage, ActivitySyncOverviewPageViewModel>(ViewName.ActivitySyncOverviewPage);
            containerRegistry.RegisterForNavigation<ActivitySyncDetailPage, ActivitySyncDetailPageViewModel>(ViewName.ActivitySyncDetailPage);
            containerRegistry.RegisterForNavigation<CoachingDetailPage, CoachingDetailPageViewModel>(ViewName.ConsultingDetailPage);
            containerRegistry.RegisterForNavigation<ResultPage, ResultPageViewModel>(ViewName.ResultPage);
            containerRegistry.RegisterForNavigation<HeartBeatPage, HeartBeatPageViewModel>(ViewName.HeartBeatPage);
            containerRegistry.RegisterForNavigation<MeditationAreaPage, MeditationAreaPageViewModel>(ViewName.MeditationAreaPage);
            containerRegistry.RegisterForNavigation<MediaElementViewPage, MediaElementViewPageViewModel>(ViewName.MediaElementViewPage);
            containerRegistry.RegisterForNavigation<EnduranceTrackingPage, EnduranceTrackingPageViewModel>(ViewName.EnduranceTrackingPage);
            containerRegistry.RegisterForNavigation<StreamingPage, StreamingPageViewModel>(ViewName.StreamingPage);
            containerRegistry.RegisterForNavigation<FeedbackUpdateMeasurementsPage, PersonalDataPageViewModel>(ViewName.FeedbackUpdateMeasurementsPage);
            containerRegistry.RegisterForNavigation<MyWeightPage, MyWeightPageViewModel>(ViewName.MyWeightPage);

            containerRegistry.RegisterForNavigation<WizardStartPage, WizardStartPageViewModel>(ViewName.WizardStartPage);
            containerRegistry.RegisterForNavigation<WizardNewPage, WizardNewPageViewModel>(ViewName.WizardNewPage);
            containerRegistry.RegisterForNavigation<WizardStep1Page, WizardStep1PageViewModel>(ViewName.WizardStep1Page);
            containerRegistry.RegisterForNavigation<WizardStep2Page, WizardStep2PageViewModel>(ViewName.WizardStep2Page);
            containerRegistry.RegisterForNavigation<WizardStep3Page, WizardStep3PageViewModel>(ViewName.WizardStep3Page);
            containerRegistry.RegisterForNavigation<WizardStep4Page, WizardStep4PageViewModel>(ViewName.WizardStep4Page);
            containerRegistry.RegisterForNavigation<WizardStep5Page, WizardStep5PageViewModel>(ViewName.WizardStep5Page);
            containerRegistry.RegisterForNavigation<WizardFinishSetupPage, WizardFinishSetupPageViewModel>(ViewName.WizardFinishSetupPage);

            containerRegistry.RegisterForNavigation<SugarBeatConnectPage, SugarBeatConnectPageViewModel>(ViewName.SugarBeatConnectPage);
            containerRegistry.RegisterForNavigation<MetabolicHealthPage, MetabolicHealthPageViewModel>(ViewName.MetabolicHealthPage);
            containerRegistry.RegisterForNavigation<TermsAndPrivacyPage, TermsAndPrivacyPageViewModel>(ViewName.TermsAndPrivacyPage);

            containerRegistry.RegisterForNavigation<VideoUploadPage, VideoUploadPageViewModel>(ViewName.VideoUploadPage);
            containerRegistry.RegisterForNavigation<GeneralSuccessPage, GeneralSuccessPageViewModel>(ViewName.GeneralSuccessPage);
        }

        private void RegisterProviders(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IMainPageVmProvider, MainPageVmProvider>();
            containerRegistry.Register<ILoginPageVmProvider, LoginPageVmProvider>();
            containerRegistry.Register<ILoginAlertPageVmProvider, LoginAlertPageVmProvider>();
            containerRegistry.Register<IGetStartedPageVmProvider, GetStartedPageVmProvider>();
            containerRegistry.Register<IRegisterPageVmProvider, RegisterPageVmProvider>();
            containerRegistry.Register<IRegisterSuccessPageVmProvider, RegisterSuccessPageVmProvider>();
            containerRegistry.Register<IPasswordForgotSuccessPageVmProvider, PasswordForgotSuccessPageVmProvider>();
            containerRegistry.Register<IContentShellPageVmProvider, ContentShellPageVmProvider>();
            containerRegistry.Register<IIntegrationsPageVmProvider, IntegrationsPageVmProvider>();
            containerRegistry.Register<IAboutPageVmProvider, AboutPageVmProvider>();
            containerRegistry.Register<IProfileImagePageVmProvider, ProfileImagePageVmProvider>();
            containerRegistry.Register<IPersonalDataPageVmProvider, PersonalDataPageVmProvider>();
            containerRegistry.Register<IPasswordForgotPageVmProvider, PasswordForgotPageVmProvider>();
            containerRegistry.Register<IDietPreferencesPageVmProvider, DietPreferencesPageVmProvider>();
            containerRegistry.Register<IMedicalPreConditionsPageVmProvider, MedicalPreConditionsPageVmProvider>();
            containerRegistry.Register<IFirstUseWizardPageVmProvider, FirstUseWizardPageVmProvider>();
            containerRegistry.Register<IAccountOverviewPageVmProvider, AccountOverviewPageVmProvider>();
            containerRegistry.Register<IProfilePageVmProvider, ProfilePageVmProvider>();
            containerRegistry.Register<IActivitiesPageVmProvider, ActivitiesPageVmProvider>();
            containerRegistry.Register<IDayToDayPageVmProvider, DayToDayPageVmProvider>();
            containerRegistry.Register<IExercisePageVmProvider, ExercisePageVmProvider>();
            containerRegistry.Register<IPersonalGoalsPageVmProvider, PersonalGoalsPageVmProvider>();
            containerRegistry.Register<IMealsSnacksPageVmProvider, MealsSnacksPageVmProvider>();
            containerRegistry.Register<IMealsSnacksBreakfastPageVmProvider, MealsSnacksBreakfastPageVmProvider>();
            containerRegistry.Register<IMealsSnacksLunchPageVmProvider, MealsSnacksLunchPageVmProvider>();
            containerRegistry.Register<IMealsSnacksHealthySnackPageVmProvider, MealsSnacksHealthySnackPageVmProvider>();
            containerRegistry.Register<IMealsSnacksDinnerPageVmProvider, MealsSnacksDinnerPageVmProvider>();
            containerRegistry.Register<IMealsSnacksUnhealthySnackPageVmProvider, MealsSnacksUnhealthySnackPageVmProvider>();
            containerRegistry.Register<INotificationPageVmProvider, NotificationPageVmProvider>();
            containerRegistry.Register<IAccountSecurityPageVmProvider, AccountSecurityPageVmProvider>();
            containerRegistry.Register<IAccountDeletePageVmProvider, AccountDeletePageVmProvider>();
            containerRegistry.Register<IAnalysisPageVmProvider, AnalysisPageVmProvider>();
            containerRegistry.Register<IFastingDataPageVmProvider, FastingDataPageVmProvider>();
            containerRegistry.Register<IFastingSettingsPageVmProvider, FastingSettingsPageVmProvider>();
            containerRegistry.Register<IChangeEmailPageVmProvider, ChangeEmailPageVmProvider>();
            containerRegistry.Register<IChangePasswordPageVmProvider, ChangePasswordPageVmProvider>();
            containerRegistry.Register<IFeedbackPageVmProvider, FeedbackPageVmProvider>();
            containerRegistry.Register<ICoachingDetailPageVmProvider, CoachingDetailPageVmProvider>();
            containerRegistry.Register<IStreamingPageVmProvider, StreamingPageVmProvider>();
            containerRegistry.Register<IDemoPageVmProvider, DemoPageVmProvider>();
            containerRegistry.Register<INoInternetConnectionPageVmProvider, NoInternetConnectionVmProvider>();
            containerRegistry.Register<ICalorieDrinksAlcoholPageVmProvider, CalorieDrinksAlcoholPageVmProvider>();
            containerRegistry.Register<IWaterIntakePageVmProvider, WaterIntakePageVmProvider>();
            containerRegistry.Register<IStepCountPageVmProvider, StepCountPageVmProvider>();
            containerRegistry.Register<IWellbeingDataPageVmProvider, WellbeingDataPageVmProvider>();
            containerRegistry.Register<IIntegrationDetailPageVmProvider, IntegrationDetailPageVmProvider>();
            containerRegistry.Register<IActivitySyncOverviewPageVmProvider, ActivitySyncOverviewPageVmProvider>();
            containerRegistry.Register<IActivitySyncDetailPageVmProvider, ActivitySyncDetailPageVmProvider>();
            containerRegistry.Register<ISugarBeatMainPageVmProvider, SugarBeatMainPageVmProvider>();
            containerRegistry.Register<ISugarBeatScanPageVmProvider, SugarBeatScanPageVmProvider>();
            containerRegistry.Register<IResultPageVmProvider, ResultPageVmProvider>();
            containerRegistry.Register<IHeartBeatPageVmProvider, HeartBeatPageVmProvider>();
            containerRegistry.Register<IMeditationAreaPageVmProvider, MeditationAreaPageVmProvider>();
            containerRegistry.Register<IEnduranceTrackingPageVmProvider, EnduranceTrackingPageVmProvider>();
            containerRegistry.Register<IMonthlyReportPageVmProvider, MonthlyReportPageVmProvider>();
            containerRegistry.Register<IMyWeightPageVmProvider, MyWeightPageVmProvider>();

            containerRegistry.Register<IWizardNewPageVmProvider, WizardNewPageVmProvider>();
            containerRegistry.Register<IWizardStartPageVmProvider, WizardStartPageVmProvider>();
            containerRegistry.Register<IWizardStep3PageVmProvider, WizardStep3PageVmProvider>();
            containerRegistry.Register<IWizardFinishSetupPageVmProvider, WizardFinishSetupPageVmProvider>();

            containerRegistry.Register<ISugarBeatConnectPageVmProvider, SugarBeatConnectPageVmProvider>();
            containerRegistry.Register<IMetabolicHealthPageVmProvider, MetabolicHealthPageVmProvider>();
            containerRegistry.Register<ITermsAndPrivacyPageVmProvider, TermsAndPrivacyPageVmProvider>();
            containerRegistry.Register<IVideoUploadPageVmProvider, VideoUploadPageVmProvider>();
            containerRegistry.Register<IGeneralSuccessPageVmProvider, GeneralSuccessPageVmProvider>();
            containerRegistry.Register<IEducationalInfoProvider, EducationalInfoProvider>();
            containerRegistry.Register<ISugarBeatEatingSessionProvider, SugarBeatEatingSessionProvider>();


        }

        private void RegisterServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ILoggerFactory, AppLoggerFactory>();
            containerRegistry.RegisterSingleton<IBleService, BleService>();

            containerRegistry.Register(typeof(IVmCommonService), typeof(VmCommonService));
            containerRegistry.Register<ILoggingService, LoggingService>();
            containerRegistry.Register<IApiService, ApiService>();
            containerRegistry.Register<ISecureStorageService, SecureStorageService>();
            containerRegistry.Register<ITokenService, TokenService>();
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<IUserDataService, UserDataService>();
            containerRegistry.Register<IRegisterService, RegisterService>();
            containerRegistry.Register<IPasswordForgotService, PasswordForgotService>();
            containerRegistry.Register<IMediaService, MediaService>();
            containerRegistry.Register<IProfileImageService, ProfileImageService>();
            containerRegistry.Register<IActivityDataService, ActivityDataService>();
            containerRegistry.Register<IAccountSecurityService, AccountSecurityService>();
            containerRegistry.Register<IFastingDataService, FastingDataService>();
            containerRegistry.Register<IFastingSettingsService, FastingSettingService>();
            containerRegistry.Register<ILiquidIntakeDataService, LiquidIntakeDataService>();
            containerRegistry.Register<IBadgeInformationService, BadgeInformationService>();
            containerRegistry.Register<IStepCountDataService, StepCountDataService>();
            containerRegistry.Register<IWellbeingDataService, WellbeingDataService>();
            containerRegistry.Register<IIntegrationDataService, IntegrationDataService>();
            containerRegistry.Register<IAlgorithmService, AlgorithmService>();
            containerRegistry.Register<IFitbitIntegrationService, FitbitIntegrationService>();
            containerRegistry.Register<IFeedbackService, FeedbackService>();
            containerRegistry.Register<IAnalysisSummaryDataService, AnalysisSummaryDataService>();
            containerRegistry.Register<ITrackedHistoryService, TrackedHistoryService>();
            containerRegistry.Register<IGlucoseService, GlucoseService>();
            containerRegistry.Register<IMeditationService, MeditationService>();

            containerRegistry.RegisterSingleton<INotificationService, NotificationService>();
            containerRegistry.Register<IPermissionService, PermissionService>();
            containerRegistry.Register<IHoldingPagesSettingsService, HoldingPagesSettingsService>();

            containerRegistry.RegisterSingleton<ISemaphoreAsyncLockService, SemaphoreAsyncLockService>();
            containerRegistry.RegisterSingleton<IProgressSummaryDataService, ProgressSummaryDataService>();

            containerRegistry.RegisterSingleton<ISugarBeatAlertService, SugarBeatAlertService>();
            containerRegistry.RegisterSingleton<ISugarBeatGlucoseService, SugarBeatGlucoseService>();
            containerRegistry.RegisterSingleton<ISugarBeatSessionService, SugarBeatSessionService>();
            containerRegistry.RegisterSingleton<ISugarBeatSettingsSevice, SugarbeatSettingsService>();
            containerRegistry.RegisterSingleton<ISugarBeatEatingSessionService, SugarBeatEatingSessionService>();

            containerRegistry.RegisterSingleton<IWizardNewPageDataService, WizardNewPageDataService>();
            containerRegistry.RegisterSingleton<IWeightDataService, WeightDataService>();
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            Device.SetFlags(new[] { "Shapes_Experimental", "Brush_Experimental" });

            InitSyncfusion();
            CrossMediaManager.Current.Init();
            await NavigationService.NavigateAsync(ViewName.ContentShellPage);
        }

        private void InitSyncfusion()
        {
            // syncfusion license key for v. 18.2.0.56
            //SyncfusionLicenseProvider.RegisterLicense("MzExMzM1QDMxMzgyZTMyMmUzMEdaRzJMRm1kR2NYakVQdGljZTg3eFZSUElzejAzRE56SFYzL0dvT3ZleXc9");
            //syncfusion license key for v. 18.3.0.35
            SyncfusionLicenseProvider.RegisterLicense("MzI2MTYzQDMxMzgyZTMzMmUzMFdrenkxN1dtKzB2aXZZeG5JSngvRUw2UytOMFBOZDlCRVE1S25HbmgrbTA9");
            // in order to use svgs in Syncfusion buttons
            SvgImageSource.RegisterAssembly();
        }

        protected override void OnStart()
        {
            var eventAggregator = DiContainerProvider.Resolve<IEventAggregator>();
            // Handle when your app resumes
            if (eventAggregator != null)
            {
                eventAggregator.GetEvent<AppResumedEvent>().Publish();
            }
         
            // Handle when your app starts
            base.OnStart();

            Microsoft.AppCenter.AppCenter.Start("ios=41841939-9a43-4d9d-ad8d-6026b59368da;" +
               "android=88a592cb-1112-4043-b13c-d8e68fa488ba;",
                typeof(Microsoft.AppCenter.Crashes.Crashes));

        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
            base.OnSleep();
        }

        protected override void OnResume()
        {
            var eventAggregator = DiContainerProvider.Resolve<IEventAggregator>();
            // Handle when your app resumes
            if (eventAggregator != null)
            {
                eventAggregator.GetEvent<AppResumedEvent>().Publish();
            }
            base.OnResume();
        }
    }
}