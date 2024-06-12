using System;
using System.Linq;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Services.Communication;

namespace ZeroGravity.Mobile.Contract.Models
{
    // Oldest Date Time	0	Uint32
    //Index offset	4	Uint16
    //CRC 6	Uint8

    public class BulkGlucoseHistoryRequest: SugarBeatCharacteristicPayload
    {
        public BulkGlucoseHistoryRequest(byte[] bytes) : base(bytes, 7)
        {

            if (bytes.Length != 7)
                return;
            DateTime = SugarBeatCharacteristicService.GetDateTime(new[]
        {
                Payload.ElementAt(0),
                Payload.ElementAt(1),
                Payload.ElementAt(2),
                Payload.ElementAt(3)
            });
            Index = SugarBeatCharacteristicService.GetUInt16(new[]
           {
                Payload.ElementAt(4),
                Payload.ElementAt(5)
            });

        }

        public DateTime DateTime { get; }
        public ushort Index { get; }
    }
}