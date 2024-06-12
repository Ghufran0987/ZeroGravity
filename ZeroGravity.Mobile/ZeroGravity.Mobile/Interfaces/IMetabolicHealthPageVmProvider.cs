using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IMetabolicHealthPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<GlucoseBaseProxy>> SaveGlucoseAsync(GlucoseBaseProxy proxy, CancellationToken cancellationToken);
    }
}
