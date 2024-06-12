using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace ZeroGravity.Mobile.Droid
{
    // default SplashActivity created by Prism Template
    [Activity(Theme = "@style/MainTheme.Splash", MainLauncher = true, NoHistory = true, ScreenOrientation = ScreenOrientation.Portrait)]
    public class SplashActivity : AppCompatActivity
    {

        protected override void OnResume()
        {
            base.OnResume();
            StartActivity(new Intent(Application.Context, typeof(MainActivity)));
        }
    }

    //[Activity(Theme = "@android:style/Theme.NoTitleBar", MainLauncher = true, NoHistory = true, Immersive = true, ScreenOrientation = ScreenOrientation.Portrait)]
    //public class SplashScreenActivity : Activity
    //{
    //    protected override void OnCreate(Bundle savedInstanceState)
    //    {
    //        base.OnCreate(savedInstanceState);
    //        SetContentView(Resource.Layout.SplashScreen);
    //    }


    //    protected override void OnResume()
    //    {
    //        base.OnResume();

    //        Task startupWork = new Task(async() =>
    //        {
    //            await Task.Delay(2000);
    //            StartupWork();
    //        });
    //        startupWork.Start();
    //    }

    //    private void StartupWork()
    //    {
    //        // start main activity
    //        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    //    }

    //    // prevent the back button from canceling the startup process
    //    public override void OnBackPressed()
    //    {

    //    }
    //}
}
