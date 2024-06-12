using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IRegisterPageVmProvider : IPageVmProvider
    {
        Task<RegisterResult> RegisterAsync(string email, string password, string confirmPassword, bool acceptTerms, bool wantsNewsletter, CancellationToken cancellationToken);
    }

    public interface ILoginAlertPageVmProvider : IPageVmProvider
    {
        
    }
}
