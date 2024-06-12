namespace ZeroGravity.Mobile.Interfaces.Communication
{
    public interface ILocalNotificationsService
    {
        void SendNotification(string title, string message, bool critical = false);
    }
}
