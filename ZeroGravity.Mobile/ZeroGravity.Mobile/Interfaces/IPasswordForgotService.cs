using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IPasswordForgotService
    {
        Task<PasswordForgotResult> RequestForgetLinkAsync(string email, CancellationToken cancellationToken);
    }
}
