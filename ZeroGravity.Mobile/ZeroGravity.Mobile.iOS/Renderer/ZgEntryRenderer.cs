using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.iOS.Renderer;

[assembly: ExportRenderer(typeof(ZgEntry), typeof(ZgEntryRenderer))]
namespace ZeroGravity.Mobile.iOS.Renderer
{
    public class ZgEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.None;

                if (Element is ZgEntry zgEntry)
                {
                    var left = zgEntry.Padding.Left;
                    var top = zgEntry.Padding.Top;
                    var right = zgEntry.Padding.Right;
                    var bottom = zgEntry.Padding.Bottom;

                    var height = Control.Frame.Height + top + bottom;

                    var leftView = new UIView(new CGRect(0, 0, left, height));
                    var rightView = new UIView(new CGRect(0,0, right, height));

                    Control.LeftView = leftView;
                    Control.LeftViewMode = UITextFieldViewMode.Always;
                    Control.RightView = rightView;
                    Control.RightViewMode = UITextFieldViewMode.Always;
                }

                
            }
        }
    }
}