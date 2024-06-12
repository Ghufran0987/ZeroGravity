using Prism.Navigation;

namespace ZeroGravity.Mobile.Base.Interfaces
{
    public interface IVm<in T> : INavigatedAware, IDestructible where T : IPage
    {
        void SetPage(T page);
    }
}
