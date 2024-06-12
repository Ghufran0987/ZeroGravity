using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.Droid.Renderer;

[assembly: ExportRenderer(typeof(ZgEntry), typeof(ZgEntryRenderer))]
namespace ZeroGravity.Mobile.Droid.Renderer
{
    public class ZgEntryRenderer : EntryRenderer
    {
        public ZgEntryRenderer(Context context) : base(context)
        {
            
        }
        
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> args)
        {
            base.OnElementChanged(args);

            if (Control != null)
            {
                Control.Background = null;

                if (Element is ZgEntry zgEntry)
                {
                    var left = (int) zgEntry.Padding.Left;
                    var top = (int) zgEntry.Padding.Top;
                    var right = (int) zgEntry.Padding.Right;
                    var bottom = (int) zgEntry.Padding.Bottom;
                    
                    Control.SetPadding(left, top, right, bottom);
                }
            }
        }
    }
}