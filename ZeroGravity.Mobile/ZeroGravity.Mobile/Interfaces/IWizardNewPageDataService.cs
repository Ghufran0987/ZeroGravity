using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IWizardNewPageDataService
    {
        Task<ApiCallResult<QuestionDto>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ApiCallResult<List<QuestionDto>>> GetQuestionsAsync(string category, bool isActive, CancellationToken cancellationToken);

    }
}
