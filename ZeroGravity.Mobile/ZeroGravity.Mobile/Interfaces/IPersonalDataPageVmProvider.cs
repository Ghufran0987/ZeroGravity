using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IPersonalDataPageVmProvider : IPageVmProvider
    {
        /// <summary>
        ///     Gets the personal date of the user by the accountId
        /// </summary>
        /// <returns></returns>
        Task<ApiCallResult<PersonalDataProxy>> GetPersonalDataAsnyc(CancellationToken cancellationToken);

        Task<ApiCallResult<PersonalDataProxy>> UpdatePersonalDataAsnyc(PersonalDataProxy personalDataProxy,
            CancellationToken cancellationToken);

        Task<ApiCallResult<PersonalDataProxy>> CreatePersonalDataAsnyc(PersonalDataProxy personalDataProxy,
            CancellationToken cancellationToken);

        double GetBmi(double height, double weight);

        double GetBfp(GenderBiologicalType sex, double waist, double hip, double neck, double height);

        PersonalGoalProxy CreateInitialTargets(int age, int gender, double weight, double bodyMassIndex,
            double bodyFatPercentage,
            double bloodGlucoseLevel, int fastingType);

        Task<ApiCallResult<PersonalGoalProxy>> CreatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy,
            CancellationToken cancellationToken);

        Task<ApiCallResult<PersonalGoalProxy>> UpdatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy,
            CancellationToken cancellationToken);
    }
}