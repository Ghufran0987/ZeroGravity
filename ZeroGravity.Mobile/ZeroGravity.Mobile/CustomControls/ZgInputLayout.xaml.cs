using System;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PropertyChangingEventArgs = Xamarin.Forms.PropertyChangingEventArgs;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgInputLayout : ContentView
    {
        private readonly ContentPresenter _presenter;

        public ZgInputLayout()
        {
            InitializeComponent();

            var child = GetTemplateChild("Presenter");
            _presenter = (ContentPresenter)child;
            _presenter.PropertyChanging += OnPresenterPropertyChanging;
            _presenter.PropertyChanged += OnPresenterPropertyChanged;
            IsLabelVisible = true;
        }

        public static readonly BindableProperty LabelProperty = BindableProperty.Create(nameof(Label), typeof(string), typeof(ZgInputLayout));

        public string Label
        {
            get => (string)GetValue(LabelProperty);
            set => SetValue(LabelProperty, value);
        }

        public static readonly BindableProperty IsLabelVisibleProperty = BindableProperty.Create(nameof(IsLabelVisible), typeof(bool), typeof(ZgInputLayout));

        public bool IsLabelVisible
        {
            get => (bool)GetValue(IsLabelVisibleProperty);
            set => SetValue(IsLabelVisibleProperty, value);
        }

        public static readonly BindableProperty TextColorProperty = BindableProperty.Create(nameof(TextColor), typeof(Color), typeof(ZgInputLayout));

        public Color TextColor
        {
            get => (Color)GetValue(TextColorProperty);
            set => SetValue(TextColorProperty, value);
        }

        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(ZgInputLayout));

        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderThicknessProperty = BindableProperty.Create(nameof(BorderThickness), typeof(Thickness), typeof(ZgInputLayout));

        public Thickness BorderThickness
        {
            get => (Thickness)GetValue(BorderThicknessProperty);
            set => SetValue(BorderThicknessProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(double), typeof(ZgInputLayout));

        public double BorderWidth
        {
            get => (double)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }

        public static readonly BindableProperty CornerRadiusProperty =
            BindableProperty.Create(nameof(CornerRadius), typeof(Thickness), typeof(ZgInputLayout));

        public Thickness CornerRadius
        {
            get => (Thickness)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }

        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ZgInputLayout));

        public string FontFamily
        {
            get => (string)GetValue(FontFamilyProperty);
            set => SetValue(FontFamilyProperty, value);
        }

        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ZgInputLayout));

        public double FontSize
        {
            get => (double)GetValue(FontSizeProperty);
            set => SetValue(FontSizeProperty, value);
        }

        public static readonly BindableProperty LabelMarginProperty = BindableProperty.Create(nameof(LabelMargin), typeof(Thickness), typeof(ZgInputLayout));

        public Thickness LabelMargin
        {
            get => (Thickness)GetValue(LabelMarginProperty);
            set => SetValue(LabelMarginProperty, value);
        }

        public static readonly BindableProperty ContentMarginProperty = BindableProperty.Create(nameof(ContentMargin), typeof(Thickness), typeof(ZgInputLayout));

        public Thickness ContentMargin
        {
            get => (Thickness)GetValue(ContentMarginProperty);
            set => SetValue(ContentMarginProperty, value);
        }

        public static readonly BindableProperty GapHeightProperty = BindableProperty.Create(nameof(GapHeight), typeof(double), typeof(ZgInputLayout));

        public double GapHeight
        {
            get => (double)GetValue(GapHeightProperty);
            set => SetValue(GapHeightProperty, value);
        }

        public static readonly BindableProperty HighlightColorProperty = BindableProperty.Create(nameof(HighlightColor), typeof(Color), typeof(ZgInputLayout));

        public Color HighlightColor
        {
            get => (Color)GetValue(HighlightColorProperty);
            set => SetValue(HighlightColorProperty, value);
        }

        public static readonly BindableProperty IsHighlightedProperty = BindableProperty.Create(nameof(IsHighlighted), typeof(bool), typeof(ZgInputLayout), default(bool), BindingMode.TwoWay);

        public bool IsHighlighted
        {
            get => (bool)GetValue(IsHighlightedProperty);
            set => SetValue(IsHighlightedProperty, value);
        }

        public bool UseHighlighting { get; set; }

        public bool Test { get; set; }

        private void OnTapped(object sender, EventArgs args)
        {
            _presenter.Content.Focus();
        }

        private void OnPresenterPropertyChanging(object sender, PropertyChangingEventArgs args)
        {
            if (args.PropertyName == nameof(ContentPresenter.Content))
            {
                if (_presenter.Content != null)
                {
                    _presenter.Content.Focused -= OnPresenterContentFocused;
                    _presenter.Content.Unfocused -= OnPresenterContentUnfocused;
                }
            }
        }

        private void OnPresenterPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            if (args.PropertyName == nameof(ContentPresenter.Content))
            {
                if (_presenter.Content != null)
                {
                    _presenter.Content.Focused += OnPresenterContentFocused;
                    _presenter.Content.Unfocused += OnPresenterContentUnfocused;
                }
            }
        }

        private void OnPresenterContentFocused(object sender, FocusEventArgs args)
        {
            if (!UseHighlighting) return;

            IsHighlighted = true;
        }

        private void OnPresenterContentUnfocused(object sender, FocusEventArgs e)
        {
            if (!UseHighlighting) return;

            IsHighlighted = false;
        }
    }
}