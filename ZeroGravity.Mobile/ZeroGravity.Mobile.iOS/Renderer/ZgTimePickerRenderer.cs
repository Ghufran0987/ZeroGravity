using System;
using System.ComponentModel;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.iOS.Renderer;

[assembly: ExportRenderer(typeof(ZgTimePicker), typeof(ZgTimePickerRenderer))]
namespace ZeroGravity.Mobile.iOS.Renderer
{
    public class ZgTimePickerRenderer : TimePickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<TimePicker> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;

                if (Element is ZgTimePicker zgTimePicker)
                {
                    var left = zgTimePicker.Padding.Left;
                    var top = zgTimePicker.Padding.Top;
                    var right = zgTimePicker.Padding.Right;
                    var bottom = zgTimePicker.Padding.Bottom;

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

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            var property = e.PropertyName;

            if (property.Equals("Time"))
            {
                string controlText = Control?.Text;

                if (controlText != null)
                {
                    const string amDesignatorInUnitedKingdom = "yb"; // is shown instead of "am" when iOS region settings set to "United Kingdom"
                    const string pmDesignatorInUnitedKingdom = "yh"; // is shown instead of "pm" when iOS region settings set to "United Kingdom"

                    if (controlText.Contains(amDesignatorInUnitedKingdom, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Control.Text = controlText.Replace(amDesignatorInUnitedKingdom, "AM");
                    }

                    if (controlText.Contains(pmDesignatorInUnitedKingdom, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Control.Text = controlText.Replace(pmDesignatorInUnitedKingdom, "PM");
                    }
                }
            }
        }
    }
}