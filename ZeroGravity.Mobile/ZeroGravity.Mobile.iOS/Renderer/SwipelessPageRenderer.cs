using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ZeroGravity.Mobile.iOS.Renderer;

[assembly: ExportRenderer(typeof(ContentPage), typeof(SwipelessPageRenderer))]
namespace ZeroGravity.Mobile.iOS.Renderer
{
    public class SwipelessPageRenderer : PageRenderer
    {
        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ViewController != null)
            {
                if (ViewController.NavigationController != null)
                {
                    ViewController.NavigationController.InteractivePopGestureRecognizer.Enabled = false;
                }
            }
        }
    }
}