using System;
using System.IO;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using FFImageLoading.Forms.Platform;
using MediaManager;
using Prism;
using Prism.Ioc;
using Xamarin.Forms;
using ZeroGravity.Mobile.Droid.Services;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
// using Sentry;

namespace ZeroGravity.Mobile.Droid
{
    [Activity(Theme = "@style/MainTheme",
              ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        internal static Context ActivityContext { get; private set; }
        internal static MainActivity Instance { get; private set; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            //SentryXamarin.Init(options =>
            //{
            //    options.AddXamarinFormsIntegration();

            //    // Tells which project in Sentry to send events to:
            //    options.Dsn = "https://b3f787b7a7094c21beba0ad90b6e0dda@sentry.io/6102809";

            //    // When configuring for the first time, to see what the SDK is doing:
            //    options.Debug = true;
            //    // Set TracesSampleRate to 1.0 to capture 100% of transactions for performance monitoring.
            //    // We recommend adjusting this value in production.
            //    options.TracesSampleRate = 1.0;
            //    // If you installed Sentry.Xamarin.Forms:
            //    options.Release = "MiBoKo@0.28.0";
            //    options.AttachScreenshots = true;
            //    options.AttachStacktrace = true;
            //    // Disbaled Cache as getting Method Not found exception 
            //    options.DisableOfflineCaching();
            //});

            // this "error" is a prism template issue: https://github.com/xamarin/xamarin-android/issues/4981
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            ActivityContext = this;
            Instance = this;

            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            Xamarin.Forms.Svg.Droid.SvgImage.Init(this);
            CachedImageRenderer.Init(true);
            CrossMediaManager.Current.Init(this);
            LoadApplication(new App(new AndroidInitializer()));
            
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
        {
            try
            {
                Device.BeginInvokeOnMainThread(() =>
               {
                   Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

                   base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
               });
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message + " stack trace: " + ex.StackTrace);
            }
        }

        // Field, property, and method for Picture Picker
        public static readonly int PickImageId = 1000;

        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);
        }
    }

    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            // Register any platform specific implementations
            RegisterAndroidSpecificServices(containerRegistry);
        }

        private void RegisterAndroidSpecificServices(IContainerRegistry containerRegistry)
        {
            //containerRegistry.RegisterSingleton<ILoggingService, LoggingServiceAndroidToTextFile>();
            containerRegistry.Register<IStoragePathService, StoragePathServiceAndroid>();
            containerRegistry.Register<IStepCounterService, StepCounterService>();
            containerRegistry.Register<INativeBrowserService, AndroidNativeBrowserService>();
            containerRegistry.RegisterSingleton<ILocalNotificationsService, AndroidLocalNotificationsService>();
        }
    }
}