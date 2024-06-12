using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex;
using System;
using System.Collections.Generic;

namespace ZeroGravity.SugarBeat.Algorithms.Models
{
    public struct PredictionAlgorithmOutput
    {
        public List<int> timestamps;  // for convenience
        public List<double> sensorMeasurements;
        public int? warmupCompletedIndex;
        public List<double> processedRawValues;
        public List<double> preProcessedRawValues;

        public PredictionAlgorithmOutput(List<int> timestamps, List<double> sensorMeasurements, int? warmupCompletedIndex, List<double> processedRawValues, List<double> preProcessedRawValues) : this()
        {
            this.timestamps = timestamps;
            this.sensorMeasurements = sensorMeasurements;
            this.warmupCompletedIndex = warmupCompletedIndex;
            this.processedRawValues = processedRawValues;
            this.preProcessedRawValues = preProcessedRawValues;
        }
    }
}
