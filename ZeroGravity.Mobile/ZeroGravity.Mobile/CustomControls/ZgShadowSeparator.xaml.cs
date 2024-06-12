using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgShadowSeparator : ContentView
    {
        public ZgShadowSeparator()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty ThicknessProperty = BindableProperty.Create(nameof(Thickness), typeof(float), typeof(ZgShadowSeparator));

        public float Thickness
        {
            get => (float)GetValue(ThicknessProperty); 
            set => SetValue(ThicknessProperty, value);
        }

        public static readonly BindableProperty GradientStartColorProperty = BindableProperty.Create(nameof(GradientStartColor), typeof(Color), typeof(ZgShadowSeparator));

        public Color GradientStartColor
        {
            get => (Color)GetValue(GradientStartColorProperty); 
            set => SetValue(GradientStartColorProperty, value);
        }

        public static readonly BindableProperty GradientStopColorProperty = BindableProperty.Create(nameof(GradientStopColor), typeof(Color), typeof(ZgShadowSeparator));

        public Color GradientStopColor
        {
            get => (Color)GetValue(GradientStopColorProperty); 
            set => SetValue(GradientStopColorProperty, value);
        }
    }
}