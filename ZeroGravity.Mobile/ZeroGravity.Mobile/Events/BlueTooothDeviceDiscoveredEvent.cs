using Plugin.BLE.Abstractions.Contracts;
using Prism.Events;
using System;
using ZeroGravity.Mobile.Contract.Models;

namespace ZeroGravity.Mobile.Events
{
  public  class BlueTooothDeviceDiscoveredEvent: PubSubEvent<SugarBeatDevice>
    {
    }

  public  class ScanForAvailableDevicesCompleted : PubSubEvent
    {
    }

    public class DeviceConnectionFailedEvent : PubSubEvent
    {
    }

    public class PassKeyAuthFailedEvent : PubSubEvent
    {
    }
}
