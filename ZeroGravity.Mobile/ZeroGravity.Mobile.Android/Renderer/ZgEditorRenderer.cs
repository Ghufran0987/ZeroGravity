using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.Droid.Renderer;

[assembly: ExportRenderer(typeof(ZgEditor), typeof(ZgEditorRenderer))]
namespace ZeroGravity.Mobile.Droid.Renderer
{
    public class ZgEditorRenderer : EditorRenderer
    {
        public ZgEditorRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Editor> args)
        {
            base.OnElementChanged(args);

            if (Control != null)
            {
                Control.Background = null;

                if (Element is ZgEditor zgEditor)
                {
                    var left = (int)zgEditor.Padding.Left;
                    var top = (int)zgEditor.Padding.Top;
                    var right = (int)zgEditor.Padding.Right;
                    var bottom = (int)zgEditor.Padding.Bottom;

                    Control.SetPadding(left, top, right, bottom);
                }
            }
        }
    }
}