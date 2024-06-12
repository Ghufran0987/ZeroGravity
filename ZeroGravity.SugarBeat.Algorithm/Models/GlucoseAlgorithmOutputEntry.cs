namespace ZeroGravity.SugarBeat.Algorithms.Models
{
    public class GlucoseAlgorithmOutputEntry
    {
        // the timestmap of the sensor measurement provided for convenience.
        public int Timestamp { get; }

        // sensor measurement as recieved from the transmitter
        public double SensorMeasurement { get; }

        // the calculated glucose value derrived by the algorithm
        public double GlucoseValues { get; }

        public GlucoseAlgorithmOutputEntry(int timestamp, double sensorMeasurement, double glucoseValues)
        {
            Timestamp = timestamp;
            SensorMeasurement = sensorMeasurement;
            GlucoseValues = glucoseValues;
        }
    }
}
