using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IEducationalInfoProvider
    {
        Task<ApiCallResult<EducationalInfoDto>> GetEducationalInfoByIdAsync(CancellationToken cancellationToken,string category);
    }
}
