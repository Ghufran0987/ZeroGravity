using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgEllipseProgressGoal : ContentView
    {


        public static readonly BindableProperty TargetFoodProperty =
           BindableProperty.Create(nameof(TargetFood), typeof(string), typeof(ZgEllipseProgressGoal), String.Empty, BindingMode.TwoWay, OnTargetFoodChanged);

        private static bool OnTargetFoodChanged(BindableObject bindable, object value)
        {
            var item = bindable as ZgEllipseProgressGoal;

            item.Imgfood1.Source = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Progress_BreakFast.png");
            item.Imgfood2.Source = ImageSource.FromResource("ZeroGravity.Mobile.Resources.Images.Progress_BreakFast.png");

            return true;
        }

       

        public static readonly BindableProperty ActualFoodProperty =
           BindableProperty.Create(nameof(ActualFood), typeof(string), typeof(ZgEllipseProgressGoal), String.Empty, BindingMode.TwoWay, OnActualFoodChanged);

        private static bool OnActualFoodChanged(BindableObject bindable, object value)
        {
            return true;
        }

       

        public static readonly BindableProperty TitleLabelProperty =
            BindableProperty.Create(nameof(TitleLabel), typeof(string), typeof(ZgEllipseProgressGoal));
        public static readonly BindableProperty TitleIconProperty =
         BindableProperty.Create(nameof(TitleIcon), typeof(string), typeof(ZgEllipseProgressGoal));

        public string TitleLabel
        {
            get => (string)GetValue(TitleLabelProperty);
            set => SetValue(TitleLabelProperty, value);
        }

        public string TitleIcon
        {
            get => (string)GetValue(TitleIconProperty);
            set => SetValue(TitleIconProperty, value);
        }

        public string ActualFood
        {
            get => (string)GetValue(ActualFoodProperty);
            set => SetValue(ActualFoodProperty, value);
        }

        public string TargetFood
        {
            get => (string)GetValue(TargetFoodProperty);
            set => SetValue(TargetFoodProperty, value);
        }

        public ZgEllipseProgressGoal()
        {
            InitializeComponent();
        }
    }
}