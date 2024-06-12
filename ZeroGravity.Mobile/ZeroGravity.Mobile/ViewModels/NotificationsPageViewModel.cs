using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Navigation;
using Xamarin.Forms;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Base.Interfaces;
using ZeroGravity.Mobile.Contract.Models.Notification;
using ZeroGravity.Mobile.Events;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Mobile.Interfaces.Page;
using ZeroGravity.Mobile.Resx;

namespace ZeroGravity.Mobile.ViewModels
{
    public class NotificationsPageViewModel : VmBase<INotificationPage, INotificationPageVmProvider, NotificationsPageViewModel>
    {
        private readonly IEventAggregator _eventAggregator;
        public DelegateCommand AddCommand { get; }
        public DelegateCommand RemoveCommand { get; }


        private ObservableCollection<Notification> _notifications;
        private CancellationTokenSource _cts;




        public NotificationsPageViewModel(IVmCommonService service, INotificationPageVmProvider provider, ILoggerFactory loggerFactory, 
            IEventAggregator eventAggregator) : base(service, provider, loggerFactory)
        {
            _eventAggregator = eventAggregator;

            AddCommand = new DelegateCommand(AddExecute);
            RemoveCommand = new DelegateCommand(RemoveExecute);
        }

        private async Task<List<Notification>> GetNotifications()
        {
            return await Provider.GetNotificationsAsync(_cts.Token);
        }

        private void SetNotification(List<Notification> notifications)
        {
            Notifications = new ObservableCollection<Notification>(notifications);
        }

        private async void AddExecute()
        {
            var count = Notifications.Count + 1;

            var content = "";

            for (int i = 0; i <= count - 1; i++)
            {
                content = content + $"Content-{count}\n";
            }

            Provider.AddNotification($"Title-{count}", content);

            SetNotification(await GetNotifications());
        }

        private async void RemoveExecute()
        {
            var count = Notifications.Count;

            if (Notifications.Any())
            {
                var lastNotification = Notifications[count - 1];

                Provider.RemoveNotification(lastNotification.Id);

                SetNotification(await GetNotifications());
            }
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            base.OnNavigatedTo(parameters);

            _cts = new CancellationTokenSource();

            Title = AppResources.Notifications_Title;

            return;

            var notifications = await Provider.GetNotificationsAsync(_cts.Token);
            Notifications = new ObservableCollection<Notification>(notifications);

            MarkAllAsRead();
        }

        private void MarkAllAsRead()
        {
            foreach (var notification in Notifications)
            {
                notification.HasBeenRead = true;
            }
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);

            CancelPendingRequest(_cts);
            _eventAggregator.GetEvent<NotificationsReadEvent>().Publish();
        }


        public ObservableCollection<Notification> Notifications
        {
            get { return _notifications; }
            set { SetProperty(ref _notifications, value); }
        }

    }
}
