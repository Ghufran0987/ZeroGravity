using Prism.Events;

namespace ZeroGravity.Mobile.Events
{
    public class SugarBeatShowAlertEvent : PubSubEvent<string>
    {
    }

    public class SugarBeatDeviceLostAlertEvent : PubSubEvent<string>
    {
    }

    public class PatchConnectedEvent : PubSubEvent<bool>
    {
    }

 public class BleDeviceDisconnectedStatusEvent : PubSubEvent
    {
    }

 public class DeviceLostInMainPage : PubSubEvent
    {
    }

}