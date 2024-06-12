using Xamarin.Forms;

namespace ZeroGravity.Mobile.CustomControls
{
    public class ZgEditor : Editor
    {
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ZgEntry));

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
    }
}