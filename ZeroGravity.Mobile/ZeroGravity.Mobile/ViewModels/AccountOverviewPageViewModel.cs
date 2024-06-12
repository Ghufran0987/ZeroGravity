using Microsoft.Extensions.Logging;
using Prism.Events;
using Prism.Navigation;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;

namespace ZeroGravity.Mobile.ViewModels
{
    public class AccountOverviewPageViewModel : VmBase<IAccountOverviewPage, IAccountOverviewPageVmProvider,
        AccountOverviewPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;

        public AccountOverviewPageViewModel(IVmCommonService service, IAccountOverviewPageVmProvider provider,
            ILoggerFactory loggerFactory, IEventAggregator eventAggregator) : base(service, provider, loggerFactory)
        {
            _eventAggregator = eventAggregator;
        }

        public override void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            Page?.NavigateToActiveTab(parameters);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            Page?.NavigateFromActiveTab(parameters);
        }
    }
}