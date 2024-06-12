using Foundation;
using SafariServices;
using UIKit;
using Xamarin.Forms;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.iOS.Services;

[assembly: Dependency(typeof(AppleNativeBrowserService))]

namespace ZeroGravity.Mobile.iOS.Services
{
    public class AppleNativeBrowserService : INativeBrowserService
    {
        public void LaunchNativeEmbeddedBrowser(string url)
        {
            var destination = new NSUrl(url);
            var sfViewController = new SFSafariViewController(destination);

            var window = UIApplication.SharedApplication.KeyWindow;

            // TODO: Dangerous? Genuinely not sure if this will work after navigating
            var controller = window.RootViewController;

            controller?.PresentViewController(sfViewController, true, null);
        }
    }
}