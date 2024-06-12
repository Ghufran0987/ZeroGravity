using System;
using System.Linq;
using System.Collections.Generic;
using ZeroGravity.SugarBeat.Algorithms.Models;
using System.Diagnostics;

namespace ZeroGravity.SugarBeat.Algorithms
{
    public class MetabolicScoreCalulator
    {
        private IReadOnlyList<ActualGlucouseValue> _actualGlucouseValue;
        private IReadOnlyList<double> _greenValues;

        public MetabolicScoreCalulator(IReadOnlyList<ActualGlucouseValue> actualGlucouseValue)
        {
            _actualGlucouseValue = actualGlucouseValue ?? throw new ArgumentNullException();
            LoadGreenZoneValues();
            // Check is sufficient data point available before calculation else come back later ?
            //if (_actualGlucouseValue.Count < _greenValues.Count / 2)
            //{
            //    throw new ArgumentOutOfRangeException();
            //}
        }

        public MetabolicScoreCalulator()
        {
            LoadGreenZoneValues();
            LoadTestValues();
        }

        private void LoadTestValues()
        {
            _actualGlucouseValue = new List<ActualGlucouseValue>
            {
              new ActualGlucouseValue { Time =0, Value =4.41380660671832},
              new ActualGlucouseValue { Time =5, Value =4.39158645057492},
              new ActualGlucouseValue { Time =10, Value =4.1182694722105},
              new ActualGlucouseValue { Time =15, Value =4.4632565170999},
              new ActualGlucouseValue { Time =20, Value =4.52181502974547},
              new ActualGlucouseValue { Time =25, Value =4.59670580328271},
              new ActualGlucouseValue { Time =30, Value =4.7003528182841},
              new ActualGlucouseValue { Time =35, Value =4.81562204909691},
              new ActualGlucouseValue { Time =40, Value =4.9825254777198},
              new ActualGlucouseValue { Time =45, Value =5.20559124177082},
              new ActualGlucouseValue { Time =50, Value =5.46599865189965},
              new ActualGlucouseValue { Time =55, Value =5.75130506624355},
              new ActualGlucouseValue { Time =60, Value =5.93655980061571},
              new ActualGlucouseValue { Time =65, Value =5.99618153282283},
              new ActualGlucouseValue { Time =70, Value =5.93838722667264},
              new ActualGlucouseValue { Time =75, Value =5.81967170350588},
              new ActualGlucouseValue { Time =80, Value =5.62445875747664},
              new ActualGlucouseValue { Time =85, Value =5.4869869443289},
              new ActualGlucouseValue { Time =90, Value =5.47167382042053},
              new ActualGlucouseValue { Time =95, Value= 5.47620904141918},
              new ActualGlucouseValue { Time =100, Value =5.41956743410728},
              new ActualGlucouseValue { Time =105, Value = 5.322452974121},
              new ActualGlucouseValue { Time =110, Value = 5.15567736735957},
              new ActualGlucouseValue { Time =115, Value = 4.87685595500293},
              new ActualGlucouseValue { Time =120, Value =4.58149663131087},
              new ActualGlucouseValue { Time =125, Value = 4.29479400718619},
              new ActualGlucouseValue { Time =130, Value =4.01160269927478},
              new ActualGlucouseValue { Time =135, Value =3.73092351855646},
              new ActualGlucouseValue { Time =140, Value =3.55180736625592},
              new ActualGlucouseValue { Time =145, Value =3.49569630808324},
              new ActualGlucouseValue { Time =150, Value =3.49569630808324},
              new ActualGlucouseValue { Time =155, Value =3.49569630808324},
              new ActualGlucouseValue { Time =160, Value =3.49569630808324},
              new ActualGlucouseValue { Time =165, Value =3.49569630808324},
              new ActualGlucouseValue { Time =170, Value =3.49569630808324},
              new ActualGlucouseValue { Time =175, Value =3.49569630808324},
              new ActualGlucouseValue { Time =180, Value =3.49569630808324},
              new ActualGlucouseValue { Time =185, Value =3.49569630808324},
              new ActualGlucouseValue { Time =190, Value =3.49569630808324},
              new ActualGlucouseValue { Time =195, Value =3.49569630808324},
              new ActualGlucouseValue { Time =200, Value =3.49569630808324},
              new ActualGlucouseValue { Time =205, Value =3.49569630808324},
              new ActualGlucouseValue { Time =210, Value =3.49569630808324},
              new ActualGlucouseValue { Time =215, Value =3.49569630808324},
              new ActualGlucouseValue { Time =220, Value =3.49569630808324},
              new ActualGlucouseValue { Time =225, Value =3.49569630808324},
              new ActualGlucouseValue { Time =230, Value =3.49569630808324},
              new ActualGlucouseValue { Time =235, Value =3.49569630808324},
              new ActualGlucouseValue { Time =240, Value =3.49569630808324}
            };
        }

        private void LoadGreenZoneValues()
        {
            _greenValues = new List<double>
            {
                18.27919785,
                18.60042443,
                19.02283732,
                19.5650366,
                20.24399355,
                21.07288994,
                22.05866343,
                23.19950146,
                24.48260961,
                25.88263001,
                27.36108183,
                28.86712486,
                30.33980909,
                31.7117784,
                32.91417407,
                33.88227078,
                34.56121589,
                34.91116828,
                34.91116828,
                34.56121589,
                33.88227078,
                32.91417407,
                31.7117784,
                30.33980909,
                28.86712486,
                27.36108183,
                25.88263001,
                24.48260961,
                23.19950146,
                22.05866343,
                21.07288994,
                20.24399355,
                19.5650366,
                19.02283732,
                18.60042443,
                18.27919785,
                18.04064965,
                17.86759076,
                17.74490271,
                17.65988316,
                17.60228026,
                17.56411515,
                17.53938303,
                17.52370483,
                17.51398118,
                17.50808034,
                17.50457609,
                17.50253943
            };
        }

