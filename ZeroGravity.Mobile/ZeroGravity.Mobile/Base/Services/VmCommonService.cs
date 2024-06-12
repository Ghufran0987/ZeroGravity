using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Base.Services
{
    public class VmCommonService : IVmCommonService
    {
        public VmCommonService(
            INavigationService navigationService, 
            IEventAggregator eventAggregator, 
            IPageDialogService pageDialogService, 
            ISemaphoreAsyncLockService semaphoreAsyncLockService, 
            ILocalNotificationsService localNotificationsService, 
            IHoldingPagesSettingsService holdingPagesSettingsService)
        {
            NavigationService = navigationService;
            DialogService = pageDialogService;
            EventAggregator = eventAggregator;
            SemaphoreAsyncLockService = semaphoreAsyncLockService;
            LocalNotificationsService = localNotificationsService;
            HoldingPagesSettingsService = holdingPagesSettingsService;
        }

        public INavigationService NavigationService { get; }
        public IPageDialogService DialogService { get; }
        public IEventAggregator EventAggregator { get; }
        public ISemaphoreAsyncLockService SemaphoreAsyncLockService { get; }
        public ILocalNotificationsService LocalNotificationsService { get; }
        public IHoldingPagesSettingsService HoldingPagesSettingsService { get; }
    }
}
