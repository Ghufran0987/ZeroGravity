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
    public partial class ZgMessageBox : ContentView
    {
        public ZgMessageBox()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(ZgMessageBox));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty LineHeightProperty = BindableProperty.Create(nameof(LineHeight), typeof(double), typeof(ZgMessageBox));

        public double LineHeight
        {
            get => (double)GetValue(LineHeightProperty);
            set => SetValue(LineHeightProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ZgMessageBox));

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ZgMessageBox));

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ZgMessageBox));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty TextDecorationProperty = BindableProperty.Create(nameof(TextDecoration), typeof(TextDecorations), typeof(ZgMessageBox));

        public TextDecorations TextDecoration
        {
            get => (TextDecorations)GetValue(TextDecorationProperty);
            set => SetValue(TextDecorationProperty, value);
        }

        public static readonly BindableProperty FontAttributeProperty = BindableProperty.Create(nameof(FontAttribute), typeof(FontAttributes), typeof(ZgMessageBox));

        public FontAttributes FontAttribute
        {
            get => (FontAttributes)GetValue(FontAttributeProperty);
            set => SetValue(FontAttributeProperty, value);
        }

        public static readonly BindableProperty ShowIconProperty = BindableProperty.Create(nameof(ShowIcon), typeof(bool), typeof(ZgMessageBox));

        public bool ShowIcon
        {
            get => (bool) GetValue(ShowIconProperty); 
            set => SetValue(ShowIconProperty, value);
        }

        public static readonly BindableProperty GapWidthProperty = BindableProperty.Create(nameof(GapWidth), typeof(double), typeof(ZgMessageBox));

        public double GapWidth
        {
            get => (double) GetValue(GapWidthProperty); 
            set => SetValue(GapWidthProperty, value);
        }

        public static readonly BindableProperty IconTextProperty = BindableProperty.Create(nameof(IconText), typeof(string), typeof(ZgMessageBox));

        public string IconText
        {
            get => (string) GetValue(IconTextProperty); 
            set => SetValue(IconTextProperty, value);
        }

        public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(nameof(IconFontFamily), typeof(string), typeof(ZgMessageBox));

        public string IconFontFamily
        {
            get => (string) GetValue(IconFontFamilyProperty); 
            set => SetValue(IconFontFamilyProperty, value);
        }

        public static readonly BindableProperty IconFontSizeProperty = BindableProperty.Create(nameof(IconFontSize), typeof(double), typeof(ZgMessageBox));

        public double IconFontSize
        {
            get => (double) GetValue(IconFontSizeProperty); 
            set => SetValue(IconFontSizeProperty, value);
        }

        public static readonly BindableProperty IconTextColorProperty = BindableProperty.Create(nameof(IconTextColor), typeof(Color), typeof(ZgMessageBox));

        public Color IconTextColor
        {
            get => (Color) GetValue(IconTextColorProperty); 
            set => SetValue(IconTextColorProperty, value);
        }
    }
}