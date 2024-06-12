using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Resources.Fonts;

namespace ZeroGravity.Mobile.Base
{
    public abstract class ContentViewBase<T> : ContentView, IPage where T : class, IPage
    {
        //private readonly Label _navigationTitle;
        
        protected ContentViewBase()
        {
            BindingContextChanged += PageBindingContextChanged;

            ViewModelLocator.SetAutowireViewModel(this, true);
            BackgroundColor = CustomColors.White;

            //_navigationTitle = new Label
            //{
            //    Style = (Style)Application.Current.Resources["NavigationTitle-Label"],
            //};
            //_navigationTitle.SetBinding(Label.TextProperty, new Binding("Title"));
            //NavigationPage.SetTitleView(this, _navigationTitle);

            //Overlay = new ContentView();
            CloseOverlayCommand = new DelegateCommand(CloseOverlay);
            
        }

        private void PageBindingContextChanged(object sender, EventArgs e)
        {
            if (BindingContext is IVm<T> vm)
            {
                vm.SetPage(this as T);
            }

            if (Overlay != null)
            {
                Overlay.BindingContext = BindingContext;
            }
        }

        public virtual void InitPage(object obj)
        {
            
        }
        
        public void OpenOverlay()
        {
            if (ShowOverlay) return;

            NavigationPage.SetHasNavigationBar(this, false);            
            ShowOverlay = true;
        }

        public void CloseOverlay()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            ShowOverlay = false;
        }
        
        public static BindableProperty ShowTopBorderProperty = BindableProperty.Create(nameof(ShowTopBorder), typeof(bool), typeof(ContentViewBase<T>));

        public bool ShowTopBorder
        {
            get => (bool)GetValue(ShowTopBorderProperty);
            set => SetValue(ShowTopBorderProperty, value);
        }

        public static BindableProperty ShowBottomBorderProperty = BindableProperty.Create(nameof(ShowBottomBorder), typeof(bool), typeof(ContentViewBase<T>));

        public bool ShowBottomBorder
        {
            get => (bool)GetValue(ShowBottomBorderProperty);
            set => SetValue(ShowBottomBorderProperty, value);
        }

        public static BindableProperty ShowBusyIndicatorProperty = BindableProperty.Create(nameof(ShowBusyIndicator), typeof(bool), typeof(ContentViewBase<T>));

        public bool ShowBusyIndicator
        {
            get => (bool)GetValue(ShowBusyIndicatorProperty);
            set => SetValue(ShowBusyIndicatorProperty, value);
        }

        public static BindableProperty ShowOverlayProperty = BindableProperty.Create(nameof(ShowOverlay), typeof(bool), typeof(ContentViewBase<T>));

        public bool ShowOverlay
        {
            get => (bool)GetValue(ShowOverlayProperty);
            private set => SetValue(ShowOverlayProperty, value);
        }

        public static BindableProperty OverlayProperty = BindableProperty.Create(nameof(Overlay), typeof(View), typeof(ContentViewBase<T>));

        public View Overlay
        {
            get => (View)GetValue(OverlayProperty);
            set => SetValue(OverlayProperty, value);
        }

        public static BindableProperty CloseOverlayCommandProperty = BindableProperty.Create(nameof(CloseOverlayCommand), typeof(ICommand), typeof(ContentViewBase<T>));

        public ICommand CloseOverlayCommand
        {
            get => (ICommand)GetValue(CloseOverlayCommandProperty);
            private set => SetValue(CloseOverlayCommandProperty, value);
        }
    }
}
