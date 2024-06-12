using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgParagraph : ContentView
    {
        public ZgParagraph()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ZgParagraph));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty LineHeightProperty = BindableProperty.Create(nameof(LineHeight), typeof(double), typeof(ZgParagraph));

        public double LineHeight
        {
            get => (double)GetValue(LineHeightProperty); 
            set => SetValue(LineHeightProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ZgParagraph));

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty); 
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ZgParagraph));

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty); 
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ZgParagraph));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty); 
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty TextDecorationProperty = BindableProperty.Create(nameof(TextDecoration), typeof(TextDecorations), typeof(ZgParagraph));

        public TextDecorations TextDecoration
        {
            get => (TextDecorations)GetValue(TextDecorationProperty);
            set => SetValue(TextDecorationProperty, value);
        }

        public static readonly BindableProperty FontAttributeProperty = BindableProperty.Create(nameof(FontAttribute), typeof(FontAttributes), typeof(ZgParagraph));

        public FontAttributes FontAttribute
        {
            get => (FontAttributes)GetValue(FontAttributeProperty);
            set => SetValue(FontAttributeProperty, value);
        }

        public static readonly BindableProperty TextAlignmentProperty = BindableProperty.Create(nameof(HorizontalTextAlignment), typeof(TextAlignment), typeof(ZgParagraph));

        public TextAlignment HorizontalTextAlignment
        {
            get => (TextAlignment)GetValue(TextAlignmentProperty);
            set => SetValue(TextAlignmentProperty, value);
        }
    }
}