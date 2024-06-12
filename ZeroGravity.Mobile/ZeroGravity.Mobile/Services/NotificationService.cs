using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ZeroGravity.Mobile.Contract.Models.Notification;
using ZeroGravity.Mobile.Interfaces;

namespace ZeroGravity.Mobile.Services
{
    public class NotificationService : INotificationService
    {
        private List<Notification> _notifications;

        public NotificationService()
        {
            _notifications = new List<Notification>();
            _notifications = GetDummyNotifications();
        }

        public async Task<List<Notification>> GetNotificationsAsync(CancellationToken caneCancellationToken)
        {
            await Task.CompletedTask;
            return _notifications;
        }

        public void AddNotification(string title, string content)
        {
            _notifications.Add(new Notification{Title = title, Content = content});
        }

        public void RemoveNotification(Guid id)
        {
            if (_notifications.Exists(n => n.Id == id))
            {
                var notification = _notifications.Find(n => n.Id == id);
                _notifications.Remove(notification);
            }
        }

        private List<Notification> GetDummyNotifications()
        {
            var dummys = new List<Notification>();

            dummys.Add(new Notification { Title = "Title-1", Content = "Content-1\n" });
            dummys.Add(new Notification { Title = "Title-2", Content = "Content-2\nContent-2\n" });

            return dummys;
        }


    }
}
