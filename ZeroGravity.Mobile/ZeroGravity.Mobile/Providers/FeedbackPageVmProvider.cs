using System;
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
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class FeedbackPageVmProvider : PageVmProviderBase, IFeedbackPageVmProvider
    {
        private readonly IFeedbackService _feedbackService;
        private readonly ILogger _logger;
        IUserDataService _userDataService;
        public FeedbackPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService, IFeedbackService feedbackService, IUserDataService userDataService) : base(tokenService)
        {
            _feedbackService = feedbackService;
            _userDataService = userDataService;
            _logger = loggerFactory?.CreateLogger<FeedbackPageVmProvider>() ??
                      new NullLogger<FeedbackPageVmProvider>();
        }

        public async Task<ApiCallResult<FeedbackSummaryProxy>> GetFeedbackSummaryAsync( DateTime targetDateTime, CancellationToken cancellationToken)
        {
            var apiCallResult =
                await _feedbackService.GetFeedbackSummaryDtoByDateAsync(targetDateTime, cancellationToken);

            if (apiCallResult.Success)
            {
                var feedbackSummaryProxy = ProxyConverter.GetFeedbackSummaryProxy(apiCallResult.Value);

                feedbackSummaryProxy.WaterFeedbackDataProxy =
                    _feedbackService.SetFeedbackState(feedbackSummaryProxy.WaterFeedbackDataProxy);

                feedbackSummaryProxy.CalorieDrinkFeedbackDataProxy =
                    _feedbackService.SetFeedbackState(feedbackSummaryProxy.CalorieDrinkFeedbackDataProxy);

                feedbackSummaryProxy.ActivityFeedbackDataProxy =
                    _feedbackService.SetFeedbackState(feedbackSummaryProxy.ActivityFeedbackDataProxy);


                feedbackSummaryProxy.BreakfastFeedbackDataProxy =
                    _feedbackService.SetFeedbackState(feedbackSummaryProxy.BreakfastFeedbackDataProxy);

                feedbackSummaryProxy.LunchFeedbackDataProxy =
                    _feedbackService.SetFeedbackState(feedbackSummaryProxy.LunchFeedbackDataProxy);

                feedbackSummaryProxy.DinnerFeedbackDataProxy =
                    _feedbackService.SetFeedbackState(feedbackSummaryProxy.DinnerFeedbackDataProxy);


                feedbackSummaryProxy.HealthySnackFeedbackDataProxy =
                    _feedbackService.SetFeedbackState(feedbackSummaryProxy.HealthySnackFeedbackDataProxy);

                feedbackSummaryProxy.UnhealthySnackFeedbackDataProxy =
                    _feedbackService.SetFeedbackState(feedbackSummaryProxy.UnhealthySnackFeedbackDataProxy);    
                
                feedbackSummaryProxy.MeditationFeedbackDataProxy =
                    _feedbackService.SetFeedbackState(feedbackSummaryProxy.MeditationFeedbackDataProxy);

                return ApiCallResult<FeedbackSummaryProxy>.Ok(feedbackSummaryProxy);
            }

            return ApiCallResult<FeedbackSummaryProxy>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

        public async Task<ApiCallResult<AccountResponseDto>> GetAccountDetailsAsync( CancellationToken cancellationToken)
        {
            var apiCallResult =
                await _userDataService.GetAccountDataAsync(cancellationToken);

            if (apiCallResult.Success)
            {
                return ApiCallResult<AccountResponseDto>.Ok(apiCallResult.Value);
            }

            return ApiCallResult<AccountResponseDto>.Error(apiCallResult.ErrorMessage, apiCallResult.ErrorReason);
        }

    }
}