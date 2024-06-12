namespace ZeroGravity.SugarBeat.Algorithms.Models
{
    public class GlucoseAlgorithmInputEntry
    {
        // the timestmap of the sensor measurement provided for convenience.
        public int Timestamp { get; }

        // sensor measurement as recieved from the transmitter
        public double SensorMeasurement { get; }

        // calibration value entered by the user in mmol/L
        public double? CalibrationValue { get; }

        public GlucoseAlgorithmInputEntry(int timestamp, double sensorMeasurement, double? calibrationValue)
        {
            Timestamp = timestamp;
            SensorMeasurement = sensorMeasurement;
            CalibrationValue = calibrationValue;
        }
    }
}
