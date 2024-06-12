using Xamarin.Forms;

namespace ZeroGravity.Mobile.CustomControls
{
    public class ZgEntry : Entry
    {
        public static readonly BindableProperty PaddingProperty = BindableProperty.Create(nameof(Padding), typeof(Thickness), typeof(ZgEntry));

        public Thickness Padding
        {
            get => (Thickness)GetValue(PaddingProperty); 
            set => SetValue(PaddingProperty, value);
        }
        public ZgEntry()
        {
            IsTextPredictionEnabled = true;
            IsSpellCheckEnabled = true;
        }
    }
}
