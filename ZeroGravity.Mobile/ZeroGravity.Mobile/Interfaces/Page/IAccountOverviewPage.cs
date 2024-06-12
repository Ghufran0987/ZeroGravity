using Prism.Navigation;
using ZeroGravity.Mobile.Base.Interfaces;

namespace ZeroGravity.Mobile.Interfaces.Page
{
    public interface IAccountOverviewPage : IPage
    {
        void NavigateToActiveTab(INavigationParameters parameters);
        void NavigateFromActiveTab(INavigationParameters parameters);
    }
}