using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface ILoginPageVmProvider : IPageVmProvider
    {
        /// <summary>
        /// Logs the user in by calling the authentication API. 
        /// </summary>
        /// <param name="email">The user's email.</param>
        /// <param name="password">The user's password.</param>
        /// <param name="cancellationToken">The cancellation token to abort the request.</param>
        /// <param name="saveInput">Whether to save the user's inout (for security reasons the email only).</param>
        /// <returns></returns>
        Task<LoginResult> LoginAsync(string email, string password, CancellationToken cancellationToken, bool saveInput= false);
        

        //Task SaveCredentials(bool saveInput, string email, string password);

        /// <summary>
        /// Gets the user's credentials (if user decided to save them)
        /// </summary>
        /// <returns>A <see cref="LoginProxy"/> object.</returns>
        Task<LoginProxy> GetLoginProxy();

        Task<ApiCallResult<AccountResponseDto>> GetAccountDataAsnyc(CancellationToken token);
    }
}
