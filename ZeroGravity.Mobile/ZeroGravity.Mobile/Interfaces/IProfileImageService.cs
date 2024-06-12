using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IProfileImageService
    {
        Task<ApiCallResult<bool>> UploadImage(byte[] imageData, CancellationToken cancellationToken);
        Task<ApiCallResult<ProfileImageDto>> GetImage(CancellationToken cancellationToken);
    }
}
