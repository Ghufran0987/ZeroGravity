using System;
using System.Windows.Input;
using Prism.Commands;
using Prism.Mvvm;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resources.Fonts;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Base
{
    public abstract class ContentPageBase<T> : ContentPage, IPage where T : class, IPage
    {
        private readonly Color _defaultNavBarBackgroundColor = CustomColors.White;
        private readonly Color _defaultNavBarTextColor = CustomColors.Pink;

        private readonly Label _navigationTitle;

        protected ContentPageBase()
        {
            Xamarin.Forms.PlatformConfiguration.iOSSpecific.Page.SetUseSafeArea(this, true);
            ViewModelLocator.SetAutowireViewModel(this, true);
            BindingContextChanged += PageBindingContextChanged;

            _navigationTitle = new Label
            {
                Style = (Style)Application.Current.Resources["NavigationTitle-Label"]
            };
            //  _navigationTitle.SetBinding(Label.TextProperty, new Binding(TitleProperty.PropertyName));
            NavigationPage.SetTitleView(this, _navigationTitle);

            //Overlay = new ContentView();
            BackgroundColor = CustomColors.White;
            CloseOverlayCommand = new DelegateCommand(CloseOverlay);
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

            if (this.BindingContext is MealsSnacksBreakfastPageViewModel)
            {
                (this.BindingContext as MealsSnacksBreakfastPageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is MealsSnacksLunchPageViewModel)
            {
                (this.BindingContext as MealsSnacksLunchPageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is MealsSnacksUnhealthySnackPageVm)
            {
                (this.BindingContext as MealsSnacksUnhealthySnackPageVm).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is MealsSnacksHealthySnackPageViewModel)
            {
                (this.BindingContext as MealsSnacksHealthySnackPageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is MealsSnacksDinnerPageViewModel)
            {
                (this.BindingContext as MealsSnacksDinnerPageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is WaterIntakePageViewModel)
            {
                (this.BindingContext as WaterIntakePageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is CalorieDrinksAlcoholPageViewModel)
            {
                (this.BindingContext as CalorieDrinksAlcoholPageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is ActivitiesPageViewModel)
            {
                (this.BindingContext as ActivitiesPageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is WellbeingDataPageViewModel)
            {
                (this.BindingContext as WellbeingDataPageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is MediaElementViewPageViewModel)
            {
                (this.BindingContext as MediaElementViewPageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is MeditationAreaPageViewModel)
            {
                (this.BindingContext as MeditationAreaPageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is FastingDataPageViewModel)
            {
                (this.BindingContext as FastingDataPageViewModel).OnDailyCloseOverlay();
            }
            else if (this.BindingContext is SugarBeatScanPageViewModel)
            {
                (this.BindingContext as SugarBeatScanPageViewModel).OnDailyCloseOverlay();
            }

            ShowOverlay = false;
        }

        private void SetNavigationBarBackgroundAndTextColor(Color backgroundColor, Color textColor)
        {
            try
            {
                if (Application.Current.MainPage != null)
                {
                    var page = Application.Current.MainPage as NavigationPage;
                    if (page != null)
                    {
                        page.BarBackgroundColor = backgroundColor;
                        // navigation arrow
                        page.BarTextColor = textColor;
                        // Title text color
                        _navigationTitle.TextColor = textColor;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is IVm<T> vm)
            {
                vm.SetPage(this as T);

                // dynamisches Ändern der Navigations-Bar
                if (ShowGreyBackground)
                {
                    SetNavigationBarBackgroundAndTextColor(CustomColors.GrayBackgroundColor, _defaultNavBarTextColor);
                }
                else
                {
                    if (vm is IVm<IRegisterPage>)
                    {
                        // custom für RegisterPage
                        SetNavigationBarBackgroundAndTextColor(CustomColors.White, CustomColors.PinkBackground);
                    }
                    else
                    {
                        // default für alle anderen Pages
                        SetNavigationBarBackgroundAndTextColor(_defaultNavBarBackgroundColor, _defaultNavBarTextColor);
                    }
                }
            }

            if (Overlay != null)
            {
                Overlay.BindingContext = BindingContext;
            }
        }

        private void PageBindingContextChanged(object sender, EventArgs e)
        {
            if (BindingContext is IVm<T> vm)
            {
                vm.SetPage(this as T);
            }
        }

        public static BindableProperty ShowGreyBackgroundProperty = BindableProperty.Create(nameof(ShowGreyBackground), typeof(bool), typeof(ContentPageBase<T>));

        public bool ShowGreyBackground
        {
            get => (bool)GetValue(ShowGreyBackgroundProperty);
            set => SetValue(ShowGreyBackgroundProperty, value);
        }

        public static BindableProperty ShowTopBorderProperty = BindableProperty.Create(nameof(ShowTopBorder), typeof(bool), typeof(ContentPageBase<T>));

        public bool ShowTopBorder
        {
            get => (bool)GetValue(ShowTopBorderProperty);
            set => SetValue(ShowTopBorderProperty, value);
        }

        public static BindableProperty ShowBottomBorderProperty = BindableProperty.Create(nameof(ShowBottomBorder), typeof(bool), typeof(ContentPageBase<T>));

        public bool ShowBottomBorder
        {
            get => (bool)GetValue(ShowBottomBorderProperty);
            set => SetValue(ShowBottomBorderProperty, value);
        }

        public static BindableProperty ShowBusyIndicatorProperty = BindableProperty.Create(nameof(ShowBusyIndicator), typeof(bool), typeof(ContentPageBase<T>));

        public bool ShowBusyIndicator
        {
            get => (bool)GetValue(ShowBusyIndicatorProperty);
            set => SetValue(ShowBusyIndicatorProperty, value);
        }

        public static BindableProperty ShowOverlayProperty = BindableProperty.Create(nameof(ShowOverlay), typeof(bool), typeof(ContentPageBase<T>));

        public bool ShowOverlay
        {
            get => (bool)GetValue(ShowOverlayProperty);
            private set => SetValue(ShowOverlayProperty, value);
        }

        public static BindableProperty OverlayProperty = BindableProperty.Create(nameof(Overlay), typeof(View), typeof(ContentPageBase<T>));

        public View Overlay
        {
            get => (View)GetValue(OverlayProperty);
            set => SetValue(OverlayProperty, value);
        }

        public static BindableProperty CloseOverlayCommandProperty = BindableProperty.Create(nameof(CloseOverlayCommand), typeof(ICommand), typeof(ContentPageBase<T>));

        public ICommand CloseOverlayCommand
        {
            get => (ICommand)GetValue(CloseOverlayCommandProperty);
            private set => SetValue(CloseOverlayCommandProperty, value);
        }
    }
}