using System.Collections.Generic;

namespace ZeroGravity.SugarBeat.Algorithms.Models
{
    public struct GlucoseAlgorithmOutput
    {
        // the list of outputs from the glucose algorithm
        public List<GlucoseAlgorithmOutputEntry> glucoseAlgorithmOutputEntries;

        // the index at which point warmup is considered complete
        public int? warmupIndex;

        // the last calibration vector for this sensor sessions data
        public double? lastCalibrationVector;

        public GlucoseAlgorithmOutput(List<GlucoseAlgorithmOutputEntry> glucoseAlgorithmOutputEntries, int? warmupIndex, double? lastCalibrationVector) : this()
        {
            this.glucoseAlgorithmOutputEntries = glucoseAlgorithmOutputEntries;
            this.warmupIndex = warmupIndex;
            this.lastCalibrationVector = lastCalibrationVector;
        }
    }
}
