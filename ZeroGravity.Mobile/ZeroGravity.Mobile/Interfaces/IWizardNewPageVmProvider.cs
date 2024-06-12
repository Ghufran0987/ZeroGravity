using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public interface IWizardNewPageVmProvider : IPageVmProvider
    {
        Task<ApiCallResult<QuestionProxy>> GetByIdAsync(int id, CancellationToken cancellationToken);

        Task<ApiCallResult<List<QuestionProxy>>> GetQuestionsAsync(string category, bool isActive, CancellationToken cancellationToken);
    }
}