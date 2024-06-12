using Syncfusion.XForms.ComboBox;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.CustomControls
{
    public class ZgComboBox : SfComboBox
    {
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ZgComboBox));

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty);
            set => SetValue(PaddingProperty, value);
        }
    }
}