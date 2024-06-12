using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.iOS.Renderer;

[assembly: ExportRenderer(typeof(ZgDatePicker), typeof(ZgDatePickerRenderer))]
namespace ZeroGravity.Mobile.iOS.Renderer
{
    public class ZgDatePickerRenderer : DatePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<DatePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;

                if (Element is ZgDatePicker zgDatePicker)
                {
                    var left = zgDatePicker.Padding.Left;
                    var top = zgDatePicker.Padding.Top;
                    var right = zgDatePicker.Padding.Right;
                    var bottom = zgDatePicker.Padding.Bottom;

                    var height = Control.Frame.Height + top + bottom;

                    var leftView = new UIView(new CGRect(0, 0, left, height));
                    var rightView = new UIView(new CGRect(0, 0, right, height));

                    Control.LeftView = leftView;
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                    Control.RightView = rightView;
                    Control.RightViewMode = UITextFieldViewMode.Always;
                }
            }
        }
    }
}