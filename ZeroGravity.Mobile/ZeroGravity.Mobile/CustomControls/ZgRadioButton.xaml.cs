using System;
using System.Windows.Input;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgRadioButton : ContentView
    {
        private readonly Label _descriptionLabel;

        public ZgRadioButton()
        {
            InitializeComponent();

            var child = GetTemplateChild("DescriptionLabel");
            _descriptionLabel = (Label)child;
        }

        public static readonly BindableProperty GroupKeyProperty = BindableProperty.Create(nameof(GroupKey), typeof(SfRadioGroupKey), typeof(ZgRadioButton));

        public SfRadioGroupKey GroupKey
        {
            get => (SfRadioGroupKey)GetValue(GroupKeyProperty); 
            set => SetValue(GroupKeyProperty, value);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ZgRadioButton));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty); 
            set => SetValue(CommandProperty, value);
        }

        #region Label

        public static readonly BindableProperty LabelContentProperty = BindableProperty.Create(nameof(LabelContent), typeof(View), typeof(ZgRadioButton));

        public View LabelContent
        {
            get => (View)GetValue(LabelContentProperty);
            set => SetValue(LabelContentProperty, value);
        }

        public static readonly BindableProperty LabelTextProperty = BindableProperty.Create(nameof(LabelText), typeof(string), typeof(ZgRadioButton));

        public string LabelText
        {
            get => (string)GetValue(LabelTextProperty); 
            set => SetValue(LabelTextProperty, value);
        }

        public static readonly BindableProperty LabelFontFamilyProperty = BindableProperty.Create(nameof(LabelFontFamily), typeof(string), typeof(ZgRadioButton));

        public string LabelFontFamily
        {
            get => (string)GetValue(LabelFontFamilyProperty); 
            set => SetValue(LabelFontFamilyProperty, value);
        }

        public static readonly BindableProperty LabelFontSizeProperty = BindableProperty.Create(nameof(LabelFontSize), typeof(double), typeof(ZgRadioButton));

        public double LabelFontSize
        {
            get => (double)GetValue(LabelFontSizeProperty); 
            set => SetValue(LabelFontSizeProperty, value);
        }

        public static readonly BindableProperty LabelTextColorProperty = BindableProperty.Create(nameof(LabelTextColor), typeof(Color), typeof(ZgRadioButton));

        public Color LabelTextColor
        {
            get => (Color)GetValue(LabelTextColorProperty); 
            set => SetValue(LabelTextColorProperty, value);
        }

        #endregion

        #region Description

        public static readonly BindableProperty DescriptionTextProperty = BindableProperty.Create(nameof(DescriptionText), typeof(string), typeof(ZgRadioButton));

        public string DescriptionText
        {
            get => (string)GetValue(DescriptionTextProperty); 
            set => SetValue(DescriptionTextProperty, value);
        }

        public static readonly BindableProperty DescriptionFontFamilyProperty = BindableProperty.Create(nameof(DescriptionFontFamily), typeof(string), typeof(ZgRadioButton));

        public string DescriptionFontFamily
        {
            get => (string)GetValue(DescriptionFontFamilyProperty); 
            set => SetValue(DescriptionFontFamilyProperty, value);
        }

        public static readonly BindableProperty DescriptionFontSizeProperty = BindableProperty.Create(nameof(DescriptionFontSize), typeof(double), typeof(ZgRadioButton));

        public double DescriptionFontSize
        {
            get => (double)GetValue(DescriptionFontSizeProperty); 
            set => SetValue(DescriptionFontSizeProperty, value);
        }

        public static readonly BindableProperty DescriptionTextColorProperty = BindableProperty.Create(nameof(DescriptionTextColor), typeof(Color), typeof(ZgRadioButton));

        public Color DescriptionTextColor
        {
            get => (Color)GetValue(DescriptionTextColorProperty); 
            set => SetValue(DescriptionTextColorProperty, value);
        }

        public static readonly BindableProperty DescriptionLineHeightProperty = BindableProperty.Create(nameof(DescriptionLineHeight), typeof(double), typeof(ZgRadioButton));

        public double DescriptionLineHeight
        {
            get => (double)GetValue(DescriptionLineHeightProperty); 
            set => SetValue(DescriptionLineHeightProperty, value);
        }

        #endregion

        #region Icon

        public static readonly BindableProperty IconFontFamilyProperty = BindableProperty.Create(nameof(IconFontFamily), typeof(string), typeof(ZgRadioButton));

        public string IconFontFamily
        {
            get => (string)GetValue(IconFontFamilyProperty); 
            set => SetValue(IconFontFamilyProperty, value);
        }

        public static readonly BindableProperty IconFontSizeProperty = BindableProperty.Create(nameof(IconFontSize), typeof(double), typeof(ZgRadioButton));

        public double IconFontSize
        {
            get => (double)GetValue(IconFontSizeProperty); 
            set => SetValue(IconFontSizeProperty, value);
        }

        public static readonly BindableProperty IconCheckedUnicodeProperty = BindableProperty.Create(nameof(IconCheckedUnicode), typeof(string), typeof(ZgRadioButton));

        public string IconCheckedUnicode
        {
            get => (string)GetValue(IconCheckedUnicodeProperty); 
            set => SetValue(IconCheckedUnicodeProperty, value);
        }

        public static readonly BindableProperty IconUncheckedUnicodeProperty = BindableProperty.Create(nameof(IconUncheckedUnicode), typeof(string), typeof(ZgRadioButton));

        public string IconUncheckedUnicode
        {
            get => (string)GetValue(IconUncheckedUnicodeProperty); 
            set => SetValue(IconUncheckedUnicodeProperty, value);
        }

        public static readonly BindableProperty IconCheckedColorProperty = BindableProperty.Create(nameof(IconCheckedColor), typeof(Color), typeof(ZgRadioButton));

        public Color IconCheckedColor
        {
            get => (Color)GetValue(IconCheckedColorProperty); 
            set => SetValue(IconCheckedColorProperty, value);
        }

        public static readonly BindableProperty IconUncheckedColorProperty = BindableProperty.Create(nameof(IconUncheckedColor), typeof(Color), typeof(ZgRadioButton));

        public Color IconUncheckedColor
        {
            get => (Color)GetValue(IconUncheckedColorProperty); 
            set => SetValue(IconUncheckedColorProperty, value);
        }

        public static readonly BindableProperty IconCheckedBackgroundColorProperty = BindableProperty.Create(nameof(IconCheckedBackgroundColor), typeof(Color), typeof(ZgRadioButton));

        public Color IconCheckedBackgroundColor
        {
            get => (Color)GetValue(IconCheckedBackgroundColorProperty); 
            set => SetValue(IconCheckedBackgroundColorProperty, value);
        }

        public static readonly BindableProperty IconUncheckedBackgroundColorProperty = BindableProperty.Create(nameof(IconUncheckedBackgroundColor), typeof(Color), typeof(ZgRadioButton));

        public Color IconUncheckedBackgroundColor
        {
            get => (Color)GetValue(IconUncheckedBackgroundColorProperty); 
            set => SetValue(IconUncheckedBackgroundColorProperty, value);
        }

        public static readonly BindableProperty IconCornerRadiusProperty = BindableProperty.Create(nameof(IconCornerRadius), typeof(double), typeof(ZgRadioButton));

        public double IconCornerRadius
        {
            get => (double)GetValue(IconCornerRadiusProperty); 
            set => SetValue(IconCornerRadiusProperty, value);
        }

        #endregion

        public static readonly BindableProperty GapWidthProperty = BindableProperty.Create(nameof(GapWidth), typeof(double), typeof(ZgRadioButton));

        public double GapWidth
        {
            get => (double)GetValue(GapWidthProperty); 
            set => SetValue(GapWidthProperty, value);
        }

        public static readonly BindableProperty GapHeightProperty = BindableProperty.Create(nameof(GapHeight), typeof(double), typeof(ZgRadioButton));

        public double GapHeight
        {
            get => (double)GetValue(GapHeightProperty); 
            set => SetValue(GapHeightProperty, value);
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(ZgRadioButton), default(bool), BindingMode.TwoWay);

        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty); 
            set => SetValue(IsCheckedProperty, value);
        }

        public static readonly BindableProperty ShowDescriptionProperty = BindableProperty.Create(nameof(ShowDescription), typeof(bool), typeof(ZgRadioButton));

        public bool ShowDescription
        {
            get => (bool)GetValue(ShowDescriptionProperty); 
            set => SetValue(ShowDescriptionProperty, value);
        }

        public static readonly BindableProperty HasCustomLabelContentProperty = BindableProperty.Create(nameof(HasCustomLabelContent), typeof(bool), typeof(ZgRadioButton));

        public bool HasCustomLabelContent
        {
            get => (bool)GetValue(HasCustomLabelContentProperty); 
            set => SetValue(HasCustomLabelContentProperty, value);
        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();

            if (LabelContent != null)
            {
                SetInheritedBindingContext(LabelContent, BindingContext);
            }
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(LabelContent))
            {
                HasCustomLabelContent = LabelContent != null;
                return;
            }

            if (propertyName == nameof(IsChecked))
            {
                if (Command != null)
                {
                    Command.Execute(null);
                }
            }
        }

        private void OnTapped(object sender, EventArgs args)
        {
            var isCheckbox = GroupKey == null;

            if (isCheckbox)
            {
                var inverted = !IsChecked;
                IsChecked = inverted;
            }
            else
            {
                IsChecked = true;
            }
            
        }
    }
}