using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.NavigationParameter;

namespace ZeroGravity.Mobile.Interfaces.Page
{
    public interface IContentShellPage : IPage
    {
        void NavigateToTab(PageNavigationParams navParams);
        void NavigateFromLastTabContent();
    }
}