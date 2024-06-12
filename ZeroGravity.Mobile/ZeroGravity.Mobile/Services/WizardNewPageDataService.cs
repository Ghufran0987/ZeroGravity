using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Exceptions;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Models.Dto;


namespace ZeroGravity.Mobile.Services
{
    public class WizardNewPageDataService : IWizardNewPageDataService
    {

        private readonly IApiService _apiService;
        private readonly ILogger _logger;
        public WizardNewPageDataService(ILoggerFactory loggerFactory, IApiService apiService)
        {
            _apiService = apiService;
            _logger = loggerFactory?.CreateLogger<WizardNewPageDataService>() ?? new NullLogger<WizardNewPageDataService>();
        }

        public async Task<ApiCallResult<QuestionDto>> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;
            var api = "/questiondata/" + id;
            var url = baseUrl + api;

            try
            {
                var activityDataDto = await _apiService.GetSingleJsonAsync<QuestionDto>(url, cancellationToken);

                return ApiCallResult<QuestionDto>.Ok(activityDataDto);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to GetByAccountIdAsync for WizardNewPageDataService.");
                    return ApiCallResult<QuestionDto>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user.");
                return ApiCallResult<QuestionDto>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<QuestionDto>.Error(ex.CustomErrorMessage);
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<QuestionDto>.Error(AppResources.Common_Error_Unknown);
            }
        }

        public async Task<ApiCallResult<List<QuestionDto>>> GetQuestionsAsync(string category, bool isActive, CancellationToken cancellationToken)
        {
            var baseUrl = Common.ServerUrl;


            var api = $"/questiondata/{category}/{isActive}";
            var url = baseUrl + api;

            try
            {
                var result = await _apiService.GetAllJsonAsync<QuestionDto>(url, cancellationToken);

                return ApiCallResult<List<QuestionDto>>.Ok(result);
            }
            catch (TaskCanceledException e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    // Timed Out
                    _logger.LogInformation("Time out while attempting to WizardNewPageDataService->GetQuestionsAsync.");
                    return ApiCallResult<List<QuestionDto>>.Error(e.Message, ErrorReason.TimeOut);
                }

                // Cancelled for some other reason
                _logger.LogInformation("Unfinished request cancelled by user at WizardNewPageDataService->GetQuestionsAsync.");
                return ApiCallResult<List<QuestionDto>>.Error(e.Message, ErrorReason.TaskCancelledByUserOperation);
            }
            catch (Exception e)
            {
                if (e is HttpException ex)
                {
                    _logger.LogInformation($"{e.Message}", e);
                    return ApiCallResult<List<QuestionDto>>.Error(ex.CustomErrorMessage + " at GetQuestionsAsync");
                }

                _logger.LogInformation($"{e.Message}", e);
                return ApiCallResult<List<QuestionDto>>.Error(e.Message);
            }
        }

    }
}