        private double GetGreenZoneSum(int count)
        {
            if (count <= 0)
                return 0;
            var sum = (from x in _greenValues select x).Take(count).Sum();
            return sum;
        }

        private double GetActualGlucoseSum(int count)
        {
            if (count <= 0)
                return 0;
            var sum = (from x in _actualGlucouseValue select x.Delta).Take(count).Sum();
            return sum;
        }

        public double GetMetabolicScore()
        {
            // Normailze values if we have minimum 2 points
            if(_actualGlucouseValue?.Count > 2)
            {
                var temp = NormailzeDrops(_actualGlucouseValue?.ToList(), _actualGlucouseValue?.OrderByDescending(x => x.Time).FirstOrDefault());
                if(temp != null)
                {
                    _actualGlucouseValue = temp;
                }
            }

            // Calculate sum based on no of Glucose records
            for (int i = 0; i <= _actualGlucouseValue.Count - 1; i++)
            {
                if ((i + 1) <= _actualGlucouseValue.Count - 1)
                {
                    ActualGlucouseValue first = _actualGlucouseValue[i];
                    ActualGlucouseValue second = _actualGlucouseValue[i + 1];
                    first.Delta = GetDelta(first, second);
                }
            }

            int count = _actualGlucouseValue.Count;
            if (count > _greenValues.Count)
            {
                count = _greenValues.Count;
            }
            double greenSum = GetGreenZoneSum(count);
            double actualSum = GetActualGlucoseSum(count);
            if (greenSum > 0)
            {
                double scorePercentage = Math.Ceiling((actualSum / greenSum) * 100);
                if (scorePercentage <= 50)
                {
                    return 95;
                }
                else if (scorePercentage > 50 && scorePercentage <= 100)
                {
                    return 90;
                }
                else if (scorePercentage > 100 && scorePercentage <= 120)
                {
                    return 80;
                }
                else if (scorePercentage > 120 && scorePercentage <= 250)
                {
                    return Math.Round(80 - ((scorePercentage - 120) / 1.857));
                }
                else if (scorePercentage > 250)
                {
                    return 10;
                }
            }
            return 0;
        }

        private double GetDelta(ActualGlucouseValue first, ActualGlucouseValue second)
        {
            if (first == null && second == null)
                return -1;
            var d = (second.Value + first.Value) / 2;
            return d * (second.Time - first.Time);
        }


        private List<ActualGlucouseValue> NormailzeDrops(List<ActualGlucouseValue> range, ActualGlucouseValue current)
        {
            try
            {
                if (current == null || range == null) return null;

                // Look for peak in actual glucose values between the 30 and 150 minute mark
                var greenZonePeakCheckValues = range.Where(x => x.Time >= 30 && x.Time <= 150).ToList();
                var ordered = greenZonePeakCheckValues.OrderByDescending(x => x.Value).ToList();
                var peak = ordered.First();
                var indexOfPeak = range.IndexOf(peak);

                // Wait for minumum 5 points after prek
                if (current.Time >= peak.Time)
                {
                    // Find Max Point in next 5 points after peak
                    var pointsAfterPeak = range.Where(x => x.Time >= peak.Time).ToList();
                    if (pointsAfterPeak?.Count > 5)
                    {
                        // Check for Rate of Fall in next 5 points from max points if not found skip

                        double sumDifferencesPostPeak = 0;

                        for (int i = 1; i < 5; i++)
                        {
                            sumDifferencesPostPeak += pointsAfterPeak[i].Value - pointsAfterPeak[i - 1].Value;
                        }

                        var averageRateOfChangePostPeak = sumDifferencesPostPeak / 4;

                        if (averageRateOfChangePostPeak < 0)
                        {
                            // debug logging showing values before modification
                            for (int i = 0; i < range.Count; i++)
                            {
                               Debug.WriteLine(range[i].Time + "," + range[i].Value);
                            }

                            // the glucose values are falling after the identified peak.

                            var updateList = range.ToList();

                            // update check the y values after the 5 points used to find the rate of fall and correct them if
                            // needed to ensure they stay within that rate of fall.

                            for (int i = indexOfPeak + 5; i < updateList.Count; i++)
                            {
                                if (updateList[i].Value - updateList[i - 1].Value > averageRateOfChangePostPeak)
                                {
                                    updateList[i].Value = updateList[i - 1].Value + averageRateOfChangePostPeak;
                                }
                            }

                            // debug logging showing values after modification
                            Debug.WriteLine("sumDifferencesPostPeak: " + sumDifferencesPostPeak);
                            Debug.WriteLine("averageRateOfChangePostPeak: " + averageRateOfChangePostPeak);
                            Debug.WriteLine("peak x: " + peak.Time + ", y: " + peak.Value);
                            Debug.WriteLine("peak index: " + indexOfPeak);

                            for (int i = 0; i < range.Count; i++)
                            {
                                Debug.WriteLine(updateList[i].Time + "," + updateList[i].Value);
                            }

                            return updateList;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex, ex.Message);
            }
            return null;
        }

    }
}