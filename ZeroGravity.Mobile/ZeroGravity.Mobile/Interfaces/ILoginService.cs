using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ILoginService
    {
        Task<LoginResult> LoginAsync(string email, string password, CancellationToken cancellationToken, bool saveToken = false);
        Task<LoginResult> LoginSilent();
    }
}
