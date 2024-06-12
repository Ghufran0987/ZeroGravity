using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Xamarin.Essentials;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.Logging
{
    public class AppLogger : ILogger, IDisposable
    {
        private readonly string _categoryName;
        private readonly ILoggingService _loggingService;
        private readonly IPermissionService _permissionService;

        public AppLogger(string categoryName, ILoggingService loggingService, IPermissionService permissionService)
        {
            _permissionService = permissionService;
            _categoryName = categoryName;
            _loggingService = loggingService;
        }



        public async void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var s = formatter(state, exception);


            var textToLog = $"[ZeroG] [{DateTime.Now:dd.MM.yyyy HH.mm.ss}] [{logLevel}] [{_categoryName}] {s}{Environment.NewLine}";

#if DEBUG
            // log to console while in development
            Debug.WriteLine(textToLog);
#endif


#if DEBUG
            // only log to device storage while developing/debugging

            await _permissionService.CheckAndRequestPermissionAsync(new Permissions.StorageWrite());
            
#endif

            _loggingService.Log(textToLog);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return true;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return this;
        }

        public void Dispose()
        {
        }
    }
}
