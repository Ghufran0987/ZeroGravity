using Android.Content;
using Android.Views;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZeroGravity.Mobile.Droid.Renderer;

[assembly: ExportRenderer(typeof(WebView), typeof(CustomWebViewRenderer))]
namespace ZeroGravity.Mobile.Droid.Renderer
{
    public class CustomWebViewRenderer : WebViewRenderer
    {
        public CustomWebViewRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<WebView> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.VerticalScrollBarEnabled = false;
                Control.HorizontalScrollBarEnabled = false;
                Control.Touch += (sender, touch) =>
                {
                    if (touch.Event == null) return;
                    touch.Handled = touch.Event.Action == MotionEventActions.Move;
                };
            }
        }
    }
}