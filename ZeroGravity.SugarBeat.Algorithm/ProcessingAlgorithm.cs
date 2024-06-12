using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Double;
using ZeroGravity.SugarBeat.Algorithms.Models;

namespace ZeroGravity.SugarBeat.Algorithms
{
    class ProcessingAlgorithm
    {
        // protected for unit testing
        private List<double> preProcessedRawValues;
        private List<double> processedRawValues;
        private List<double> rateOfChangeRaw;
        private int? warmupCompletedIndex;


        private readonly AlgorithmSubFunctions algorithmSubFunctions = new AlgorithmSubFunctions();


        public PredictionAlgorithmOutput? Run(List<GlucoseAlgorithmInputEntry> glucoseAlgorithmInputEnties)
        {
            if (glucoseAlgorithmInputEnties == null || glucoseAlgorithmInputEnties.Count == 0)
            {
                throw new ArgumentException("Empty list of readings supplied to algorithm part A calculation");
            }

            // (1) convert raw values to uAmp
            List<double> processedValues1 = ConvertToMicroAmp(glucoseAlgorithmInputEnties);
            List<double> processedValues2 = ProcessFirstFourReadings(processedValues1);
            List<double> processedValues3 = Detrends(processedValues2);
            preProcessedRawValues = ApplyKalmanFilter(ApplyPowerTransform(processedValues3), 1.0);
            rateOfChangeRaw = RateOfChangeOverTime(processedValues2);

            if (glucoseAlgorithmInputEnties.Count > 3)
            {
                ProcessWarmupReadings(processedValues2, rateOfChangeRaw);
                if (warmupCompletedIndex != null)
                {
                    List<double> processedValues4 = ClippLimits(processedValues3);
                    List<double> processedValues5 = ApplyPowerTransform(processedValues4);
                    List<double> processedValues6 = ApplyKalmanFilter(processedValues5, 1.0);
                    List<double> processedValues7 = FivePointsMovingAverage(processedValues6);
                    // finally save the last process into separate array for easy access in the main algorithm and for testing
                    processedRawValues = new List<double>();
                    processedRawValues = processedValues7;

                    List<int> timestamps = ExtractTimestamps(glucoseAlgorithmInputEnties);
                    return new PredictionAlgorithmOutput(timestamps, processedValues1, warmupCompletedIndex, processedRawValues, preProcessedRawValues);
                }
            }

            return null;
        }


        /**
         * Function to apply Kalman Filter to provide estimates of  a given list of values
         *
         * @param values - subset of Xn
         * @return predictedSmoothedRawValues list
         */
        protected List<double> ApplyKalmanFilter(List<double> values, double dt)
        {

            List<double> estimatedValues = new List<double>();

            if (values.Count > 0)
            {
                double initialValue = values[0];
                double u = 0.0;

                double[,] aMatrixValues = new double[,] { { 1d, dt }, { 0d, 1d } };
                Matrix<double> A = DenseMatrix.OfArray(aMatrixValues);

                double[,] bMatrixValues = new double[,] { { Math.Pow(dt, 2) / 2d }, { dt } };
                Matrix<double> B = DenseMatrix.OfArray(bMatrixValues);

                double[,] cMatrixValues = new double[,] { { 1d, 0d } };
                Matrix<double> C = DenseMatrix.OfArray(cMatrixValues);

                double[,] idMatirxValues = new double[,] { { 1d, 0d }, { 0d, 1d } };
                Matrix<double> Id = DenseMatrix.OfArray(idMatirxValues);

                double[,] xEstimateMatrixValues = new double[,] { { initialValue }, { 0d } };
                Matrix<double> xEstimate = DenseMatrix.OfArray(xEstimateMatrixValues);

                double processNoiseMag = 0.1; // algorithmSubFunctions.StandardDeviation(algorithmSubFunctions.Differences(values));
                double measurementNoise = 1.7686;  //2 * algorithmSubFunctions.StandardDeviation(values);  // measurement noise: (2*stdv of Xn)

                double Ez = Math.Pow(measurementNoise, 2.0);

                double[,] EzMatrixValues = new double[,] { { Math.Pow(dt, 4d) / 4d, Math.Pow(dt, 3d) / 2d }, { Math.Pow(dt, 3d) / 2d, Math.Pow(dt, 2d) } };
                Matrix<double> Ex = DenseMatrix.OfArray(EzMatrixValues).Multiply(Math.Pow(processNoiseMag, 2d));

                Matrix<double> P = DenseMatrix.OfMatrix(Ex);

                for (int t = 0; t < values.Count; t++)
                {
                    xEstimate = A.Multiply(xEstimate).Subtract(B.Multiply(u));
                    P = A.Multiply(P).Multiply(A.Transpose()).Add(Ex);
                    Matrix<double> KGain = P.Multiply(C.Transpose()).Multiply(1.0 / (C.Multiply(P).Multiply(C.Transpose())[0, 0] + Ez));
                    xEstimate = xEstimate.Add(KGain.Multiply(values[t] - C.Multiply(xEstimate)[0, 0]));
                    P = Id.Subtract(KGain.Multiply(C)).Multiply(P);
                    estimatedValues.Add(xEstimate[0, 0]);
                }

            }

            return estimatedValues;

        }


