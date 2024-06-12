using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using ZeroGravity.Mobile.Interfaces.Communication;
using AndroidX.Core.App;
using AndroidApp = Android.App.Application;

namespace ZeroGravity.Mobile.Droid.Services
{
    public class AndroidLocalNotificationsService : ILocalNotificationsService
    {
        private const string ChannelId = "miboko";
        private const string ChannelName = "MiBoKo";
        private const string ChannelDescription = "The Miboko channel for notifications.";

        public const string TitleKey = "ZgPushTitle";
        public const string MessageKey = "ZgPushMessage";
        public const string CriticalKey = "ZgPushCritical";

        private bool _channelInitialized;
        private int _messageId;
        private int _pendingIntentId;

        private NotificationManager _manager;

        public AndroidLocalNotificationsService()
        {
            CreateNotificationChannel();
        }

        public void SendNotification(string title, string message, bool critical = false)
        {
            if (!_channelInitialized)
            {
                return;
            }

            Show(title, message, critical);
        }

        public void Show(string title, string message, bool critical)
        {
            var intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);
            intent.PutExtra(CriticalKey, critical);
            
            _pendingIntentId++;
            var pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, _pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);
            var category = critical ? Notification.CategoryAlarm : Notification.CategoryReminder;

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, ChannelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Mipmap.icon)) //ignore error, prism template issue
                .SetSmallIcon(Resource.Mipmap.icon) //ignore error, prism template issue
                .SetDefaults((int)NotificationDefaults.All)
                .SetPriority((int)NotificationPriority.Max)
                .SetCategory(category);

            var notification = builder.Build();
            _messageId++;
            _manager.Notify(_messageId, notification);
        }

        private void CreateNotificationChannel()
        {
            _manager = (NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O && _manager != null)
            {
                var channelNameJava = new Java.Lang.String(ChannelName);
                var channel = new NotificationChannel(ChannelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = ChannelDescription
                };
                _manager.CreateNotificationChannel(channel);
            }
            
            _channelInitialized = true;
        }
    }
}