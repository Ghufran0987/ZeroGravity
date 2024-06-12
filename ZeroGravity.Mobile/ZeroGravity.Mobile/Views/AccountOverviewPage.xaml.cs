using System;
using System.Linq;
using Prism.Navigation;
using Syncfusion.XForms.TabView;
using Syncfusion.XForms.Border;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using SelectionChangedEventArgs = Syncfusion.XForms.TabView.SelectionChangedEventArgs;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AccountOverviewPage : IAccountOverviewPage
    {
        private ViewModelBase _actualModelBase;

        public AccountOverviewPage()
        {
            InitializeComponent();

            // TabView.SelectionChanged += TabViewOnSelectionChanged;

            var sfTabItem = TabView.Items.ElementAt(TabView.SelectedIndex);

            var view = sfTabItem.Content;

            var navParams = NavigationParametersHelper.CreateNavigationParameter(sfTabItem.Title);

            _actualModelBase = view.BindingContext as ViewModelBase;

            if (view.IsFocused)
            {
                _actualModelBase?.OnNavigatedTo(navParams);
            }
        }

        private void TabViewOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            foreach (var item in TabView.Items)
            {
                var innerBorder = (item.HeaderContent as SfBorder).Content as SfBorder;
                var ellipse = (innerBorder.Content as Grid).Children[0] as Ellipse;
                var label = (innerBorder.Content as Grid).Children[1] as Label;
                if (e.Index == TabView.Items.IndexOf(item))
                {
                    innerBorder.BackgroundColor = Color.FromHex("FF5869");
                    ellipse.Fill = new SolidColorBrush(Color.White);
                    ellipse.Stroke = new SolidColorBrush(Color.White);
                    label.TextColor = Color.White;
                }
                else
                {
                    innerBorder.BackgroundColor = Color.White;
                    ellipse.Fill = new SolidColorBrush(Color.White);
                    ellipse.Stroke = new SolidColorBrush(Color.FromHex("FF5869"));
                    label.TextColor = Color.FromHex("FF5869");
                }
            }

            var navParams = NavigationParametersHelper.CreateNavigationParameter(e.Name);

            _actualModelBase?.OnNavigatedFrom(navParams);

            var sfTabItem = TabView.Items.ElementAt(e.Index);
            var view = sfTabItem.Content;

            var contentBindingContext = view.BindingContext as ViewModelBase;
            contentBindingContext?.OnNavigatedTo(navParams);

            //make the context the new ActualContext
            _actualModelBase = contentBindingContext;
        }

        public void NavigateToActiveTab(INavigationParameters parameters)
        {
            var tabItem = TabView.Items.ElementAt(TabView.SelectedIndex);

            if (tabItem.Content.BindingContext is ViewModelBase vm)
            {
                vm.OnNavigatedTo(parameters);
            }
        }

        public void NavigateFromActiveTab(INavigationParameters parameters)
        {
            var tabItem = TabView.Items.ElementAt(TabView.SelectedIndex);

            if (tabItem.Content.BindingContext is ViewModelBase vm)
            {
                vm.OnNavigatedFrom(parameters);
            }
        }
    }
}