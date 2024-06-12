using System;
using System.Diagnostics;
using UserNotifications;
using ZeroGravity.Mobile.Interfaces.Communication;

namespace ZeroGravity.Mobile.iOS.Services
{
    public class IosLocalNotificationsService : ILocalNotificationsService
    {
        private int _messageId;
        private bool _hasNotificationsPermission;
        private bool _hasCriticalNotificationsPermission;

        public IosLocalNotificationsService()
        {
            Initialize();
        }

        public void SendNotification(string title, string message, bool critical = false)
        {
            // EARLY OUT: app doesn't have permissions
            if (!_hasNotificationsPermission)
            {
                return;
            }

            if (critical && !_hasCriticalNotificationsPermission)
            {
                return;
            }

            var trigger = UNTimeIntervalNotificationTrigger.CreateTrigger(0.25, false); //timeoutInterval has to be greater 0

            _messageId++;
            var requestId = _messageId.ToString();
            var sound = critical ? UNNotificationSound.DefaultCriticalSound : UNNotificationSound.Default;
            
            var content = new UNMutableNotificationContent
            {
                Title = title,
                Subtitle = "",
                Body = message,
                Badge = 1,
                Sound = sound,
            };

            var request = UNNotificationRequest.FromIdentifier(requestId, content, trigger);
            
            UNUserNotificationCenter.Current.AddNotificationRequest(request, (err) =>
            {
                if (err != null)
                {
                   Debug.WriteLine($"Failed to schedule notification: {err}");
                }
            });
        }

        private void Initialize()
        {
            // request the permission to use local notifications
            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert, (approved, err) =>
            {
                _hasNotificationsPermission = approved;
            });

            UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.CriticalAlert, (approved, err) =>
            {
                _hasCriticalNotificationsPermission = approved;
            });
        }
    }

    public class IosNotificationReceiver : UNUserNotificationCenterDelegate
    {
        public override void WillPresentNotification(
            UNUserNotificationCenter center, 
            UNNotification notification, 
            Action<UNNotificationPresentationOptions> completionHandler)
        {
            completionHandler(UNNotificationPresentationOptions.Alert);
        }
    }
}