        /**
         * Does any processing on the sensor measurement received from the transmitter
         *
         * @param sensorMeasurement: the sensor measurement to process
         * @return the processed sensor measurement value
         */
        private double ProcessRawReading(double sensorMeasurement)
        {
            return sensorMeasurement / 1000.0;
        }


        /**
         * (1) Convert the raw value received from the glucose monitor to micro Amp
         *
         * @param readings: list of readings
         * @return the list of raw readings from the GlucoseReading objects
         */
        private List<double> ConvertToMicroAmp(List<GlucoseAlgorithmInputEntry> algorithmGlucoseEntries)
        {
            List<double> convertedReadings = new List<double>();
            foreach (GlucoseAlgorithmInputEntry algorithmGlucoseEntry in algorithmGlucoseEntries)
            {
                convertedReadings.Add(ProcessRawReading(algorithmGlucoseEntry.SensorMeasurement));
            }
            return convertedReadings;
        }


        /**
         * Process First Two Readings
         *
         * @param values: list of readings
         */
        private List<double> ProcessFirstTwoReadings(List<double> values)
        {
            List<double> processedValues = new List<double>(values);
            if (values.Count > 3)
            {
                double difference = values[3] - values[2];
                processedValues[1] = values[2] - difference;
                processedValues[0] = processedValues[1] - difference;
            }
            return processedValues;
        }


        /**
         * Process First Four Readings
         *
         * @param values: list of readings
         */
        private List<double> ProcessFirstFourReadings(List<double> values)
        {
            List<double> processedValues = new List<double>(values);
            if (values.Count > 4)
            {
                double difference = values[4] - values[3];
                processedValues[2] = values[3] - difference;
                processedValues[1] = processedValues[2] - difference;
                processedValues[0] = processedValues[1] - difference;
            }
            return processedValues;
        }


        /**
          * Remove inherited Trends
          */
        private List<double> Detrends(List<double> values)
        {
            List<double> detrendedValues = new List<double>();
            List<double> logValues = algorithmSubFunctions.CalculateLogValues(values);
            detrendedValues.Add(values[0]);
            for (int ts = 1; ts < values.Count; ts++)
            {
                List<double> theseValues = new List<double>(logValues.GetRange(0, ts + 1));
                double meanValue = algorithmSubFunctions.Mean(theseValues);
                double stdValue = algorithmSubFunctions.StandardDeviation(theseValues);
                if (stdValue < 0)
                {
                    stdValue = 0.001;
                }
                detrendedValues.Add((logValues[ts] - meanValue) / stdValue);
            }
            return ProcessFirstTwoReadings(detrendedValues);
        }


        /*
         * Applies Power Transformation
         *
         * @param values : values to be transformed
         */
        protected List<double> ApplyPowerTransform(List<double> values)
        {
            List<double> processedValues = new List<double>();
            for (int i = 0; i < values.Count; i++)
            {
                if (values[i] >= 0)
                {
                    processedValues.Add(Math.Sqrt(values[i]));
                }
                else
                {
                    processedValues.Add(-1 * Math.Sqrt(Math.Abs(values[i])));
                }
            }
            return processedValues;
        }


        /**
         * Calculate rate Of Change Over Time
         *
         * @param values: list of readings
         */
        private List<double> RateOfChangeOverTime(List<double> values)
        {
            List<double> rateOfChange = new List<double>();
            List<double> processedValues = new List<double>();
            for (int i = 0; i < values.Count; i++)
            {
                processedValues.Add(Math.Log(values[i]));
            }
            for (int i = 0; i < processedValues.Count; i++)
            {
                rateOfChange.Add(((processedValues[i] - processedValues[0]) / i) + 10D);
            }
            return ProcessFirstTwoReadings(rateOfChange);
        }


