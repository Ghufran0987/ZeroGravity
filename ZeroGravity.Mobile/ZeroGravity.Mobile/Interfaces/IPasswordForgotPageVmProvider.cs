using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IPasswordForgotPageVmProvider : IPageVmProvider
    {
        Task<PasswordForgotResult> RequestForgetLinkAsync(string email, CancellationToken cancellationToken);
    }
}
