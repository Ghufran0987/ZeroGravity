using System;
using ZeroGravity.Mobile.Services.Communication;

namespace ZeroGravity.Mobile.Base
{
    public abstract class SugarBeatCharacteristicPayload
    {
        protected SugarBeatCharacteristicPayload(byte[] bytes, int length)
        {
            if (bytes.Length < length)
            {
                //  throw new ArgumentOutOfRangeException("length of bytes");
                return;
            }

            var countBytesForCrc = length - 1;

            var bytesForCrc = new byte[countBytesForCrc];

            Array.Copy(bytes, bytesForCrc, countBytesForCrc);

            var crcByte = SugarBeatCharacteristicService.GetOneByteCrc(bytesForCrc);
            if (crcByte != bytes[countBytesForCrc])
            {
                //throw new ArgumentException("CRC of bytes");
            }

            Payload = bytes;
        }

        public byte[] Payload { get; }
    }
}