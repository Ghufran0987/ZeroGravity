using System;
using System.Collections.Generic;

namespace ZeroGravity.SugarBeat.Algorithms
{
    public class AlgorithmSubFunctions
    {
        /**
         * Calculate the slope and intercept for the Best Fit Slope
         *
         * @param values: list of values
         * @param indexes: list of indexes
         * @return the slope and intercept for the Best Fit Slope
         */

        public List<double> GetLinearModelParams(List<double> indexes, List<double> values)
        {
            List<double> getLinearModelParams = new List<double>();
            double slopeValue = ((Mean(indexes) * Mean(values)) - Mean(DotProduct(indexes, values))) / ((Mean(indexes) * Mean(indexes)) - Mean(DotProduct(indexes, indexes)));
            double interceptValue = Mean(values) - (slopeValue * Mean(indexes));
            getLinearModelParams.Add(slopeValue);
            getLinearModelParams.Add(interceptValue);
            return getLinearModelParams;
        }

        /**
         * Calculates the mean of a list of values
         *
         * @param values: list of values
         * @return the mean of the list of values
         */

        public double Mean(List<double> values)
        {
            double sum = 0;

            foreach (double value in values)
            {
                sum += value;
            }

            return sum / values.Count;
        }

        /**
         * Calculate the product of two lists of values
         *
         * @param v1: list of values
         * @param v2: list of values
         * @return a list of values containing the products
         */

        public List<double> DotProduct(List<double> v1, List<double> v2)
        {
            List<double> dotProduct = new List<double>();
            for (int i = 0; i < v1.Count; i++)
            {
                dotProduct.Add(v1[i] * v2[i]);
            }
            return dotProduct;
        }

        /**
         * Generate list of indexes
         *
         * @return list of indexes
         */

        public List<double> GenIndexes(int num)
        {
            List<double> genIndexes = new List<double>();
            for (double i = 0; i < num; i++)
            {
                genIndexes.Add(i);
            }
            return genIndexes;
        }

        /**
         * Standard deviation is a statistical measure of spread or variability.The
         * standard deviation is the root mean square (RMS) deviation of the values
         * from their arithmetic mean.
         *
         * <b>standardDeviation</b> normalizes values by (N-1), where N is the sample size.  This is the
         * sqrt of an unbiased estimator of the variance of the population from
         * which X is drawn, as long as X consists of independent, identically
         * distributed samples.
         *
         * @param values
         * @return
         */

        public double StandardDeviation(List<double> values)
        {
            double mean = Mean(values);
            double dv = 0D;
            foreach (double d in values)
            {
                double dm = d - mean;
                dv += dm * dm;
            }
            return Math.Sqrt(dv / (values.Count - 1));
        }

        /**
         * Difference and approximate derivative
         *
         */

        public List<double> Differences(List<double> values)
        {
            List<double> differences = new List<double>();
            for (int i = 1; i < values.Count; i++)
            {
                differences.Add(values[i] - values[i - 1]);
            }
            return differences;
        }


        /**
         * calculate Log Values
         *
         */
        public List<double> CalculateLogValues(List<double> values)
        {
            List<double> logValues = new List<double>();
            foreach (double value in values)
            {
                logValues.Add(Math.Log(value));
            }
            return logValues;
        }

        /**
         * calculate Power Values
         *
         */
        public List<double> CalculatePowerValues(List<double> values)
        {
            List<double> powerValues = new List<double>();
            foreach (double value in values)
            {
                powerValues.Add(Math.Pow(value, 2));
            }
            return powerValues;
        }

        /**
         * calculate abd for List Values
         *
         */
        public List<double> AbsListValues(List<double> values)
        {
            List<double> absListValues = new List<double>();
            foreach (double value in values)
            {
                absListValues.Add(Math.Abs(value));
            }
            return absListValues;
        }

        /**
         * Generate list of indexes
         *
         * @return list of indexes
         */
        public List<double> ListOfIndexes(int fromThisValue, int toThisValue)
        {
            List<double> listOfIndexes = new List<double>();
            for (double i = fromThisValue; i <= toThisValue; i++)
            {
                listOfIndexes.Add(i);
            }
            return listOfIndexes;
        }

        /**
         * Calculate the median of an array of values
         *
         * @param values The values to calculate
         * @return The median of the values
         */

        public double Median(List<double> values)
        {
            // sort array
            values.Sort();
            double median;
            int totalElements = values.Count;
            // check if total number of scores is even
            if (totalElements % 2 == 0)
            {
                double sumOfMiddleElements = values[(totalElements / 2) - 1] + values[totalElements / 2];
                // calculate average of middle elements
                median = sumOfMiddleElements / 2;
            }
            else
            {
                // get the middle element
                median = values[totalElements / 2];
            }
            return median;
        }

        /**
         * Calculate the parameters for the power model
         *
         * @param values: list of values
         * @param indexes: list of indexes
         * @return the slope and intercept for the Best Fit Slope
         */
        public List<double> FitPowerModelVars(List<double> indexes, List<double> values)
        {
            List<double> powerModelParams = new List<double>();
            List<double> logX = CalculateLogValues(indexes);
            double meanLogX = Mean(logX);
            List<double> logY = CalculateLogValues(values);
            double meanLogY = Mean(logY);

            double sXX = Sum(CalculatePowerValues(SubValFromList(logX, meanLogX)));
            double sYY = Sum(CalculatePowerValues(SubValFromList(logY, meanLogY)));
            double sXY = Sum(DotProduct(SubValFromList(logX, meanLogX), SubValFromList(logY, meanLogY)));

            double b = sXY / sXX;
            double a = Math.Exp(meanLogY - b * meanLogX);

            powerModelParams.Add(a);
            powerModelParams.Add(b);
            return powerModelParams;
        }

        /**
          * Sum up all the values in an array
          *
          * @param values an array of values
          * @return The sum of all values in the Array
          */
        public double Sum(List<double> values)
        {
            double sum = 0;
            foreach (double value in values)
            {
                sum += value;
            }
            return sum;
        }

        /**
          * Subtract value from a List
          *
          */
        public List<double> SubValFromList(List<double> listValues, double val)
        {
            List<double> subValFromList = new List<double>();
            foreach (double thisValue in listValues)
            {
                subValFromList.Add(thisValue - val);
            }
            return subValFromList;
        }

        /**
         * subtract two lists of values
         *
         * @param v1: list of values
         * @param v2: list of values
         * @return a list of values containing the products
         */
        public List<double> SubtractTwoLists(List<double> v1, List<double> v2)
        {
            List<double> subtractTwoLists = new List<double>();
            for (int i = 0; i < v1.Count; i++)
            {
                subtractTwoLists.Add(v1[i] - v2[i]);
            }
            return subtractTwoLists;
        }
    }
}