        /**
         * Find the warmupCompletedIndex, defined as the Index of reading at which the sensor has warmed
         * Accounts for the warm up period of the device once a new sensor has been fitted.
         */
        protected void ProcessWarmupReadings(List<double> values, List<double> rateOfChange)
        {
            int readingThresholdIndex = 4;
            // (1) Model the warmup trend as power model
            int firstConsideredIndex = 1;
            int lastConsideredIndex = Math.Min(values.Count - 1, 9); // may be -1 to not crash
            List<double> trainingIndexes = algorithmSubFunctions.ListOfIndexes(firstConsideredIndex + 1, lastConsideredIndex + 1); //  create indexes like Matlab to get the same model params
            List<double> trainingValues = new List<double>(rateOfChange.GetRange(firstConsideredIndex, (lastConsideredIndex + 1) - firstConsideredIndex));
            // (2) fit the data to the power model
            List<double> powerModelParams = algorithmSubFunctions.FitPowerModelVars(trainingIndexes, trainingValues);
            double a = powerModelParams[0];
            double b = powerModelParams[1];
            List<double> indexes = algorithmSubFunctions.ListOfIndexes(1, values.Count); // create indexes like Matlab
            List<double> trend = new List<double>();
            foreach (double index in indexes)
            {
                trend.Add(a * (Math.Pow(index, b)));
            }

            // (3) Find the warmupCompletedIndex which is the detected index at which the sensor data point deviates more than 4 scaled median absolute deviations (MAD) away from the median
            // in the devations of the trend from the origonal values.

            List<double> theseTrendValues = new List<double>(trend.GetRange(firstConsideredIndex, (lastConsideredIndex + 1) - firstConsideredIndex));
            List<double> deviations = algorithmSubFunctions.SubtractTwoLists(trainingValues, theseTrendValues);
            double medianOfDevs = algorithmSubFunctions.Median(deviations);
            double thresholdFactor = 3;
            double MAD = algorithmSubFunctions.Median(algorithmSubFunctions.AbsListValues(algorithmSubFunctions.SubValFromList(deviations, medianOfDevs)));
            int count = 0;
            for (int ts = 3; ts < rateOfChange.Count; ts++)
            {
                if (ts == readingThresholdIndex)
                {
                    warmupCompletedIndex = ts;
                    break;
                }
                double thisDeviation = Math.Abs(rateOfChange[ts] - trend[ts]);
                double distanceFromMedian = Math.Abs(thisDeviation - medianOfDevs);
                if (distanceFromMedian > thresholdFactor * MAD)
                {
                    count = count + 1;
                    if (count == 3)
                    {
                        warmupCompletedIndex = ts - count;
                        break;
                    }
                }
                else
                {
                    count = 0;
                }
            }
        }


        /* Smoothing
         * Applies a Clipping Limits smoothing to calibratedReadings values
         *
         * @param values : values to be clipped
         */
        private List<double> ClippLimits(List<double> values)
        {
            List<double> clippedValues = new List<double>(values);
            double clippingLimit = 0.5;
            double previousValue = values[warmupCompletedIndex.Value];
            for (int i = warmupCompletedIndex.Value; i < values.Count; i++)
            {
                double currentDifference = values[i] - previousValue;
                if (currentDifference <= -1 * clippingLimit)
                {

                    clippedValues[i] = previousValue - clippingLimit;
                }
                previousValue = clippedValues[i];
            }
            return clippedValues;
        }


        /*
         * smoothing method using movingAverage
         */
        private List<double> FivePointsMovingAverage(List<double> values)
        {
            List<double> smoothedValues = new List<double>(values);
            for (int i = 1; i < values.Count; i++)
            {

                if (i < 4)
                {
                    smoothedValues[i] =(values[i - 1] + values[i]) / 2d;
                }
                if (i >= 4)
                {
                    smoothedValues[i - 2] = (values[i - 4] + values[i - 3] + values[i - 2] + values[i - 1] + values[i]) / 5d;
                }
                if (i >= values.Count - 2)
                {
                    smoothedValues[i] = (values[i - 1] + values[i]) / 2d;
                }
            }
            return smoothedValues;
        }


        /**
         * Extracts the raw reading values from a list or GlucoseReading objects
         *
         * @param glucoseAlgorithmInputEntries: list of glucose algorithm input entries
         * @return the list of timestamps
         */
        private List<int> ExtractTimestamps(List<GlucoseAlgorithmInputEntry> glucoseAlgorithmInputEntries)
        {
            List<int> timestamps = new List<int>();
            foreach (GlucoseAlgorithmInputEntry algorithmGlucoseEntry in glucoseAlgorithmInputEntries)
            {
                timestamps.Add(algorithmGlucoseEntry.Timestamp);
            }
            return timestamps;
        }
    }
}
