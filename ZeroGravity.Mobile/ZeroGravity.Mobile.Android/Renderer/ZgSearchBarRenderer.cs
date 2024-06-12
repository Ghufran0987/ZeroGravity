using Android.Content;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZeroGravity.Mobile.CustomControls;
using ZeroGravity.Mobile.Droid.Renderer;

[assembly: ExportRenderer(typeof(ZgSearchBar), typeof(ZgSearchBarRenderer))]
namespace ZeroGravity.Mobile.Droid.Renderer
{
    public class ZgSearchBarRenderer : SearchBarRenderer
    {
        public ZgSearchBarRenderer(Context context) : base(context)
        {
            
        }

        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);

            if (Control?.GetChildAt(0) is LinearLayout linearLayout)
            {
                linearLayout = linearLayout.GetChildAt(2) as LinearLayout;
                if (linearLayout != null)
                {
                    linearLayout = linearLayout.GetChildAt(1) as LinearLayout;

                    if (linearLayout != null) linearLayout.Background = null;
                }
            }
        }
    }
}