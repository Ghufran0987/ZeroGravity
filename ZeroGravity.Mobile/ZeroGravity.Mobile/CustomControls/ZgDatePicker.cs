using Xamarin.Forms;

namespace ZeroGravity.Mobile.CustomControls
{
    public class ZgDatePicker : DatePicker
    {
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ZgDatePicker));

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
    }
}
