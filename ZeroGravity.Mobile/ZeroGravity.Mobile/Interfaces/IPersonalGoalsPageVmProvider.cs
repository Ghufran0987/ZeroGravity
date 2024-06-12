using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IPersonalGoalsPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<PersonalDataProxy>> GetPersonalDataAsnyc(CancellationToken cancellationToken);

        Task<ApiCallResult<PersonalGoalProxy>> GetPersonalGoalAsnyc(CancellationToken cancellationToken);

        Task<ApiCallResult<PersonalGoalProxy>> CreatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy, CancellationToken cancellationToken);

        Task<ApiCallResult<PersonalGoalProxy>> UpdatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy, CancellationToken cancellationToken);
    }
}