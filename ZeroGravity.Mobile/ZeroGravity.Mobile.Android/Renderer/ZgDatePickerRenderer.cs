using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.Droid.Renderer;

[assembly: ExportRenderer(typeof(ZgDatePicker), typeof(ZgDatePickerRenderer))]
namespace ZeroGravity.Mobile.Droid.Renderer
{
    public class ZgDatePickerRenderer : DatePickerRenderer
    {
        public ZgDatePickerRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Background = null;

                if (Element is ZgDatePicker zgDatePicker)
                {
                    var left = (int)zgDatePicker.Padding.Left;
                    var top = (int)zgDatePicker.Padding.Top;
                    var right = (int)zgDatePicker.Padding.Right;
                    var bottom = (int)zgDatePicker.Padding.Bottom;

                    Control.SetPadding(left, top, right, bottom);
                }
            }
        }
    }
}