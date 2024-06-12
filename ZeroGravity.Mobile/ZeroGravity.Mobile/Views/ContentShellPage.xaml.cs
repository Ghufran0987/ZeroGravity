using System;
using ZeroGravity.Mobile.Contract.Helper;
using ZeroGravity.Mobile.Contract.NavigationParameter;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.ViewModels;

namespace ZeroGravity.Mobile.Views
{
    public partial class ContentShellPage : IContentShellPage
    {
        private readonly ITabBarPage _tabBar;

        public ContentShellPage()
        {
            try
            {
                InitializeComponent();

               // call OnNavigatedTo in ProfileImagePageViewModel
              // var profileImgPage = ProfileImgPage.BindingContext as ViewModelBase;

                //var navParams = NavigationParametersHelper.CreateNavigationParameter(Title);
               // profileImgPage?.OnNavigatedTo(navParams);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            _tabBar = _tabBarPage;
        }

        public void NavigateToTab(PageNavigationParams navParams)
        {
            _tabBar.NavigateToTab(navParams);
        }

        public void NavigateFromLastTabContent()
        {
            _tabBar.NavigateFromLastTabContent();
        }
    }
}