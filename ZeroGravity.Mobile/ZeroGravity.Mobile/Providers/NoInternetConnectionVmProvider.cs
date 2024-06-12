using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class NoInternetConnectionVmProvider : PageVmProviderBase, INoInternetConnectionPageVmProvider
    {
        private readonly ILogger _logger;

        public NoInternetConnectionVmProvider(ITokenService tokenService, ILoggerFactory loggerFactory) : base(tokenService)
        {
            _logger = loggerFactory?.CreateLogger<NoInternetConnectionVmProvider>() ?? new NullLogger<NoInternetConnectionVmProvider>();
        }
    }
}
