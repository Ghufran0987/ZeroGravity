using Android.App;
using Android.Support.CustomTabs;
using Xamarin.Forms;
using ZeroGravity.Mobile.Droid.Services;
using ZeroGravity.Mobile.Interfaces;

[assembly: Dependency(typeof(AndroidNativeBrowserService))]
namespace ZeroGravity.Mobile.Droid.Services
{
    public class AndroidNativeBrowserService : INativeBrowserService
    {
        public void LaunchNativeEmbeddedBrowser(string url)
        {
            var activity = Forms.Context as Activity;
 
            if (activity == null) return;
 
            var mgr = new CustomTabsActivityManager(activity);
            mgr.CustomTabsServiceConnected += delegate {
                mgr.LaunchUrl(url);
            };
 
            mgr.BindService();
        }
    }
}