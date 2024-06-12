using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.iOS.Renderer;

[assembly: ExportRenderer(typeof(ZgEntry), typeof(ZgEntryRenderer))]
namespace ZeroGravity.Mobile.iOS.Renderer
{
    public class ZgSearchBarRenderer : SearchBarRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.SearchTextField.BorderStyle = UITextBorderStyle.None;
            }
        }
    }
}