using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Contract.Models.Notification;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IProfileImagePageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<byte[]>> GetProfilePictureAsync(CancellationToken cancellationToken);
        Task<List<Notification>> GetNotificationsAsync(CancellationToken caneCancellationToken);
    }
}