using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;
using Prism.Mvvm;
using Syncfusion.XForms.Buttons;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CheckedChangedEventArgs = Syncfusion.XForms.Buttons.CheckedChangedEventArgs;

namespace ZeroGravity.Mobile.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ZgButtonGroup : ContentView
    {
        public ZgButtonGroup()
        {
            InitializeComponent();

            Group = new SfRadioGroupKey();
            Group.CheckedChanged += OnCheckedChanged;
        }

        public static BindableProperty GroupProperty = BindableProperty.Create(nameof(Group), typeof(SfRadioGroupKey), typeof(ZgButtonGroup));

        public SfRadioGroupKey Group
        {
            get => (SfRadioGroupKey)GetValue(GroupProperty); 
            set => SetValue(GroupProperty, value);
        }

        public static BindableProperty ItemsSourceProperty = BindableProperty.Create(nameof(ItemsSource), typeof(IEnumerable<ZgButtonGroupItem>), typeof(ZgButtonGroup));

        public IEnumerable<ZgButtonGroupItem> ItemsSource
        {
            get => (IEnumerable<ZgButtonGroupItem>) GetValue(ItemsSourceProperty); 
            set => SetValue(ItemsSourceProperty, value);
        }

        public static BindableProperty SelectedItemProperty = BindableProperty.Create(nameof(SelectedItem), typeof(ZgButtonGroupItem), typeof(ZgButtonGroup));

        public ZgButtonGroupItem SelectedItem
        {
            get => (ZgButtonGroupItem) GetValue(SelectedItemProperty); 
            set => SetValue(SelectedItemProperty, value);
        }

        public static BindableProperty ActiveBackgroundColorProperty = BindableProperty.Create(nameof(ActiveBackgroundColor), typeof(Color), typeof(ZgButtonGroup));

        public Color ActiveBackgroundColor
        {
            get => (Color) GetValue(ActiveBackgroundColorProperty); 
            set => SetValue(ActiveBackgroundColorProperty, value);
        }

        public static BindableProperty InactiveBackgroundColorProperty = BindableProperty.Create(nameof(InactiveBackgroundColor), typeof(Color), typeof(ZgButtonGroup));

        public Color InactiveBackgroundColor
        {
            get => (Color) GetValue(InactiveBackgroundColorProperty); 
            set => SetValue(InactiveBackgroundColorProperty, value);
        }

        public static BindableProperty ActiveTextColorProperty = BindableProperty.Create(nameof(ActiveTextColor), typeof(Color), typeof(ZgButtonGroup));

        public Color ActiveTextColor
        {
            get => (Color) GetValue(ActiveTextColorProperty); 
            set => SetValue(ActiveTextColorProperty, value);
        }

        public static BindableProperty InactiveTextColorProperty = BindableProperty.Create(nameof(InactiveTextColor), typeof(Color), typeof(ZgButtonGroup));

        public Color InactiveTextColor
        {
            get => (Color) GetValue(InactiveTextColorProperty); 
            set => SetValue(InactiveTextColorProperty, value);
        }

        public static BindableProperty LabelPaddingProperty = BindableProperty.Create(nameof(LabelPadding), typeof(Thickness), typeof(ZgButtonGroup));

        public Thickness LabelPadding
        {
            get => (Thickness) GetValue(LabelPaddingProperty); 
            set => SetValue(LabelPaddingProperty, value);
        }

        public static BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(FontFamily), typeof(string), typeof(ZgButtonGroup));

        public string FontFamily
        {
            get => (string) GetValue(FontFamilyProperty); 
            set => SetValue(FontFamilyProperty, value);
        }

        public static BindableProperty FontSizeProperty = BindableProperty.Create(nameof(FontSize), typeof(double), typeof(ZgButtonGroup));

        public double FontSize
        {
            get => (double) GetValue(FontSizeProperty); 
            set => SetValue(FontSizeProperty, value);
        }

        public static BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(ZgButtonGroup));

        public float CornerRadius
        {
            get => (float) GetValue(CornerRadiusProperty); 
            set => SetValue(CornerRadiusProperty, value);
        }
        
        public static BindableProperty FlexLayoutBasisProperty = BindableProperty.Create(nameof(FlexLayoutBasis), typeof(FlexBasis), typeof(ZgButtonGroup));

        public FlexBasis FlexLayoutBasis
        {
            get => (FlexBasis) GetValue(FlexLayoutBasisProperty); 
            set => SetValue(FlexLayoutBasisProperty, value);
        }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);

            if (propertyName == nameof(SelectedItem) && ItemsSource != null)
            {
                foreach (var item in ItemsSource)
                {
                    if (SelectedItem != null && item == SelectedItem)
                    {
                        SelectedItem.IsChecked = true;
                        continue;
                    }

                    item.IsChecked = false;
                }

                return;
            }

            if (propertyName == nameof(ItemsSource) && ItemsSource != null)
            {
                var items = ItemsSource.ToList();
                var itemsCount = items.Count;
                var chunks = 1f;
                if (itemsCount > 0)
                {
                    chunks /= itemsCount;
                }
                FlexLayoutBasis = new FlexBasis(chunks, true);
            }
        }

        private void OnCheckedChanged(object sender, CheckedChangedEventArgs args)
        {
            SelectedItem = (ZgButtonGroupItem) args.CurrentItem.BindingContext;
        }

        private void OnTapped(object sender, EventArgs e)
        {
            if (sender is Grid grid)
            {
                if (grid.BindingContext is ZgButtonGroupItem item)
                {
                    item.IsChecked = true;
                }
            }
        }
    }

    public class ZgButtonGroupItem : BindableBase
    {
        private string _label;
        private bool _isChecked;
        private ICommand _command;
        private object _commandParameter;

        public string Label
        {
            get => _label;
            set => SetProperty(ref _label, value);
        }

        public bool IsChecked
        {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        public ICommand Command
        {
            get => _command;
            set => SetProperty(ref _command, value);
        }

        public object CommandParameter
        {
            get => _commandParameter;
            set => SetProperty(ref _commandParameter, value);
        }

        protected override void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            base.OnPropertyChanged(args);

            if (args.PropertyName == nameof(IsChecked) && IsChecked)
            {
                Command?.Execute(CommandParameter);
            }
        }
    }
}