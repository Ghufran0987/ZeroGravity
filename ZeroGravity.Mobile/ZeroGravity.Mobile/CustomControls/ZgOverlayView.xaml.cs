using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgOverlayView : ContentView
    {
        public ZgOverlayView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty CloseOverlayCommandProperty = BindableProperty.Create(nameof(CloseOverlayCommand), typeof(ICommand), typeof(ZgOverlayView));

        public ICommand CloseOverlayCommand
        {
            get => (ICommand) GetValue(CloseOverlayCommandProperty); 
            set => SetValue(CloseOverlayCommandProperty, value);
        }
    }
}