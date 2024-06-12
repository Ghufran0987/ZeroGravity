using System;
using System.Linq;

namespace ZeroGravity.Mobile.Services.Communication
{
    public static class SugarBeatCharacteristicService
    {
        public static byte[] GetDateTimeBytes(DateTime dateTime)
        {
            var defaultDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            var timeSpan = dateTime - defaultDateTime;
            var second = (int) timeSpan.TotalSeconds;
            var bytes = BitConverter.GetBytes(second);
            return bytes;
        }

        public static DateTime GetDateTime(byte[] value)
        {
            if (value.Length != 4)
            {
                throw new ArgumentOutOfRangeException();
            }

            var dateTimeInt32 = GetUInt32(value);
            var defaultDateTime = new DateTime(1970, 1, 1, 0, 0, 0);
            defaultDateTime = defaultDateTime.AddMilliseconds(dateTimeInt32 * 1000L);
            return defaultDateTime;
        }

        public static double GetDouble(byte[] value)
        {
            if (value.Length != 4)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(value);

            }
            
            return BitConverter.ToSingle(value, 0);
        }

        public static uint GetUInt32(byte[] value)
        {
            if (value.Length != 4)
            {
                throw new ArgumentOutOfRangeException();
            }

            return BitConverter.ToUInt32(value, 0);
        }

        public static ushort GetUInt16(byte[] value)
        {
            if (value.Length != 2)
            {
                throw new ArgumentOutOfRangeException();
            }

            return BitConverter.ToUInt16(value, 0);
        }

        public static byte[] GetPasskeyPayload(string passKey)
        {
            if (passKey.Length != 4)
            {
                return default;
            }

            var resultBytes = new byte[20];
            var random = new Random();
            random.NextBytes(resultBytes);

            resultBytes[0] = Convert.ToByte(passKey.ElementAt(0));
            resultBytes[1] = Convert.ToByte(passKey.ElementAt(1));
            resultBytes[2] = Convert.ToByte(passKey.ElementAt(2));
            resultBytes[3] = Convert.ToByte(passKey.ElementAt(3));
            resultBytes[4] = 0x01;


            resultBytes[0] = (byte) (resultBytes[5] ^ resultBytes[14] ^ 0xFF ^ resultBytes[0]);
            resultBytes[1] = (byte) (resultBytes[7] ^ resultBytes[11] ^ 0xFF ^ resultBytes[1]);
            resultBytes[2] = (byte) (resultBytes[9] ^ resultBytes[13] ^ 0xFF ^ resultBytes[2]);
            resultBytes[3] = (byte) (resultBytes[8] ^ resultBytes[12] ^ 0xFF ^ resultBytes[3]);
            resultBytes[4] = (byte) (resultBytes[6] ^ resultBytes[10] ^ 0xFF ^ resultBytes[4]);

            var bytesForCrc = new byte[19];
            Array.Copy(resultBytes, bytesForCrc, 19);
            resultBytes[19] = GetOneByteCrc(bytesForCrc);
            return resultBytes;
        }

        public static byte GetOneByteCrc(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0)
            {
                return 0;
            }

            byte crc = 0;

            foreach (var b in bytes)
            {
                crc += b;
            }

            return (byte) (0xff - crc);
        }
    }
}