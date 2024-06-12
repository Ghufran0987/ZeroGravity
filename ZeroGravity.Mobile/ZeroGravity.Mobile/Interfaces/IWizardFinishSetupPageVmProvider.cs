using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IWizardFinishSetupPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<PersonalDataProxy>> GetPersonalDataAsnyc(CancellationToken cancellationToken);

        Task<ApiCallResult<PersonalGoalProxy>> GetPersonalGoalAsnyc(CancellationToken cancellationToken);

        Task<ApiCallResult<PersonalGoalProxy>> CreatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy, CancellationToken cancellationToken);

        Task<ApiCallResult<PersonalGoalProxy>> UpdatePersonalGoalAsnyc(PersonalGoalProxy personalGoalProxy, CancellationToken cancellationToken);
        Task<ApiCallResult<bool>> UpdateWizardCompletionStatusAsync(UpdateWizardRequestDto updateDto, CancellationToken token);

        Task<ApiCallResult<List<AnalysisItemProxy>>> GetAnalysisSummaryDataByDateAsync(DateTime dateTime,
            CancellationToken cancellationToken);
    }
}
