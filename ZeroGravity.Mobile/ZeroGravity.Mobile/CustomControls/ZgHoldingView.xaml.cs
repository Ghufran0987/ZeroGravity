using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgHoldingView : ContentView
    {
        public ZgHoldingView()
        {
            InitializeComponent();
        }

        #region Icon

        public static readonly BindableProperty IconTextProperty = BindableProperty.Create(nameof(IconText), typeof(string), typeof(ZgHoldingView));

        public string IconText
        {
            get => (string) GetValue(IconTextProperty); 
            set => SetValue(IconTextProperty, value);
        }

        public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(nameof(IconFontFamily), typeof(string), typeof(ZgHoldingView));

        public string IconFontFamily
        {
            get => (string) GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }

        public static readonly BindableProperty IconFontSizeProperty = BindableProperty.Create(nameof(IconFontSize), typeof(double), typeof(ZgHoldingView));

        public double IconFontSize
        {
            get => (double) GetValue(IconFontSizeProperty);
            set => SetValue(IconFontSizeProperty, value);
        }

        public static readonly BindableProperty IconLineHeightProperty = BindableProperty.Create(nameof(IconLineHeight), typeof(double), typeof(ZgHoldingView));

        public double IconLineHeight
        {
            get => (double) GetValue(IconLineHeightProperty); 
            set => SetValue(IconLineHeightProperty, value);
        }

        public static readonly BindableProperty IconTextColorProperty = BindableProperty.Create(nameof(IconTextColor), typeof(Color), typeof(ZgHoldingView));

        public Color IconTextColor
        {
            get => (Color) GetValue(IconTextColorProperty); 
            set => SetValue(IconTextColorProperty, value);
        }

        public static readonly BindableProperty IconBackgroundColorProperty = BindableProperty.Create(nameof(IconBackgroundColor), typeof(Color), typeof(ZgHoldingView));

        public Color IconBackgroundColor
        {
            get => (Color) GetValue(IconBackgroundColorProperty); 
            set => SetValue(IconBackgroundColorProperty, value);
        }

        #endregion

        #region Title

        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(Title), typeof(string), typeof(ZgHoldingView));

        public string Title
        {
            get => (string) GetValue(TitleProperty); 
            set => SetValue(TitleProperty, value);
        }

        public static readonly BindableProperty TitleFontFamilyProperty = BindableProperty.Create(nameof(TitleFontFamily), typeof(string), typeof(ZgHoldingView));

        public string TitleFontFamily
        {
            get => (string) GetValue(TitleFontFamilyProperty); 
            set => SetValue(TitleFontFamilyProperty, value);
        }

        public static readonly BindableProperty TitleFontSizeProperty = BindableProperty.Create(nameof(TitleFontSize), typeof(double), typeof(ZgHoldingView));

        public double TitleFontSize
        {
            get => (double) GetValue(TitleFontSizeProperty); 
            set => SetValue(TitleFontFamilyProperty, value);
        }

        public static readonly BindableProperty TitleLineHeightProperty = BindableProperty.Create(nameof(TitleLineHeight), typeof(double), typeof(ZgHoldingView));

        public double TitleLineHeight
        {
            get => (double) GetValue(TitleLineHeightProperty);
            set => SetValue(TitleLineHeightProperty, value);
        }

        public static readonly BindableProperty TitleTextColorProperty = BindableProperty.Create(nameof(TitleTextColor), typeof(Color), typeof(ZgHoldingView));

        public Color TitleTextColor
        {
            get => (Color) GetValue(TitleTextColorProperty);
            set => SetValue(TitleTextColorProperty, value);
        }

        #endregion

        #region Description

        public static readonly BindableProperty DescriptionTextProperty = BindableProperty.Create(nameof(DescriptionText), typeof(string), typeof(ZgHoldingView));

        public string DescriptionText
        {
            get => (string) GetValue(DescriptionTextProperty);
            set => SetValue(DescriptionTextProperty, value);
        }

        public static readonly BindableProperty DescriptionFontFamilyProperty = BindableProperty.Create(nameof(DescriptionFontFamily), typeof(string), typeof(ZgHoldingView));

        public string DescriptionFontFamily
        {
            get => (string) GetValue(DescriptionFontFamilyProperty); 
            set => SetValue(DescriptionFontFamilyProperty, value);
        }

        public static readonly BindableProperty DescriptionFontSizeProperty = BindableProperty.Create(nameof(DescriptionFontSize), typeof(double), typeof(ZgHoldingView));

        public double DescriptionFontSize
        {
            get => (double) GetValue(DescriptionFontSizeProperty); 
            set => SetValue(DescriptionFontSizeProperty, value);
        }

        public static readonly BindableProperty DescriptionLineHeightProperty = BindableProperty.Create(nameof(DescriptionLineHeight), typeof(double), typeof(ZgHoldingView));

        public double DescriptionLineHeight
        {
            get => (double) GetValue(DescriptionLineHeightProperty); 
            set => SetValue(DescriptionLineHeightProperty, value);
        }

        public static readonly BindableProperty DescriptionTextColorProperty = BindableProperty.Create(nameof(DescriptionTextColor), typeof(Color), typeof(ZgHoldingView));

        public Color DescriptionTextColor
        {
            get => (Color) GetValue(DescriptionTextColorProperty); 
            set => SetValue(DescriptionTextColorProperty, value);
        }

        #endregion

        #region Button

        public static readonly BindableProperty ButtonTextProperty = BindableProperty.Create(nameof(ButtonText), typeof(string), typeof(ZgHoldingView));

        public string ButtonText
        {
            get => (string) GetValue(ButtonTextProperty); 
            set => SetValue(ButtonTextProperty, value);
        }

        public static readonly BindableProperty ButtonFontFamilyProperty = BindableProperty.Create(nameof(ButtonFontFamily), typeof(string), typeof(ZgHoldingView));

        public string ButtonFontFamily
        {
            get => (string) GetValue(ButtonFontFamilyProperty);
            set => SetValue(ButtonFontFamilyProperty, value);
        }

        public static readonly BindableProperty ButtonFontSizeProperty = BindableProperty.Create(nameof(ButtonFontSize), typeof(double), typeof(ZgHoldingView));

        public double ButtonFontSize
        {
            get => (double) GetValue(ButtonFontSizeProperty); 
            set => SetValue(ButtonFontSizeProperty, value);
        }

        public static readonly BindableProperty ButtonLineHeightProperty = BindableProperty.Create(nameof(ButtonLineHeight), typeof(double), typeof(ZgHoldingView));

        public double ButtonLineHeight
        {
            get => (double) GetValue(ButtonLineHeightProperty);
            set => SetValue(ButtonLineHeightProperty, value);
        }

        public static readonly BindableProperty ButtonTextColorProperty = BindableProperty.Create(nameof(ButtonTextColor), typeof(Color), typeof(ZgHoldingView));

        public Color ButtonTextColor
        {
            get => (Color) GetValue(ButtonTextColorProperty);
            set => SetValue(ButtonTextColorProperty, value);
        }

        public static readonly BindableProperty ButtonBackgroundColorProperty = BindableProperty.Create(nameof(ButtonBackgroundColor), typeof(Color), typeof(ZgHoldingView));

        public Color ButtonBackgroundColor
        {
            get => (Color) GetValue(ButtonBackgroundColorProperty); 
            set => SetValue(ButtonBackgroundColorProperty, value);
        }

        public static readonly BindableProperty ButtonCommandProperty = BindableProperty.Create(nameof(ButtonCommand), typeof(ICommand), typeof(ZgHoldingView));

        public ICommand ButtonCommand
        {
            get => (ICommand) GetValue(ButtonCommandProperty); 
            set => SetValue(ButtonCommandProperty, value);
        }

        public static readonly BindableProperty ButtonCommandParameterProperty = BindableProperty.Create(nameof(ButtonCommandParameter), typeof(object), typeof(ZgHoldingView));

        public object ButtonCommandParameter
        {
            get => (object) GetValue(ButtonCommandParameterProperty);
            set => SetValue(ButtonCommandParameterProperty, value);
        }

        public static readonly BindableProperty ButtonMarginProperty = BindableProperty.Create(nameof(ButtonMargin), typeof(Thickness), typeof(ZgHoldingView));

        public Thickness ButtonMargin
        {
            get => (Thickness) GetValue(ButtonMarginProperty); 
            set => SetValue(ButtonMarginProperty, value);
        }

        public static readonly BindableProperty ButtonCornerRadiusProperty = BindableProperty.Create(nameof(ButtonCornerRadius), typeof(CornerRadius), typeof(ZgHoldingView));

        public CornerRadius ButtonCornerRadius
        {
            get => (CornerRadius) GetValue(ButtonCornerRadiusProperty);
            set => SetValue(ButtonCornerRadiusProperty, value);
        }

        public static readonly BindableProperty ButtonBlurRadiusProperty = BindableProperty.Create(nameof(ButtonBlurRadius), typeof(float), typeof(ZgHoldingView));

        public float ButtonBlurRadius
        {
            get => (float) GetValue(ButtonBlurRadiusProperty); 
            set => SetValue(ButtonBlurRadiusProperty, value);
        }

        #endregion

        #region TapableLabel

        public static readonly BindableProperty TapableLabelTextProperty = BindableProperty.Create(nameof(TapableLabelText), typeof(string), typeof(ZgHoldingView));

        public string TapableLabelText
        {
            get => (string) GetValue(TapableLabelTextProperty);
            set => SetValue(TapableLabelTextProperty, value);
        }

        public static readonly BindableProperty TapableLabelFontFamilyProperty = BindableProperty.Create(nameof(TapableLabelFontFamily), typeof(string), typeof(ZgHoldingView));

        public string TapableLabelFontFamily
        {
            get => (string) GetValue(TapableLabelFontFamilyProperty); 
            set => SetValue(TapableLabelFontFamilyProperty, value);
        }

        public static readonly BindableProperty TapableLabelFontSizeProperty = BindableProperty.Create(nameof(TapableLabelFontSize), typeof(double), typeof(ZgHoldingView));

        public double TapableLabelFontSize
        {
            get => (double) GetValue(TapableLabelFontSizeProperty); 
            set => SetValue(TapableLabelFontSizeProperty, value);
        }

        public static readonly BindableProperty TapableLabelLineHeightProperty = BindableProperty.Create(nameof(TapableLabelLineHeight), typeof(double), typeof(ZgHoldingView));

        public double TapableLabelLineHeight
        {
            get => (double) GetValue(TapableLabelLineHeightProperty); 
            set => SetValue(TapableLabelLineHeightProperty, value);
        }

        public static readonly BindableProperty TapableLabelTextColorProperty = BindableProperty.Create(nameof(TapableLabelTextColor), typeof(Color), typeof(ZgHoldingView));

        public Color TapableLabelTextColor
        {
            get => (Color) GetValue(TapableLabelTextColorProperty);
            set => SetValue(TapableLabelTextColorProperty, value);
        }

        public static readonly BindableProperty TapableLabelTextDecorationProperty = BindableProperty.Create(nameof(TapableLabelTextDecoration), typeof(TextDecorations), typeof(ZgHoldingView));

        public TextDecorations TapableLabelTextDecoration
        {
            get => (TextDecorations) GetValue(TapableLabelTextDecorationProperty); 
            set => SetValue(TapableLabelTextDecorationProperty, value);
        }

        public static readonly BindableProperty TapableLabelCommandProperty = BindableProperty.Create(nameof(TapableLabelCommand), typeof(ICommand), typeof(ZgHoldingView));

        public ICommand TapableLabelCommand
        {
            get => (ICommand) GetValue(TapableLabelCommandProperty);
            set => SetValue(TapableLabelCommandProperty, value);
        }

        public static readonly BindableProperty TapableLabelCommandParameterProperty = BindableProperty.Create(nameof(TapableLabelCommandParameter), typeof(object), typeof(ZgHoldingView));

        public object TapableLabelCommandParameter
        {
            get => (object) GetValue(TapableLabelCommandParameterProperty);
            set => SetValue(TapableLabelCommandParameterProperty, value);
        }

        #endregion
    }
}