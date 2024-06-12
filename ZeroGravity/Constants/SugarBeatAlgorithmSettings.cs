namespace ZeroGravity.Constants
{
    public class SugarBeatAlgorithmSettings
    {
        // We aren’t calibrating the MiBoKo system so these calibration values can be left null
        public const double CALIBRATION_VALUE = 12.5;

        // LastCalibrationVectorFromPreviousSensorSession should be null, since we are not calibrating the MiBoKo system
        public const double LAST_CALIBRATION_VECTOR_FROM_PREVIOUS_SENSOR_SESSION = 0.615d;

        public const double F0_VALUE = 12.5;

        public const int RECALIBRATE_AFTER = 3;
    }
}