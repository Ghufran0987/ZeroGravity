using Microsoft.Extensions.Logging;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.Logging
{
    /// <summary>
    /// Logger factory to create a new instance of an <see cref="AppLoggerProvider"/>
    /// to create an appropriate <see cref="ILogger"/> type instance 
    /// </summary>
    public class AppLoggerFactory : ILoggerFactory
    {
        private readonly AppLoggerProvider _appLoggerProvider;

        public AppLoggerFactory(ILoggingService loggingService, IPermissionService permissionService)
        {
            _appLoggerProvider = new AppLoggerProvider(loggingService, permissionService);
        }


        public ILogger CreateLogger(string categoryName)
        {
            return _appLoggerProvider.CreateLogger(categoryName);
        }

        public void AddProvider(ILoggerProvider provider)
        {
        }

        public void Dispose()
        {
            _appLoggerProvider.Dispose();
        }
    }
}
