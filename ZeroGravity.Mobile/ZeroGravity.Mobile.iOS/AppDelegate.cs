using FFImageLoading.Forms.Platform;
using Foundation;
using MediaManager;
using Prism;
using Prism.Ioc;
// using Sentry;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfBusyIndicator.XForms.iOS;
using Syncfusion.SfChart.XForms.iOS.Renderers;
using Syncfusion.SfGauge.XForms.iOS;
using Syncfusion.SfNumericTextBox.XForms.iOS;
using Syncfusion.SfRangeSlider.XForms.iOS;
using Syncfusion.SfRotator.XForms.iOS;
using Syncfusion.XForms.iOS.BadgeView;
using Syncfusion.XForms.iOS.Buttons;
using Syncfusion.XForms.iOS.Cards;
using Syncfusion.XForms.iOS.ComboBox;
using Syncfusion.XForms.iOS.ProgressBar;
using Syncfusion.XForms.iOS.RichTextEditor;
using Syncfusion.XForms.iOS.Shimmer;
using Syncfusion.XForms.iOS.TabView;
using Syncfusion.XForms.iOS.TextInputLayout;
using Syncfusion.XForms.Pickers.iOS;
using UIKit;
using UserNotifications;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.Svg.iOS;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.iOS.Services;

namespace ZeroGravity.Mobile.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            //SentryXamarin.Init(opt =>
            //{
            //    opt.Dsn = "https://b3f787b7a7094c21beba0ad90b6e0dda@o1088181.ingest.sentry.io/6102809";
            //    // When configuring for the first time, to see what the SDK is doing:
            //    opt.AddXamarinFormsIntegration();
            //    opt.Debug = true;
            //    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
            //    // We recommend adjusting this value in production.
            //    opt.TracesSampleRate = 1.0;
            //    opt.Release = "MiBoKo@0.28.0";
            //    opt.AttachScreenshots = true;
            //    opt.AttachStacktrace = true;
            //    opt.DisableOfflineCaching();
            //});
            Forms.Init();
            SfTabViewRenderer.Init();
            SfComboBoxRenderer.Init();
            SfTextInputLayoutRenderer.Init();
            SfTimePickerRenderer.Init();
            SfDatePickerRenderer.Init();
            SvgImage.Init();
            SfCardViewRenderer.Init();
            SfButtonRenderer.Init();
            SfBusyIndicatorRenderer.Init();
            SfRangeSliderRenderer.Init();
            SfBadgeViewRenderer.Init();
            SfListViewRenderer.Init();
            SfLinearGaugeRenderer.Init();
            SfRichTextEditorRenderer.Init();
            SfNumericTextBoxRenderer.Init();
            SfCircularProgressBarRenderer.Init();
            SfShimmerRenderer.Init();
            CachedImageRenderer.Init();
            new SfRotatorRenderer();
            SfLinearProgressBarRenderer.Init();
            SfChartRenderer.Init();
            UNUserNotificationCenter.Current.Delegate = new IosNotificationReceiver();
            CrossMediaManager.Current.Init();
            LoadApplication(new App(new iOSInitializer()));

            return base.FinishedLaunching(app, options);
        }
    }

    public class iOSInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            RegisterIosSpecificServices(containerRegistry);
        }

        private void RegisterIosSpecificServices(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IStoragePathService, StoragePathServiceIos>();
            containerRegistry.Register<IStepCounterService, StepManagerService>();
            containerRegistry.Register<INativeBrowserService, AppleNativeBrowserService>();
            containerRegistry.RegisterSingleton<ILocalNotificationsService, IosLocalNotificationsService>();
        }
    }
}