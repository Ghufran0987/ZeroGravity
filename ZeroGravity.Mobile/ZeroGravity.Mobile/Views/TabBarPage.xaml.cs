using System.Linq;
using System.Threading.Tasks;
using Syncfusion.XForms.TabView;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TabBarPage : ITabBarPage
    {
        private PageNavigationParams _pageNavigationParams = null;
        private ViewModelBase _actualModelBase;

        public TabBarPage()
        {
            InitializeComponent();

            SetTab(AppResources.TabPage_MyDay);
            
            TabView.SelectionChanged += TabViewOnSelectionChanged;
        }

        private void TabViewOnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var navParams = NavigationParametersHelper.CreateNavigationParameter(e.Name);

            _actualModelBase?.OnNavigatedFrom(navParams);

            var sfTabItem = TabView.Items.ElementAt(e.Index);
            var view = sfTabItem.Content;
            view.IsVisible = true;
            var contentBindingContext = view.BindingContext as ViewModelBase;

            if (_pageNavigationParams != null)
            {
                navParams = NavigationParametersHelper.CreateNavigationParameter(_pageNavigationParams);
            }

            contentBindingContext?.OnNavigatedTo(navParams);

            //make the context the new ActualContext
            _actualModelBase = contentBindingContext;

            _pageNavigationParams = null;
        }

        public void NavigateToTab(PageNavigationParams navParams)
        {
            if (navParams.PageName == ViewName.FeedbackPage)
            {
                _pageNavigationParams = navParams;
                var tabTitle = AppResources.TabPage_Feedback;
                SetTab(tabTitle);
            }
        }

        public void NavigateFromLastTabContent()
        {
            _actualModelBase?.OnNavigatedFrom(null);
        }

        private void SetTab(string tabTitle)
        {
            var firstItem = TabView.Items.FirstOrDefault(_ => _.Title.Equals(tabTitle));

            if (firstItem != null)
            {
                // verhindert, dass bei nicht vorhandener Internetverbindung (z.B. Flugmodus) bei iOS die App crasht
                Task.Run(async () =>
                {
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        TabView.SelectedIndex = TabView.Items.IndexOf(firstItem);
                    });
                });
            }
        }
    }
}