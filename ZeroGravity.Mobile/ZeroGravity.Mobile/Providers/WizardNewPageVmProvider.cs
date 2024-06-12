using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Base.Proxy;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Proxies;

namespace ZeroGravity.Mobile.Providers
{
    public class WizardNewPageVmProvider : PageVmProviderBase, IWizardNewPageVmProvider
    {
        private readonly IWizardNewPageDataService _dataServce;
        private readonly ILogger _logger;

        public WizardNewPageVmProvider(ILoggerFactory loggerFactory, IWizardNewPageDataService dataServce, ITokenService tokenService) : base(tokenService)
        {
            _dataServce = dataServce;
            _logger = loggerFactory?.CreateLogger<WizardNewPageVmProvider>() ??
                      new NullLogger<WizardNewPageVmProvider>();
        }

        public async Task<ApiCallResult<QuestionProxy>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var apiCallResult = await _dataServce.GetByIdAsync(id, cancellationToken);

            if (apiCallResult.Success)
            {
                var dayToDayActivityProxy = ProxyConverter.GetQuestionProxy(apiCallResult.Value);
                return ApiCallResult<QuestionProxy>.Ok(dayToDayActivityProxy);
            }

            return ApiCallResult<QuestionProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<List<QuestionProxy>>> GetQuestionsAsync(string category, bool isActive, CancellationToken cancellationToken)
        {
            var apiCallResult = await _dataServce.GetQuestionsAsync(category, isActive, cancellationToken);

            if (apiCallResult.Success)
            {
                List<QuestionProxy> proxies = new List<QuestionProxy>();

                foreach (var dto in apiCallResult.Value)
                {
                    var proxy = ProxyConverter.GetQuestionProxy(dto);

                    proxies.Add(proxy);
                }
                return ApiCallResult<List<QuestionProxy>>.Ok(proxies);
            }
            return ApiCallResult<List<QuestionProxy>>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }
    }
}