using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.Droid.Renderer;

[assembly: ExportRenderer(typeof(ZgTimePicker), typeof(ZgTimePickerRenderer))]
namespace ZeroGravity.Mobile.Droid.Renderer
{
    public class ZgTimePickerRenderer : TimePickerRenderer
    {
        public ZgTimePickerRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = null;

                if (Element is ZgTimePicker zgTimePicker)
                {
                    var left = (int)zgTimePicker.Padding.Left;
                    var top = (int)zgTimePicker.Padding.Top;
                    var right = (int)zgTimePicker.Padding.Right;
                    var bottom = (int)zgTimePicker.Padding.Bottom;

                    Control.SetPadding(left, top, right, bottom);
                }
            }
        }
    }
}