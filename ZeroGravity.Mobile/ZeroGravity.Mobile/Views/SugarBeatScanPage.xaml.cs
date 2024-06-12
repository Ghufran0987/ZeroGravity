using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SugarBeatScanPage : ISugarBeatScanPage
    {
        public SugarBeatScanPage()
        {
            InitializeComponent();
        }

        private SugarBeatScanPageViewModel vm;

        protected override void OnAppearing()
        {
            base.OnAppearing();

            vm = BindingContext as SugarBeatScanPageViewModel;
            if (vm != null)
            {
                vm.PropertyChanged += Vm_PropertyChanged;
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            if (vm != null)
            {
                vm.PropertyChanged -= Vm_PropertyChanged;
            }
            ltDevice.StopAnimation();
            // ViewExtensions.CancelAnimations(this);
        }

        private Animation animation;
        private bool _isAnimationRunning;

        private async void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "SugerBeatSearch":
                case "SugarBeatReconnecting":
                    if (vm.SugerBeatSearch || vm.SugarBeatReconnecting)
                    {
                        _isAnimationRunning = true;
                        ltDevice.PlayAnimation();
                        //if (animation == null)
                        //{
                        //    animation = new Animation(v => imgDevice.Scale = v, 1, 1.2);
                        //}
                        //animation.Commit(this, "ScaleIt", length: 2000, easing: Easing.Linear,
                        //    finished: (v, c) => imgDevice.Scale = 1, repeat: () => _isAnimationRunning);
                    }
                    else
                    {
                        ltDevice.StopAnimation();
                        _isAnimationRunning = false;
                    }
                    break;
            }
        }
    }
}