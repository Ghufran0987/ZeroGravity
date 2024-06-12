using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgNavBarButton : ContentView
    {
        public ZgNavBarButton()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ZgNavBarButton));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ZgNavBarButton));

        public object CommandParameter
        {
            get => GetValue(CommandParameterProperty);
            set => SetValue(CommandParameterProperty, value);
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ZgNavBarButton));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ZgNavBarButton));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ZgNavBarButton));

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ZgNavBarButton));

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty LineHeightProperty = BindableProperty.Create(nameof(LineHeight), typeof(double), typeof(ZgNavBarButton));

        public double LineHeight
        {
            get => (double)GetValue(LineHeightProperty);
            set => SetValue(LineHeightProperty, value);
        }

        public static readonly BindableProperty IconUnicodeProperty = BindableProperty.Create(nameof(IconText), typeof(string), typeof(ZgNavBarButton));

        public string IconText
        {
            get => (string)GetValue(IconUnicodeProperty);
            set => SetValue(IconUnicodeProperty, value);
        }

        public static readonly BindableProperty IconTextColorProperty = BindableProperty.Create(nameof(IconTextColor), typeof(Color), typeof(ZgNavBarButton));

        public Color IconTextColor
        {
            get => (Color)GetValue(IconTextColorProperty);
            set => SetValue(IconTextColorProperty, value);
        }

        public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(nameof(IconFontFamily), typeof(string), typeof(ZgNavBarButton));

        public string IconFontFamily
        {
            get => (string)GetValue(IconFontFamilyProperty);
            set => SetValue(IconFontFamilyProperty, value);
        }

        public static readonly BindableProperty IconFontSizeProperty = BindableProperty.Create(nameof(IconFontSize), typeof(double), typeof(ZgNavBarButton));

        public double IconFontSize
        {
            get => (double)GetValue(IconFontSizeProperty);
            set => SetValue(IconFontSizeProperty, value);
        }
        public static readonly BindableProperty GapWidthProperty = BindableProperty.Create(nameof(GapWidth), typeof(double), typeof(ZgNavBarButton));

        public double GapWidth
        {
            get => (double)GetValue(GapWidthProperty);
            set => SetValue(GapWidthProperty, value);
        }

        public static readonly BindableProperty ButtonMarginProperty = BindableProperty.Create(nameof(ButtonMargin), typeof(Thickness), typeof(ZgNavBarButton));

        public Thickness ButtonMargin
        {
            get => (Thickness)GetValue(ButtonMarginProperty);
            set => SetValue(ButtonMarginProperty, value);
        }

        public static readonly BindableProperty ButtonColorProperty = BindableProperty.Create(nameof(ButtonColor), typeof(Color), typeof(ZgNavBarButton));

        public Color ButtonColor
        {
            get => (Color)GetValue(ButtonColorProperty);
            set => SetValue(ButtonColorProperty, value);
        }
    }
}