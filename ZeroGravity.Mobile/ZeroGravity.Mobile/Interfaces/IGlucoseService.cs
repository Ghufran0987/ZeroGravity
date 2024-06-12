using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IGlucoseService
    {
        Task<ApiCallResult<GlucoseDataDto>> SaveGlucoseAsync(GlucoseDataDto glucoseDataDto, CancellationToken cancellationToken);
    }
}
