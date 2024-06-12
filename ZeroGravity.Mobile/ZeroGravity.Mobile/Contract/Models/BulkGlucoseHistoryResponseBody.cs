using System;
using System.Linq;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Services.Communication;

namespace ZeroGravity.Mobile.Contract.Models
{

    // Index	0	Uint16
    //Date Time	2	Uint32
    //Battery Voltage mV  6	Uint16
    //Active Alarm Bitmaps    8	Uint32
    //Glucose Accumulated	12	Float32
    //CRC 16	Uint8

    public class BulkGlucoseHistoryResponseBody : SugarBeatCharacteristicPayload
    {
        public BulkGlucoseHistoryResponseBody(byte[] bytes) : base(bytes, 20)
        {
            if (bytes.Length != 20)
                return;
            Index = SugarBeatCharacteristicService.GetUInt16(new[]
           {
                Payload.ElementAt(0),
                Payload.ElementAt(1)
            });
            DateTime = SugarBeatCharacteristicService.GetDateTime(new[]
          {
                Payload.ElementAt(2),
                Payload.ElementAt(3),
                Payload.ElementAt(4),
                Payload.ElementAt(5)
            });
            Battery = SugarBeatCharacteristicService.GetUInt16(new[]
           {
                Payload.ElementAt(6),
                Payload.ElementAt(7)
            });

            ActiveAlarmBitmaps = SugarBeatCharacteristicService.GetUInt32(new[]
           {
                Payload.ElementAt(8),
                Payload.ElementAt(9),
                Payload.ElementAt(10),
                Payload.ElementAt(11)
            });



            Glucose = SugarBeatCharacteristicService.GetDouble(new[]
            {
                Payload.ElementAt(12),
                Payload.ElementAt(13),
                Payload.ElementAt(14),
                Payload.ElementAt(15)
            });


        }
        public uint Index { get; }
        public DateTime DateTime { get; }
        public ushort Battery { get; }
        public uint ActiveAlarmBitmaps { get; }
        public double Glucose { get; }

    }
}