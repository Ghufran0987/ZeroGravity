using System;
using System.Collections.Generic;
using Plugin.BLE.Abstractions.Contracts;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class SugarBeatDevice
    {
        public static Guid ServiceId = Guid.Parse("5F860706-D5E1-41C7-B490-99279F557D36");
        public static Guid PasskeyAuthenticationRequestCharacteristicId = Guid.Parse("5F860413-D5E1-41C7-B490-99279F557D36");

        public static Guid PasskeyAuthenticationResponseCharacteristicId =
            Guid.Parse("5F860511-D5E1-41C7-B490-99279F557D36");

        public static Guid AlertCharacteristicId = Guid.Parse("5F860412-D5E1-41C7-B490-99279F557D36");
        public static Guid CurrentGlucoseCharacteristicId = Guid.Parse("5F860411-D5E1-41C7-B490-99279F557D36");
        public static Guid HistoryRequestCharacteristicId = Guid.Parse("5F860414-D5E1-41C7-B490-99279F557D36");
        public static Guid HistoryResponseHeaderCharacteristicId = Guid.Parse("5F860715-D5E1-41C7-B490-99279F557D36");
        public static Guid HistoryResponseBodyCharacteristicId = Guid.Parse("5F860408-D5E1-41C7-B490-99279F557D36");
        public static Guid SyncTimeCharacteristicsId = Guid.Parse("5F860415-D5E1-41C7-B490-99279F557D36");

        public SugarBeatDevice(Guid id, string name, string machId, int rssi)
        {
            Id = id;
            Name = name;
            MacAddress = machId;
            Rssi = rssi;
        }

        public SugarBeatDevice(Guid id, string name, int rssi)
        {
            Id = id;
            Name = name;
            Rssi = rssi;
        }

        public Dictionary<CharacteristicType, ICharacteristic> Characteristics { get; } =
            new Dictionary<CharacteristicType, ICharacteristic>();

        public string PasskeyAuthentication { get; set; }
        public IDevice PluginBleDevice { get; set; }
        public string Name { get; }
        public Guid Id { get; }
        public int Rssi { get; }
      

        public string MacAddress { get; }
        public bool DeviceFound => PluginBleDevice != null;

        public void ClearCharacteristic()
        {
            Characteristics.Clear();
        }

        public void AddCharacteristic(CharacteristicType type, ICharacteristic characteristic)
        {
            if (!Characteristics.ContainsKey(type))
            {
                Characteristics.Add(type, characteristic);
            }
        }

        public bool HasCharacteristic(CharacteristicType type)
        {
            return Characteristics.ContainsKey(type);
        }
    }

    public enum CharacteristicType
    {
        PasskeyAuthenticationRequest,
        PasskeyAuthenticationResponse,
        Alert,
        CurrentGlucose,
        BulkGlucoseHistoryRequest,
        BulkGlucoseHistoryResponseHeader,
        BulkGlucoseHistoryResponseBody,
        TimeSynchronization
    }
}