using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IIntegrationDetailPageVmProvider : IPageVmProvider
    {
        void OpenNativeWebBrowser(string url);

        Task<ApiCallResult<FitbitAccountProxy>> GetFitbitAccountAsnyc(CancellationToken cancellationToken);
    }
}