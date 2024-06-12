using System;
using System.Linq;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Services.Communication;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class BulkGlucoseHistoryResponseHeader : SugarBeatCharacteristicPayload
    {

        //Date Time	0	Uint32 Should match Oldest Date Time set on Bulk Request Characteristic
        //Number of readings	4	Uint16 Total reading available since date time
        //CRC	6	Uint8

        public BulkGlucoseHistoryResponseHeader(byte[] bytes) : base(bytes, 11)
        {
            if (bytes.Length == 11)
            {

                DateTime = SugarBeatCharacteristicService.GetDateTime(new[]
               {
                Payload.ElementAt(0),
                Payload.ElementAt(1),
                Payload.ElementAt(2),
                Payload.ElementAt(3)
            });
                ReadCount = SugarBeatCharacteristicService.GetUInt16(new[]
               {
                Payload.ElementAt(4),
                Payload.ElementAt(5),
            });
            }
        }

        public DateTime DateTime { get; }
        public uint ReadCount { get; }
    }
}
