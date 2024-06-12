using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgEllipseGoal : ContentView
    {

        public static readonly BindableProperty TitleLabelProperty =
            BindableProperty.Create(nameof(TitleLabel), typeof(string), typeof(ZgEllipse));

        public static readonly BindableProperty SubTitleLabelProperty =
             BindableProperty.Create(nameof(SubTitleLabel), typeof(string), typeof(ZgEllipse));

        public static readonly BindableProperty FillColorProperty =
            BindableProperty.Create(nameof(FillColor), typeof(string), typeof(ZgEllipse));

        public static readonly BindableProperty FillTextProperty =
           BindableProperty.Create(nameof(FillText), typeof(string), typeof(ZgEllipse));

        public static readonly BindableProperty TitleIconProperty =
          BindableProperty.Create(nameof(TitleIcon), typeof(string), typeof(ZgEllipse));

        public static readonly BindableProperty ShowCircularProgressProperty =
          BindableProperty.Create(nameof(ShowCircularProgress), typeof(bool), typeof(ZgEllipse));

        public static readonly BindableProperty ShowEllipseProgressProperty =
          BindableProperty.Create(nameof(ShowEllipseProgress), typeof(bool), typeof(ZgEllipse));

        public static readonly BindableProperty ShowWaterCupProgressProperty =
               BindableProperty.Create(nameof(ShowWaterCupProgress), typeof(bool), typeof(ZgEllipse));

        public static readonly BindableProperty CircularProgressCountProperty =
         BindableProperty.Create(nameof(CircularProgressCount), typeof(double), typeof(ZgEllipse));

       

        public ZgEllipseGoal()
        {
            InitializeComponent();
            ShowWaterCupProgress = false;
            ShowCalorieCupProgress = false;

        }
        public string TitleLabel
        {
            get => (string)GetValue(TitleLabelProperty);
            set => SetValue(TitleLabelProperty, value);
        }

        public string SubTitleLabel
        {
            get => (string)GetValue(SubTitleLabelProperty);
            set => SetValue(SubTitleLabelProperty, value);
        }

        public string TitleIcon
        {
            get => (string)GetValue(TitleIconProperty);
            set => SetValue(TitleIconProperty, value);
        }

        public string FillColor
        {
            get => (string)GetValue(FillColorProperty);
            set => SetValue(FillColorProperty, value);
        }

        public string FillText
        {
            get => (string)GetValue(FillTextProperty);
            set => SetValue(FillTextProperty, value);
        }

        public bool ShowCircularProgress
        {
            get => (bool)GetValue(ShowCircularProgressProperty);
            set => SetValue(ShowCircularProgressProperty, value);
        }

        public bool ShowWaterCupProgress
        {
            get => (bool)GetValue(ShowWaterCupProgressProperty);
            set => SetValue(ShowWaterCupProgressProperty, value);
        }
        public bool ShowEllipseProgress
        {
            get => (bool)GetValue(ShowEllipseProgressProperty);
            set => SetValue(ShowEllipseProgressProperty, value);
        }

        public double CircularProgressCount
        {
            get => (double)GetValue(CircularProgressCountProperty);
            set => SetValue(CircularProgressCountProperty, value);
        }

        public static readonly BindableProperty CupCountProperty =
        BindableProperty.Create(nameof(CupCount), typeof(List<int>), typeof(ZgEllipse));

        public List<int> CupCount
        {
            get => (List<int>)GetValue(CupCountProperty);
            set => SetValue(CupCountProperty, value);
        }

        public static readonly BindableProperty ShowCalorieCupProgressProperty =
              BindableProperty.Create(nameof(ShowCalorieCupProgress), typeof(bool), typeof(ZgEllipse));

        public bool ShowCalorieCupProgress
        {
            get => (bool)GetValue(ShowCalorieCupProgressProperty);
            set => SetValue(ShowCalorieCupProgressProperty, value);
        }


        
    }
}