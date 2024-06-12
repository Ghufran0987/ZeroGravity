using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.iOS.Renderer;

[assembly: ExportRenderer(typeof(ZgEditor), typeof(ZgEditorRenderer))]
namespace ZeroGravity.Mobile.iOS.Renderer
{
    public class ZgEditorRenderer : EditorRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Editor> e)
        {
            base.OnElementChanged(e);

            if (Control?.TextInputView is UITextField textField)
            {
                textField.BorderStyle = UITextBorderStyle.None;

                if (Element is ZgEditor zgEditor)
                {
                    var left = zgEditor.Padding.Left;
                    var top = zgEditor.Padding.Top;
                    var right = zgEditor.Padding.Right;
                    var bottom = zgEditor.Padding.Bottom;

                    var height = Control.Frame.Height + top + bottom;

                    var leftView = new UIView(new CGRect(0, 0, left, height));
                    var rightView = new UIView(new CGRect(0, 0, right, height));

                    textField.LeftView = leftView;
                    textField.LeftViewMode = UITextFieldViewMode.Always;
                    textField.RightView = rightView;
                    textField.RightViewMode = UITextFieldViewMode.Always;
                }
            }
        }
    }
}