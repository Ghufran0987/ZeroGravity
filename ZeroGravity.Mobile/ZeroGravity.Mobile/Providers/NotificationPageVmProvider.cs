using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using ZeroGravity.Mobile.Base.Provider;
using ZeroGravity.Mobile.Contract.Models.Notification;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.Providers
{
    public class NotificationPageVmProvider : PageVmProviderBase, INotificationPageVmProvider
    {
        private readonly INotificationService _notificationService;
        private readonly ILogger _logger;

        public NotificationPageVmProvider(ITokenService tokenService, ILoggerFactory loggerFactory, 
            INotificationService notificationService) : base(tokenService)
        {
            _notificationService = notificationService;
            _logger = loggerFactory?.CreateLogger<NotificationPageVmProvider>() ?? new NullLogger<NotificationPageVmProvider>();
        }

        public async Task<List<Notification>> GetNotificationsAsync(CancellationToken caneCancellationToken)
        {
            return await _notificationService.GetNotificationsAsync(caneCancellationToken);
        }

        public void AddNotification(string title, string content)
        {
            _notificationService.AddNotification(title, content);
        }

        public void RemoveNotification(Guid id)
        {
            _notificationService.RemoveNotification(id);
        }
    }
}
