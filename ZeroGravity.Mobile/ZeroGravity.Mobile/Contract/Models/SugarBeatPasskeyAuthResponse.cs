using System;
using System.Linq;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Services.Communication;

namespace ZeroGravity.Mobile.Contract.Models
{
    public  class SugarBeatPasskeyAuthResponse : SugarBeatCharacteristicPayload
    {

        // Authentication Status - offset: 0, type: UINT8, 1 = success, 0 = fail
        // CRC - offset: 1, type: UINT8

        public SugarBeatPasskeyAuthResponse(byte[] bytes) : base(bytes, 2)
        {
            if (bytes.Length == 2)
            {
                // First check the CRC in the payload is correct
                var expectedCRC = SugarBeatCharacteristicService.GetOneByteCrc(new[] { Payload.ElementAt(0) });
                if (bytes[1] != expectedCRC)
                {
                    // Throw an exception if the CRC validation fails
                    throw new ArgumentException("SugarBeatPasskeyAuthResponse CRC mismatch, received: " + bytes[1] + ", expected: " + expectedCRC);
                }

                // If the CRC matches then read the payload
                authenticationStatus = Convert.ToBoolean(bytes[0]);
            }
        }

        public bool authenticationStatus { get; }
    }

}
