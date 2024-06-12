using Xamarin.Forms;

namespace ZeroGravity.Mobile.CustomControls
{
    public class ZgPicker : Picker
    {
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ZgPicker));

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
    }
}
