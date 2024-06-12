using System;
using Prism.Events;

namespace ZeroGravity.Mobile.Events
{
    public class BleDeviceDisconnectedEvent : PubSubEvent<Guid>
    {
    }
}