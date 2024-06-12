using System;
using Prism.Events;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Events
{
    public class BleDeviceConnectedEvent : PubSubEvent<Guid>
    {
    }

    public class BleDeviceReConnectedStatusEvent : PubSubEvent<bool>
    {
    }
    public class BleDeviceReConnectedEvent : PubSubEvent<SugarBeatDevice>
    {
    }
    public class HistorySyncStatusEvent : PubSubEvent<bool>
    {
    }

    public class HistorySyncStartedEvent : PubSubEvent<bool>
    {
    }
    public class HistorySyncExceptionEvent : PubSubEvent
    {

    }
}