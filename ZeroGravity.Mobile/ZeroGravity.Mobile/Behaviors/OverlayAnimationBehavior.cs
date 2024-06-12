using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.Behaviors
{
    public class OverlayAnimationBehavior : Behavior<View>
    {

        public static readonly BindableProperty IsVisibleProperty = BindableProperty.Create(nameof(IsVisible), typeof(bool), typeof(OverlayAnimationBehavior));

        public bool IsVisible
        {
            get => (bool) GetValue(IsVisibleProperty); 
            set => SetValue(IsVisibleProperty, value);
        }

        private View _associatedObject;

        protected override void OnAttachedTo(View view)
        {
            base.OnAttachedTo(view);

            _associatedObject = view;
            var y = DeviceDisplay.MainDisplayInfo.Height;
            TranslateY(y);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (_associatedObject == null)
            {
                return;
            }

            if (propertyName == nameof(IsVisible))
            {
                if (IsVisible)
                {
                    TranslateY(-0);
                }
            }
        }
        
        private void TranslateY(double y)
        {
            var x = _associatedObject.X;
            _associatedObject.TranslateTo(x, y, 800, Easing.Linear);
        }
    }
}
