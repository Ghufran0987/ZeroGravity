using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgImageView : ContentView
    {
        public static readonly BindableProperty TitleLabelProperty =
            BindableProperty.Create(nameof(TitleLabel), typeof(string), typeof(ZgImageView));

        public static readonly BindableProperty RecommendationLabelProperty =
            BindableProperty.Create(nameof(RecommendationLabel), typeof(string), typeof(ZgImageView));

        public static readonly BindableProperty TargetImageSourceProperty =
            BindableProperty.Create(nameof(TargetImageSource), typeof(ImageSource), typeof(ZgImageView));

        public static readonly BindableProperty TargetValueLabelProperty =
            BindableProperty.Create(nameof(TargetValueLabel), typeof(string), typeof(ZgImageView));

        public static readonly BindableProperty ActualValueLabelProperty =
            BindableProperty.Create(nameof(ActualValueLabel), typeof(string), typeof(ZgImageView));

        public static readonly BindableProperty ValueLabelUnitProperty =
            BindableProperty.Create(nameof(ValueLabelUnit), typeof(string), typeof(ZgImageView));

        public static readonly BindableProperty FeedbackStateValueProperty =
            BindableProperty.Create(nameof(FeedbackStateValue), typeof(FeedbackState), typeof(ZgImageView));

        public static readonly BindableProperty BlurRadiusProperty =
            BindableProperty.Create(nameof(BlurRadius), typeof(float), typeof(ZgImageView));

        public static readonly BindableProperty FeedbackColorProperty =
            BindableProperty.Create(nameof(FeedbackColor), typeof(Color), typeof(ZgImageView));

        public static readonly BindableProperty TitleIconProperty =
          BindableProperty.Create(nameof(TitleIcon), typeof(string), typeof(ZgImageView));

        public static readonly BindableProperty ShowProgressProperty =
     BindableProperty.Create(nameof(ShowProgress), typeof(bool), typeof(ZgImageView));

        public static readonly BindableProperty ShowAdviceTextProperty =
BindableProperty.Create(nameof(ShowAdviceText), typeof(bool), typeof(ZgImageView));

        public ZgImageView()
        {
            InitializeComponent();
        }

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

        public string RecommendationLabel
        {
            get => (string)GetValue(RecommendationLabelProperty);
            set => SetValue(RecommendationLabelProperty, value);
        }

        public ImageSource TargetImageSource
        {
            get => (ImageSource)GetValue(TargetImageSourceProperty);
            set => SetValue(TargetImageSourceProperty, value);
        }

        public string TargetValueLabel
        {
            get => (string)GetValue(TargetValueLabelProperty);
            set => SetValue(TargetValueLabelProperty, value);
        }

        public string ActualValueLabel
        {
            get => (string)GetValue(ActualValueLabelProperty);
            set => SetValue(ActualValueLabelProperty, value);
        }

        public string ValueLabelUnit
        {
            get => (string)GetValue(ValueLabelUnitProperty);
            set => SetValue(ValueLabelUnitProperty, value);
        }

        public FeedbackState FeedbackStateValue
        {
            get => (FeedbackState)GetValue(FeedbackStateValueProperty);
            set => SetValue(FeedbackStateValueProperty, value);
        }

        public float BlurRadius
        {
            get => (float)GetValue(BlurRadiusProperty);
            set => SetValue(BlurRadiusProperty, value);
        }

        public Color FeedbackColor
        {
            get => (Color)GetValue(FeedbackColorProperty);
            set => SetValue(FeedbackColorProperty, value);
        }

        public bool ShowProgress
        {
            get => (bool)GetValue(ShowProgressProperty);
            set => SetValue(ShowProgressProperty, value);
        }

        public bool ShowAdviceText
        {
            get => (bool)GetValue(ShowAdviceTextProperty);
            set => SetValue(ShowAdviceTextProperty, value);
        }


        
        //private string GetTitleIcon(string titleLabel)
        //{
        //    if (!string.IsNullOrEmpty(titleLabel))
        //    {
        //        switch (titleLabel.ToLower())
        //        {
        //            case "water": return "&#xf748;";
        //            default:
        //                break;
        //        }
        //    }
        //    return string.Empty;
        //}

    }
}