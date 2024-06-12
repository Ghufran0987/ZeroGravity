using Microsoft.Extensions.Logging;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.Logging
{
    public class AppLoggerProvider : ILoggerProvider
    {
        private readonly ILoggingService _loggingService;
        private readonly IPermissionService _permissionService;

        public AppLoggerProvider(ILoggingService loggingService, IPermissionService permissionService)
        {
            _permissionService = permissionService;
            _loggingService = loggingService;
        }

        public void Dispose()
        {
        }

        /// <summary>
        /// Creates a logger which writes to a txt-file.
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public ILogger CreateLogger(string categoryName)
        {
            return new AppLogger(categoryName, _loggingService, _permissionService);
        }
    }
}
