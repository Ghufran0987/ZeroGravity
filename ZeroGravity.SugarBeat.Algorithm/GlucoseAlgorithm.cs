using System;
using System.Collections.Generic;
using ZeroGravity.SugarBeat.Algorithms.Models;

namespace ZeroGravity.SugarBeat.Algorithms
{
    public class GlucoseAlgorithm
    {

        private static readonly double DEFAULT_CALIBRATION_POINT = 5;

        private List<double> glucoseValues;
        private List<double> calibratedGlucoseValues;

        private readonly ProcessingAlgorithm processing = new ProcessingAlgorithm();
        private readonly LinearCalibrationModel linearCalibrationModel = new LinearCalibrationModel();

        /**
         * Run the glucose algorithm on the set of readings provided.
         *
         * @param glucoseAlgorithmInputEntries - the glucose readings received from the gluose monitor.
         * @param lastCalibrationVectorFromPreviousSensorSession - Cn calibration factor from previous sensor used.
         * @param F0 - the F0 value used when no calibrations have been provided, will vary depending on whether the user is diabetic or not
         * @param recalibrateAfter - indicates that calibration should be run again this number of ts entries after the calibration was recieved.
         */
        public GlucoseAlgorithmOutput? Run(List<GlucoseAlgorithmInputEntry> glucoseAlgorithmInputEntries, double? lastCalibrationVectorFromPreviousSensorSession, double F0, int recalibrateAfter)
        {
            if (glucoseAlgorithmInputEntries == null || glucoseAlgorithmInputEntries.Count == 0)
            {
                throw new ArgumentException("Empty list of readings supplied to algorithm part A calculation");
            }

            PredictionAlgorithmOutput? predictionAlgorithmOutput = processing.Run(glucoseAlgorithmInputEntries);

            if (predictionAlgorithmOutput != null)
            {

                if (predictionAlgorithmOutput.Value.processedRawValues == null || predictionAlgorithmOutput.Value.processedRawValues.Count == 0)
                {
                    throw new ArgumentException("processedRawValues has not been populated prior to calling calculateGlucoseValues()");
                }

                if (predictionAlgorithmOutput.Value.preProcessedRawValues == null || predictionAlgorithmOutput.Value.preProcessedRawValues.Count == 0)
                {
                    throw new ArgumentException("preProcessedRawValues has not been populated prior to calling calculateGlucoseValues()");
                }

                CalculateGlucoseValues(predictionAlgorithmOutput.Value);
            }
            else
            {
                // The sensor has not warmed up.

                EstimateGlucoseValues(predictionAlgorithmOutput.Value);
            }

            // create the glucose algorithm output entries.
            List<GlucoseAlgorithmOutputEntry> glucoseAlgorithmOutputEntries = new List<GlucoseAlgorithmOutputEntry>();
            for (int ts = 0; ts < glucoseValues.Count; ts++)
            {
                glucoseAlgorithmOutputEntries.Add(
                    new GlucoseAlgorithmOutputEntry(
                        predictionAlgorithmOutput.Value.timestamps[ts], 
                        predictionAlgorithmOutput.Value.sensorMeasurements[ts], 
                        glucoseValues[ts] * 1.4
                    )
                );
            }

            return new GlucoseAlgorithmOutput(glucoseAlgorithmOutputEntries, predictionAlgorithmOutput.Value.warmupCompletedIndex, null);
        }

        /**
         * Calculate the final glucose value using DEFAULT_CALIBRATION_VALUE
         *
         */
        private void CalculateGlucoseValues(PredictionAlgorithmOutput predictionAlgorithmOutput)
        {
            glucoseValues = new List<double>();
            calibratedGlucoseValues = new List<double>();
            double y = DEFAULT_CALIBRATION_POINT;
            int firstCalibrationIndex = predictionAlgorithmOutput.warmupCompletedIndex.Value;
            double x = predictionAlgorithmOutput.processedRawValues[firstCalibrationIndex];
            calibratedGlucoseValues = linearCalibrationModel.Predict(predictionAlgorithmOutput.processedRawValues, y, x);
            List<double> smoothedCalibratedGlucoseValues = ApplyLowPassFilter(calibratedGlucoseValues);

            for (int ts = 0; ts < predictionAlgorithmOutput.processedRawValues.Count; ts++)
            {
                glucoseValues.Add(CorrectGlucoseRange(smoothedCalibratedGlucoseValues[ts]));
            }
        }


        /*
         * smoothing method using lowPassFilter
         *
         * @param values : Values to be smoothed using lowPassFilter
         */
        private List<double> ApplyLowPassFilter(List<double> values)
        {
            List<double> smoothedValues = new List<double>();
            smoothedValues.AddRange(values);
            if (values.Count > 2)
            {
                for (int i = 2; i < values.Count; i++)
                {
                    if (i < values.Count)
                    {
                        double alphaForLPF = 0.5;
                        double betaForLPF = 0.25;
                        smoothedValues[i - 1] = alphaForLPF * values[i - 1] + betaForLPF * (values[i - 2] + values[i]);
                    }
                    if (i == values.Count - 1)
                    {
                        smoothedValues[i] = (values[i] + values[i - 1]) / 2d;
                    }
                }
            }
            return smoothedValues;
        }



        /**
         * Part H of Summary of Dynamic Algorithm_v2.pdf
         * Applies a final correction to Yf values
         *
         * @param Yf: the Yf value to correct
         * @return the corrected Yf value
         */
        private static double CorrectGlucoseRange(double Yf)
        {
            if (Yf >= 3.6 && Yf <= 22.22)
            {
                return Yf;
            }
            else if (Yf < 3.6)
            {
                return 3.6;
            }
            return 22.22;

        }


        /**
         * Display Estimated Glucose Values using the preProcessedRawValues during warmup and before user inserts calibration value
         *
         */
        private void EstimateGlucoseValues(PredictionAlgorithmOutput predictionAlgorithmOutput)
        {
            // Extract preProcessedRawValues into separate array for easy access.
            if (predictionAlgorithmOutput.sensorMeasurements.Count > 0)
            {
                List<double> preProcessedRawValues = predictionAlgorithmOutput.preProcessedRawValues;
                glucoseValues = new List<double>();
                List<double> estimatedGlucoseValues = linearCalibrationModel.Predict(preProcessedRawValues, DEFAULT_CALIBRATION_POINT, preProcessedRawValues[0]);
                // apply final correction to estimatedGlucoseValues
                for (int ts = 0; ts < estimatedGlucoseValues.Count; ts++)
                {
                    glucoseValues.Add(CorrectGlucoseRange(estimatedGlucoseValues[ts]));
                }
            }
        }


        /**
         * LinearCalibrationModel - used to predict glucose values
         */
        protected class LinearCalibrationModel
        {
            /**
             * predict glucose values here using this method
             */
            public List<double> Predict(List<double> processedRawValues, double y, double x)
            {
                List<double> predictedGlucoseValues = new List<double>();
                double slope = 3.5;
                double intercept = y - slope * x;
                for (int ts = 0; ts < processedRawValues.Count; ts++)
                {
                    predictedGlucoseValues.Add(slope * processedRawValues[ts] + intercept);
                }
                return predictedGlucoseValues;
            }
        }
    }
}
