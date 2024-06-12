using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgButton : ContentView
    {
        public ZgButton()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ZgButton));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty); 
            set => SetValue(CommandProperty, value);
        }

        public static  readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ZgButton));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty); 
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ZgButton));

        public string Text
        {
            get => (string)GetValue(TextProperty); 
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ZgButton));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty); 
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ZgButton));

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty); 
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ZgButton));

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty); 
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty IconUnicodeProperty = BindableProperty.Create(nameof(IconUnicode), typeof(string), typeof(ZgButton));

        public string IconUnicode
        {
            get => (string)GetValue(IconUnicodeProperty); 
            set => SetValue(IconUnicodeProperty, value);
        }

        public static readonly BindableProperty IconTextColorProperty = BindableProperty.Create(nameof(IconTextColor), typeof(Color), typeof(ZgButton));

        public Color IconTextColor
        {
            get => (Color)GetValue(IconTextColorProperty); 
            set => SetValue(IconTextColorProperty, value);
        }

        public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(nameof(IconFontFamily), typeof(string), typeof(ZgButton));

        public string IconFontFamily
        {
            get => (string)GetValue(IconFontFamilyProperty); 
            set => SetValue(IconFontFamilyProperty, value);
        }

        public static readonly BindableProperty IconFontSizeProperty = BindableProperty.Create(nameof(IconFontSize), typeof(double), typeof(ZgButton));

        public double IconFontSize
        {
            get => (double)GetValue(IconFontSizeProperty); 
            set => SetValue(IconFontSizeProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(CornerRadius), typeof(ZgButton));

        public CornerRadius CornerRadius
        {
            get => (CornerRadius)GetValue(CornerRadiusProperty); 
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty GapWidthProperty = BindableProperty.Create(nameof(GapWidth), typeof(double), typeof(ZgButton));

        public double GapWidth
        {
            get => (double)GetValue(GapWidthProperty); 
            set => SetValue(GapWidthProperty, value);
        }

        public static readonly BindableProperty BlurRadiusProperty = BindableProperty.Create(nameof(BlurRadius), typeof(float), typeof(ZgButton));

        public float BlurRadius
        {
            get => (float)GetValue(BlurRadiusProperty); 
            set => SetValue(BlurRadiusProperty, value);
        }

        public static readonly BindableProperty ButtonMarginProperty = BindableProperty.Create(nameof(ButtonMargin), typeof(Thickness), typeof(ZgButton));

        public Thickness ButtonMargin
        {
            get => (Thickness)GetValue(ButtonMarginProperty); 
            set => SetValue(ButtonMarginProperty, value);
        }

        public static readonly BindableProperty ButtonColorProperty = BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(ZgButton));

        public Color ButtonColor
        {
            get => (Color)GetValue(ButtonColorProperty); 
            set => SetValue(ButtonColorProperty, value);
        }

        public static readonly BindableProperty HorizontalTextAlignmentProperty = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(ZgButton));

        public TextAlignment HorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(HorizontalTextAlignmentProperty); 
            set => SetValue(HorizontalTextAlignmentProperty, value);
        }
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(FontAttributes), typeof(FontAttributes), typeof(ZgButton));

        public FontAttributes FontAttributes
        {
            get => (FontAttributes)GetValue(FontAttributesProperty);
            set => SetValue(FontAttributesProperty, value);
        }
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ZgButton));

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(float), typeof(ZgButton));

        public float BorderWidth
        {
            get => (float)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }        
    }
}