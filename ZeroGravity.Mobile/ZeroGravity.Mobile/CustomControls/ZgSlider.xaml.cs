using System;
using System.Globalization;
using Syncfusion.SfGauge.XForms;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgSlider : ContentView
    {
        private Span _headerValueSpan;
        private IValueConverter _headerValueConverter;

        public ZgSlider()
        {
            InitializeComponent();
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            var headerValueSpanChild = GetTemplateChild("HeaderValueSpan");
            _headerValueSpan = (Span) headerValueSpanChild;

            SetHeaderValueBinding();
        }
        
        #region Header

        public static readonly BindableProperty ShowHeaderProperty = BindableProperty.Create(nameof(ShowHeader), typeof(bool), typeof(ZgSlider), true);

        public bool ShowHeader
        {
            get => (bool)GetValue(ShowHeaderProperty); 
            set => SetValue(ShowHeaderProperty, value);
        }

        public static readonly BindableProperty ShowHeaderValueProperty = BindableProperty.Create(nameof(ShowHeaderValue), typeof(bool), typeof(ZgSlider), true);

        public bool ShowHeaderValue
        {
            get => (bool)GetValue(ShowHeaderValueProperty); 
            set => SetValue(ShowHeaderValueProperty, value);
        }

        public static readonly BindableProperty HeaderTextProperty = BindableProperty.Create(nameof(HeaderText), typeof(string), typeof(ZgSlider));

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty); 
            set => SetValue(HeaderTextProperty, value);
        }

        public static readonly BindableProperty HeaderFontFamilyProperty = BindableProperty.Create(nameof(HeaderFontFamily), typeof(string), typeof(ZgSlider));

        public string HeaderFontFamily
        {
            get => (string)GetValue(HeaderFontFamilyProperty); 
            set => SetValue(HeaderFontFamilyProperty, value);
        }

        public static readonly BindableProperty HeaderFontSizeProperty =
            BindableProperty.Create(nameof(HeaderFontSize), typeof(double), typeof(ZgSlider));

        public double HeaderFontSize
        {
            get => (double)GetValue(HeaderFontSizeProperty); 
            set => SetValue(HeaderFontSizeProperty, value);
        }

        public static readonly BindableProperty HeaderTextColorProperty = BindableProperty.Create(nameof(HeaderTextColor), typeof(Color), typeof(ZgSlider));

        public Color HeaderTextColor
        {
            get => (Color)GetValue(HeaderTextColorProperty); 
            set => SetValue(HeaderTextColorProperty, value);
        }

        public static readonly BindableProperty HeaderValueUnitProperty = BindableProperty.Create(nameof(HeaderValueUnit), typeof(string), typeof(ZgSlider));

        public string HeaderValueUnit
        {
            get => (string)GetValue(HeaderValueUnitProperty); 
            set => SetValue(HeaderValueUnitProperty, value);
        }

        public static readonly BindableProperty HeaderValueFontFamilyProperty = BindableProperty.Create(nameof(HeaderValueFontFamily), typeof(string), typeof(ZgSlider));

        public string HeaderValueFontFamily
        {
            get => (string)GetValue(HeaderValueFontFamilyProperty); 
            set => SetValue(HeaderValueFontFamilyProperty, value);
        }

        public static readonly BindableProperty HeaderValueFontSizeProperty = BindableProperty.Create(nameof(HeaderValueFontSize), typeof(double), typeof(ZgSlider));

        public double HeaderValueFontSize
        {
            get => (double)GetValue(HeaderValueFontSizeProperty); 
            set => SetValue(HeaderValueFontFamilyProperty, value);
        }

        public static readonly BindableProperty HeaderValueTextColorProperty = BindableProperty.Create(nameof(HeaderValueTextColor), typeof(Color), typeof(ZgSlider));

        public Color HeaderValueTextColor
        {
            get => (Color)GetValue(HeaderValueTextColorProperty); 
            set => SetValue(HeaderValueTextColorProperty, value);
        }

        public static readonly BindableProperty HeaderLineHeightProperty = BindableProperty.Create(nameof(HeaderLineHeight), typeof(double), typeof(ZgSlider));

        public double HeaderLineHeight
        {
            get => (double)GetValue(HeaderLineHeightProperty); 
            set => SetValue(HeaderLineHeightProperty, value);
        }

        public IValueConverter HeaderValueConverter
        {
            get => _headerValueConverter;
            set
            {
                _headerValueConverter = value;
                SetHeaderValueBinding();
            }
        }

        #endregion

        #region Slider

        public static readonly BindableProperty ValueProperty = BindableProperty.Create(nameof(Value), typeof(double), typeof(ZgSlider), default(double), BindingMode.TwoWay);

        public double Value
        {
            get => (double)GetValue(ValueProperty); 
            set => SetValue(ValueProperty, value);
        }

        public static readonly BindableProperty MinimumProperty = BindableProperty.Create(nameof(Minimum), typeof(double), typeof(ZgSlider));

        public double Minimum
        {
            get => (double)GetValue(MinimumProperty); 
            set => SetValue(MinimumProperty, value);
        }

        public static readonly BindableProperty MaximumProperty = BindableProperty.Create(nameof(Maximum), typeof(double), typeof(ZgSlider));

        public double Maximum
        {
            get => (double)GetValue(MaximumProperty); 
            set => SetValue(MaximumProperty, value);
        }

        public static readonly BindableProperty RangeMinimumProperty = BindableProperty.Create(nameof(RangeMinimum), typeof(double), typeof(ZgSlider));

        public double RangeMinimum
        {
            get => (double)GetValue(RangeMinimumProperty); 
            set => SetValue(RangeMinimumProperty, value);
        }

        public static readonly BindableProperty RangeMaximumProperty = BindableProperty.Create(nameof(RangeMaximum), typeof(double), typeof(ZgSlider));

        public double RangeMaximum
        {
            get => (double)GetValue(RangeMaximumProperty); 
            set => SetValue(RangeMaximumProperty, value);
        }

        public static readonly BindableProperty ThumbSizeProperty = BindableProperty.Create(nameof(ThumbSize), typeof(double), typeof(ZgSlider));

        public double ThumbSize
        {
            get => (double)GetValue(ThumbSizeProperty); 
            set => SetValue(ThumbSizeProperty, value);
        }

        public static readonly BindableProperty ThumbColorProperty = BindableProperty.Create(nameof(ThumbColor), typeof(Color), typeof(ZgSlider));

        public Color ThumbColor
        {
            get => (Color)GetValue(ThumbColorProperty); 
            set => SetValue(ThumbColorProperty, value);
        }

        public static readonly BindableProperty ThumbBorderColorProperty = BindableProperty.Create(nameof(ThumbBorderColor), typeof(Color), typeof(ZgSlider));

        public Color ThumbBorderColor
        {
            get => (Color)GetValue(ThumbBorderColorProperty); 
            set => SetValue(ThumbBorderColorProperty, value);
        }

        public static readonly BindableProperty TrackThicknessProperty = BindableProperty.Create(nameof(TrackThickness), typeof(double), typeof(ZgSlider));

        public double TrackThickness
        {
            get => (double)GetValue(TrackThicknessProperty); 
            set => SetValue(TrackThicknessProperty, value);
        }

        public static readonly BindableProperty TrackColorProperty = BindableProperty.Create(nameof(TrackColor), typeof(Color), typeof(ZgSlider));

        public Color TrackColor
        {
            get => (Color)GetValue(TrackColorProperty); 
            set => SetValue(TrackColorProperty, value);
        }

        public static readonly BindableProperty TrackSelectionColorProperty = BindableProperty.Create(nameof(TrackSelectionColor), typeof(Color), typeof(ZgSlider));

        public Color TrackSelectionColor
        {
            get => (Color)GetValue(TrackSelectionColorProperty); 
            set => SetValue(TrackSelectionColorProperty, value);
        }

        public static readonly BindableProperty SliderHeightRequestProperty = BindableProperty.Create(nameof(SliderHeightRequest), typeof(double), typeof(ZgSlider));

        public double SliderHeightRequest
        {
            get => (double)GetValue(SliderHeightRequestProperty); 
            set => SetValue(SliderHeightRequestProperty, value);
        }

        public static readonly BindableProperty StepFrequencyProperty = BindableProperty.Create(nameof(StepFrequency), typeof(double), typeof(ZgSlider), 1.0);

        public double StepFrequency
        {
            get => (double)GetValue(StepFrequencyProperty); 
            set => SetValue(StepFrequencyProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(ZgSlider));

        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty); 
            set => SetValue(CornerRadiusProperty, value);
        }
        
        #endregion
        
        private void SetHeaderValueBinding()
        {
            var binding = new Binding(nameof(Value), BindingMode.OneWay, converter: HeaderValueConverter, source: this);
            _headerValueSpan.SetBinding(Span.TextProperty, binding);
        }
    }

    public class ZgSliderCornerRadiusTypeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double d)
            {
                if (d > 0)
                {
                    return CornerRadiusType.Start;
                }
            }

            return CornerRadiusType.Both;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}