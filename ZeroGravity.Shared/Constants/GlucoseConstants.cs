namespace ZeroGravity.Shared.Constants
{
    public static class GlucoseConstants
    {
        public static double MinGlucoseValue = 0.0;
        public static double MaxGlucoseValue = 20.0;

        public const string GlucoseTypeManual = "manual";
        public const string GlucoseTypeSugarBeat = "sugarBeat";
        public const string GlucoseTypeAll = "all";

        public const int SensorExpiryPeriod = 14; // sensors are only valid for 14 hours
        public const int EatingSessionPeriod = 3; // eating sessions run for 3 hours

    }
}
