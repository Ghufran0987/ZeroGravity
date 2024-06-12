using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IStreamingPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<IEnumerable<StreamContentDto>>> GetAvailableStreamContentAsync(CancellationToken token = new CancellationToken());
    }
}