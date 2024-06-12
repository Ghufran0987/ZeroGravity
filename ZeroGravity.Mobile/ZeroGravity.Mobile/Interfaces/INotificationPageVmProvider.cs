using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models.Notification;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface INotificationPageVmProvider : IPageVmProvider
    {
        Task<List<Notification>> GetNotificationsAsync(CancellationToken caneCancellationToken);
        void AddNotification(string title, string content);
        void RemoveNotification(Guid id);
    }
}
