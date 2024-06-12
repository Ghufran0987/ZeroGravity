using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [Obsolete]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgPageWrapper : ContentView
    {
        public ZgPageWrapper()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ShowBusyIndicatorProperty = BindableProperty.Create(nameof(ShowBusyIndicator), typeof(bool), typeof(ZgPageWrapper), false);

        public bool ShowBusyIndicator { 
            get => (bool)GetValue(ShowBusyIndicatorProperty); 
            set => SetValue(ShowBusyIndicatorProperty, value);
        }

        public static readonly BindableProperty ShowTopBorderProperty = BindableProperty.Create(nameof(ShowTopBorder), typeof(bool), typeof(ZgPageWrapper), true);

        public bool ShowTopBorder { 
            get => (bool)GetValue(ShowTopBorderProperty); 
            set => SetValue(ShowTopBorderProperty, value);
        }

        public static readonly BindableProperty ShowBottomBorderProperty = BindableProperty.Create(nameof(ShowBottomBorder), typeof(bool), typeof(ZgPageWrapper));

        public bool ShowBottomBorder
        {
            get => (bool)GetValue(ShowBottomBorderProperty); 
            set => SetValue(ShowBottomBorderProperty, value);
        }
    }
}