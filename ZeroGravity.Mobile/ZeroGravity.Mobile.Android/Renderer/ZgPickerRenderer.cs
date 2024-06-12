using System;
using System.Reflection;
using Android.Content;
using Syncfusion.XForms.Android.ComboBox;
using Syncfusion.XForms.ComboBox;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.Droid.Renderer;

[assembly: ExportRenderer(typeof(ZgPicker), typeof(ZgPickerRenderer))]
namespace ZeroGravity.Mobile.Droid.Renderer
{
    public class ZgPickerRenderer : PickerRenderer
    {
        public ZgPickerRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Picker> args)
        {
            base.OnElementChanged(args);

            if (Control != null)
            {
                Control.Background = null;
                
                if (Element is ZgPicker zgEntry)
                {
                    var left = (int)zgEntry.Padding.Left;
                    var top = (int)zgEntry.Padding.Top;
                    var right = (int)zgEntry.Padding.Right;
                    var bottom = (int)zgEntry.Padding.Bottom;
                    
                    Control.SetPadding(left, top, right, bottom);
                }
            }
        }
    }
}