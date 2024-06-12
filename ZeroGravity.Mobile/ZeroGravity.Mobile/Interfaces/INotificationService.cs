using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models.Notification;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface INotificationService
    {
        Task<List<Notification>> GetNotificationsAsync(CancellationToken caneCancellationToken);
        void AddNotification(string title, string content);
        void RemoveNotification(Guid id);
    }
}
