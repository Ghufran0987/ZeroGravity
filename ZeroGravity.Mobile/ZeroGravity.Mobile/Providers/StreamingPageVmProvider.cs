using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract.Constants;
using ZeroGravity.Mobile.Contract.Enums;
using ZeroGravity.Mobile.Contract.Models;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models.Dto;

namespace ZeroGravity.Mobile.Providers
{
    public class StreamingPageVmProvider : PageVmProviderBase, IStreamingPageVmProvider
    {
        private readonly IApiService _api;
        private readonly ILogger _logger;

        public StreamingPageVmProvider(ILoggerFactory loggerFactory, ITokenService tokenService, IApiService api) : base(tokenService)
        {
            _api = api;
            _logger = loggerFactory?.CreateLogger<StreamingPageVmProvider>() ??
                      new NullLogger<StreamingPageVmProvider>();
        }

        public async Task<ApiCallResult<IEnumerable<StreamContentDto>>> GetAvailableStreamContentAsync(CancellationToken token = new CancellationToken())
        {
            var baseUrl = Common.ServerUrl;
            var type = (int)StreamContentType.Stream;
            // var url = $"{baseUrl}/stream/{type}/all";
            var url = $"{baseUrl}/video/{type}/all";

            try
            {
                var list = await _api.GetAllJsonAsync<StreamContentDto>(url, token);

                var result = ApiCallResult<IEnumerable<StreamContentDto>>.Ok(list);

                return result;
            }
            catch (Exception e)
            {
                return ApiCallResult<IEnumerable<StreamContentDto>>.Error(e.Message);
            }
        }
    }
}