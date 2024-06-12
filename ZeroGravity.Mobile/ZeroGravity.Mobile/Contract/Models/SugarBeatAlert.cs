using System;
using System.Collections.Generic;
using System.Linq;
using ZeroGravity.Mobile.Base;
using ZeroGravity.Mobile.Services.Communication;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Models
{
    public class SugarBeatAlert : SugarBeatCharacteristicPayload
    {
        //Current Date Time	0-3	Uint32
        //Active Alarm Bitmap	4-7	Uint32 Bitmap
        //Critical Alarm Code	8-11	Uint32 Software Version has been added to this field.

        //8-9 – Critical error code.This is an enum.  Not a bitmap like active alerts.There can be only one critical error at a time.

        //10-11 Software version.  Three sw version fields split over two bytes as follows

        //Major – 4 bits (0-15)
        //Minor – 6 bits(0-63)
        //Test – 6 bits (0-63)

        //Byte 10  - 00010000 
        //Byte 11 -  11000111

        //Software Version
        //1.3.7

        //CRC	12	Uint8


        //History response of alert 

        //Index  0 Uint16

        //Date Time 2 Uint32

        //Battery Voltage mV 6 Uint16

        //Active Alarm Bitmaps 8 Uint32

        //Glucose Accumulated 12 Float32

        //CRC 16 Uint8



        public SugarBeatAlert(DateTime dateTime, ushort battery, uint activeAlarmBitmaps, double glucose, byte[] bytes) : base(bytes, 17)
        {
            DateTime = dateTime;
            BatteryVoltage = battery;
            ActiveAlarmBitmap = activeAlarmBitmaps;
            AccumulatedGlucose = glucose;
            getAlertCodes();
        }

        public SugarBeatAlert(byte[] bytes) : base(bytes, 17)
        {
            if (bytes.Length != 17)
            {
                return;
            }
            DateTime = SugarBeatCharacteristicService.GetDateTime(new[]
            {
                Payload.ElementAt(0),
                Payload.ElementAt(1),
                Payload.ElementAt(2),
                Payload.ElementAt(3)
            });

            ActiveAlarmBitmap = SugarBeatCharacteristicService.GetUInt32(new[]
            {
                Payload.ElementAt(4),
                Payload.ElementAt(5),
                Payload.ElementAt(6),
                Payload.ElementAt(7)
            });

            getAlertCodes();

            CriticalAlarmCode = SugarBeatCharacteristicService.GetUInt32(new[]
            {
                Payload.ElementAt(8),
                Payload.ElementAt(9),
                Payload.ElementAt(10),
                Payload.ElementAt(11),
            });


            BatteryVoltage = SugarBeatCharacteristicService.GetUInt16(new[]
            {
                Payload.ElementAt(12),
                Payload.ElementAt(13)
            });

            ParseSoftwareVersion(new[]
            {
                Payload.ElementAt(10),
                Payload.ElementAt(11)
            });

          
            SetCRACode();
        }

        private void ParseSoftwareVersion(byte[] VersionPayload)
        {
            short softwareVersionRaw = BitConverter.ToInt16(VersionPayload, 0);
            short softwareVersionRawReversed = BitConverter.ToInt16(new[] { VersionPayload[1], VersionPayload[0] }, 0);

            int major = (softwareVersionRaw & 0xF0) >> 4; // first 4 bits of the 1st byte
            int minor = (softwareVersionRawReversed >> 6) & 0x3F; // last 4 bits of 1st byte and first 2 bits of 2nd byte
            int test = ((softwareVersionRaw >> 8) & 0x3F);  // last 6 bits of 2nd byte

            SoftwareVersion = major.ToString() + '.' + minor.ToString() + '.' + test.ToString();

        }

        private List<AlertCode> alerts;
        public List<AlertCode> Alerts
        {
            get
            {
                if (alerts == null)
                {
                    alerts = new List<AlertCode>();
                }
                return alerts;
            }
        }
        public DateTime DateTime { get; }
        public uint ActiveAlarmBitmap { get; }

        private AlertCode AlertPayload;
        public uint CriticalAlarmCode { get; }
        public double AccumulatedGlucose { get; }

        public uint BatteryVoltage { get; }

        public AlertCode AlertCode { get; set; }
        public CRCCodes CRCCode { get; set; }
        public string SoftwareVersion { get; set; }

        private void SetCRACode()
        {
            switch (CriticalAlarmCode)
            {
                case 0:
                    CRCCode = CRCCodes.CRIT_ERROR_NO_ERROR;
                    break;
                case 1:
                    CRCCode = CRCCodes.CRIT_ERROR_CRC_FAILURE; break;
                case 2:
                    CRCCode = CRCCodes.CRIT_ERROR_NO_PRIVILEGES; break;
                case 3:
                    CRCCode = CRCCodes.CRIT_ERROR_WRONG_SIZE; break;
                case 4:
                    CRCCode = CRCCodes.CRIT_ERROR_NULL_PARAMETER; break;
                case 5:
                    CRCCode = CRCCodes.CRIT_ERROR_MISSING_POINTER; break;
                case 6:
                    CRCCode = CRCCodes.CRIT_ERROR_ARRAY_OUT_OF_BOUNDS; break;
                case 7:
                    CRCCode = CRCCodes.CRIT_ERROR_LIMIT_UNDEFINED; break;
                case 8:
                    CRCCode = CRCCodes.CRIT_ERROR_WRONG_ARRAY_SIZE; break;
                case 9:
                    CRCCode = CRCCodes.CRIT_ERROR_INVALID_TYPE; break;
                case 10:
                    CRCCode = CRCCodes.CRIT_ERROR_QUEUE_EMPTY; break;
                case 11:
                    CRCCode = CRCCodes.CRIT_ERROR_QUEUE_FULL; break;
                case 12:
                    CRCCode = CRCCodes.CRIT_ERROR_NO_RESOURCE; break;
                case 13:
                    CRCCode = CRCCodes.CRIT_ERROR_INVALID_INDEX; break;
                case 14:
                    CRCCode = CRCCodes.CRIT_ERROR_INVALID_STATE; break;
                case 15:
                    CRCCode = CRCCodes.CRIT_ERROR_INVALID_TRANSITION; break;
                case 16:
                    CRCCode = CRCCodes.CRIT_ERROR_SM_STACK_FULL; break;
                case 17:
                    CRCCode = CRCCodes.CRIT_ERROR_SM_STACK_EMPTY; break;
                case 18:
                    CRCCode = CRCCodes.CRIT_ERROR_MISSING_TRANSITION_TABLE; break;
                case 19:
                    CRCCode = CRCCodes.CRIT_ERROR_ASSERT_FAILURE; break;
                case 20:
                    CRCCode = CRCCodes.CRIT_ERROR_HARDFAULT; break;
                case 21:
                    CRCCode = CRCCodes.CRIT_ERROR_MEM_MANAGE; break;
                case 22:
                    CRCCode = CRCCodes.CRIT_ERROR_BUSFAULT; break;
                case 23:
                    CRCCode = CRCCodes.CRIT_ERROR_USAGEFAULT; break;
                case 24:
                    CRCCode = CRCCodes.CRIT_ERROR_SVC_CALL; break;
                case 25:
                    CRCCode = CRCCodes.CRIT_ERROR_DEBUGMON; break;
                case 26:
                    CRCCode = CRCCodes.CRIT_ERROR_PEND_SVC; break;
                case 27:
                    CRCCode = CRCCodes.CRIT_ERROR_INITIALISATION_FAULT; break;
                case 28:
                    CRCCode = CRCCodes.CRIT_ERROR_BLE_HAL; break;
                case 29:
                    CRCCode = CRCCodes.CRIT_ERROR_SELF_TEST;
                    break;
            }
        }

        private void getAlertCodes()
        {
            AlertCode code = (AlertCode)ActiveAlarmBitmap;
            Alerts.Clear();
            if (code.HasFlag(AlertCode.ALERT_NO_ALERT))
            {
                AlertCode = AlertCode.ALERT_NO_ALERT;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_BLE_ERROR))
            {
                AlertCode = AlertCode.ALERT_BLE_ERROR;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_FAULT_DETECTED_IN_THE_LOGGING_MEMORY))
            {
                AlertCode = AlertCode.ALERT_FAULT_DETECTED_IN_THE_LOGGING_MEMORY;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_BATTERY_EXHAUSTED))
            {
                AlertCode = AlertCode.ALERT_BATTERY_EXHAUSTED;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_POWER_DOWN_SEQUENCE_ACTIVATED))
            {
                AlertCode = AlertCode.ALERT_POWER_DOWN_SEQUENCE_ACTIVATED;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_INVALID_BLE_PARAMETERS))
            {
                AlertCode = AlertCode.ALERT_INVALID_BLE_PARAMETERS;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_GLUCOSE_INTEGRITY_TEST_FAILED))
            {
                AlertCode = AlertCode.ALERT_GLUCOSE_INTEGRITY_TEST_FAILED;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_GLUCOSE_SKIPPED_MEASUREMENT))
            {
                AlertCode = AlertCode.ALERT_GLUCOSE_SKIPPED_MEASUREMENT;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_GLUCOSE_RESULT_OUT_OF_RANGE))
            {
                AlertCode = AlertCode.ALERT_GLUCOSE_RESULT_OUT_OF_RANGE;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_TEMPERATURE_OUT_OF_RANGE))
            {
                AlertCode = AlertCode.ALERT_TEMPERATURE_OUT_OF_RANGE;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_SELF_TEST_FAILED_ADC))
            {
                AlertCode = AlertCode.ALERT_SELF_TEST_FAILED_ADC;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_SELF_TEST_FAILED_TEMP))
            {
                AlertCode = AlertCode.ALERT_SELF_TEST_FAILED_TEMP;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_SELF_TEST_FAILED_BATT))
            {
                AlertCode = AlertCode.ALERT_SELF_TEST_FAILED_BATT;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_SELF_TEST_FAILED_CLK))
            {
                AlertCode = AlertCode.ALERT_SELF_TEST_FAILED_CLK;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_SELF_TEST_FAILED_MEM))
            {
                AlertCode = AlertCode.ALERT_SELF_TEST_FAILED_MEM;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_PATCH_CONNECTED))
            {
                AlertCode = AlertCode.ALERT_PATCH_CONNECTED;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_BATTERY_STATUS))
            {
                AlertCode = AlertCode.ALERT_BATTERY_STATUS;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_INITIALISATION_FAILURE))
            {
                AlertCode = AlertCode.ALERT_INITIALISATION_FAILURE;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_INVALID_PARAMETERS))
            {
                AlertCode = AlertCode.ALERT_INVALID_PARAMETERS;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_TRANSMITTER_STARTUP))
            {
                AlertCode = AlertCode.ALERT_TRANSMITTER_STARTUP;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_SELF_TEST_FAILED_WDOG))
            {
                AlertCode = AlertCode.ALERT_SELF_TEST_FAILED_WDOG;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_SELF_TEST_FAILED_BLE))
            {
                AlertCode = AlertCode.ALERT_SELF_TEST_FAILED_BLE;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_SELF_TEST_FAILED_SRAM))
            {
                AlertCode = AlertCode.ALERT_SELF_TEST_FAILED_SRAM;
                Alerts.Add(AlertCode);
            }

            if (code.HasFlag(AlertCode.ALERT_PATCH_NOT_CONNECTED))
            {
                AlertCode = AlertCode.ALERT_PATCH_NOT_CONNECTED;
                Alerts.Add(AlertCode);
            }
        }
    }
}

