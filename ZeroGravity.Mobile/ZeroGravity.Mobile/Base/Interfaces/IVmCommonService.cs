using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Base.Interfaces
{
    /// <summary>
    /// This interface provides instances of Navigation and Dialog Service.
    /// These two services are meant to be useable from ViewModels directly.
    /// Instances of all other required services should be obtained using a provider.
    /// </summary>
    public interface IVmCommonService
    {
        /// <summary>
        /// The prism <see cref="INavigationService"/>.
        /// </summary>
        INavigationService NavigationService { get; }

        /// <summary>
        /// The prism <see cref="IPageDialogService"/>.
        /// </summary>
        IPageDialogService DialogService { get; }

        IEventAggregator EventAggregator { get; }

        ISemaphoreAsyncLockService SemaphoreAsyncLockService { get; }

        ILocalNotificationsService LocalNotificationsService { get; }

        IHoldingPagesSettingsService HoldingPagesSettingsService { get; }
    }
}
