using System;
using System.Linq;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Services.Communication;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class SugarBeatGlucose : SugarBeatCharacteristicPayload
    {

        //Date Time	0	Uint32
        //Battery Voltage mV	4	Uint16
        //Active Alarm Bitmaps	6	Uint32
        //Glucose	10	Float32	‘Area under the curve’ averaged current  over time.
        //Counter Electrode Voltage	14	Uint16 mV
        //Software Version	16	Uint16 Major – 4 bits (0-15)
        //Minor – 6 bits(0-63 
        //Test – 6 bits (0-63)


        //Byte 16  - 00010000  
        //Byte 17 -  11000111 

        //Software Version 
        //1.3.7 

        //CRC	18	Uint8

        //This will be called when the glucose is received from history request therefore base parameter is 17
        public SugarBeatGlucose(DateTime dateTime, ushort battery, uint activeAlarmBitmaps, double glucose, byte[] bytes) : base(bytes, 17)
        {
            DateTime = dateTime;
            Battery = battery;
            ActiveAlarmBitmaps = activeAlarmBitmaps;
            Glucose = glucose;
        }

        public SugarBeatGlucose(byte[] bytes) : base(bytes, 19)
        {
            if (bytes.Length == 19)
            {

                DateTime = SugarBeatCharacteristicService.GetDateTime(new[]
                {
                Payload.ElementAt(0),
                Payload.ElementAt(1),
                Payload.ElementAt(2),
                Payload.ElementAt(3)
            });

                Battery = SugarBeatCharacteristicService.GetUInt16(new[]
                {
                Payload.ElementAt(4),
                Payload.ElementAt(5)
            });

                ActiveAlarmBitmaps = SugarBeatCharacteristicService.GetUInt32(new[]
                {
                Payload.ElementAt(6),
                Payload.ElementAt(7),
                Payload.ElementAt(8),
                Payload.ElementAt(9)
            });

                Glucose = SugarBeatCharacteristicService.GetDouble(new[]
                {
                Payload.ElementAt(10),
                Payload.ElementAt(11),
                Payload.ElementAt(12),
                Payload.ElementAt(13)
            });

                CounterElectrodeVoltage = SugarBeatCharacteristicService.GetUInt16(new[]
                {
                Payload.ElementAt(14),
                Payload.ElementAt(15)
            });
            }
            else if (bytes.Length == 17)
            {
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


            var softwareVersionRaw = SugarBeatCharacteristicService.GetUInt16(new[]
            {
                Payload.ElementAt(16),
                Payload.ElementAt(17)
            });
            var softwareVersionRawReversedBytes = SugarBeatCharacteristicService.GetUInt16(new[]
            {
                Payload.ElementAt(17),
                Payload.ElementAt(16)
            });

            int major = (softwareVersionRaw & 0xF0) >> 4; // first 4 bits of the 1st byte
            int minor = (softwareVersionRawReversedBytes >> 6) & 0x3F; // last 4 bits of 1st byte and first 2 bits of 2nd byte
            int test = ((softwareVersionRaw >> 8) & 0x3F);  // last 6 bits of 2nd byte

            SoftwareVersion = major.ToString() + '.' + minor.ToString() + '.' + test.ToString();
        }

        public DateTime DateTime { get; }
        public ushort Battery { get; }
        public uint ActiveAlarmBitmaps { get; }
        public double Glucose { get; }
        //in mV
        public ushort CounterElectrodeVoltage { get; }
        // Major – 4 bits(0-15)
        //Minor – 6 bits(0-63 
        //Test – 6 bits (0-63)


        //Byte 16  - 00010000  
        //Byte 17 -  11000111 

        //Software Version 
        //1.3.7 
        public string SoftwareVersion { get; }
    }
}
