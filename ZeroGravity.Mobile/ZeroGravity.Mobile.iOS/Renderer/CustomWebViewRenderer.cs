using WebKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.iOS.Renderer;

[assembly: ExportRenderer(typeof(ShowHeroesWebView), typeof(CustomWebViewRenderer))]
namespace ZeroGravity.Mobile.iOS.Renderer
{
    public class CustomWebViewRenderer : WkWebViewRenderer
    {
        public CustomWebViewRenderer() : base(
            new WKWebViewConfiguration
            {
                AllowsInlineMediaPlayback = true
            })
        {
            
        }

        protected override void OnElementChanged(VisualElementChangedEventArgs e)
        {
            base.OnElementChanged(e);

            ScrollView.ScrollEnabled = false;
            ScrollView.Bounces = false;
        }
    }
}