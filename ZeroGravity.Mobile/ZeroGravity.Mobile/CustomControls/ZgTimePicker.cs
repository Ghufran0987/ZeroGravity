using Xamarin.Forms;

namespace ZeroGravity.Mobile.CustomControls
{
    public class ZgTimePicker : TimePicker
    {
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ZgTimePicker));

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
    }
}