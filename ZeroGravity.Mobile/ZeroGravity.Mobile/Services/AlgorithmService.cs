using System;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Services
{
    public class AlgorithmService : IAlgorithmService
    {
        public double CalculateBmi(double height, double weight)
        {
            double bmi = 0;

            if (height != 0 && weight != 0)
            {
                var heightInMeter = height / 100.0;

                var heightQ = Math.Pow(heightInMeter, 2);

                bmi = Math.Round(weight / heightQ, 1);
            }

            return bmi;
        }

        public double CalculateBodyFatPercentage(GenderBiologicalType sex, double waist, double hip, double neck, double height)
        {
            // Waist (m);
            // Neck =(m);
            // Height=(m);
            //waist *= 100.0;
            //hip *= 100.0;
            //neck *= 100.0;
            //height *= 100.0;

            var bodyFatPercentage = 0.0;

            switch (sex)
            {
                case GenderBiologicalType.Female:
                    //          BodyFat_Score = 495 / (1.29579 - .35004 * log10(Waist + Hip - Neck) + 0.22100 * log10(Height)) - 450;  
                    bodyFatPercentage = 495.0 / (1.29579 - 0.35004 * Math.Log10(waist + hip
                        - neck) + 0.221 * Math.Log10(height)) - 450.0;
                    break;

                case GenderBiologicalType.Male:
                case GenderBiologicalType.NonBinary:
                case GenderBiologicalType.Undefined:
                    //          BodyFat_Score = 495 / (1.29579 - .35004 * log10(Waist - Neck) + 0.22100 * log10(Height)) - 450; 
                    bodyFatPercentage = 495.0 / (1.0324 - 0.19077 * Math.Log10(waist - neck)
                                                 + 0.15456 * Math.Log10(height)) - 450.0;
                    break;
            }

            return Math.Round(bodyFatPercentage, 1);
        }

        public double GetBloodGlucose(double sugarlevel, double fasting)
        {
            double[] bFasting = { fasting, sugarlevel };

            return CompactRegressionTree_GlucosePredict(bFasting);
        }

        private double CompactRegressionTree_GlucosePredict(double[] predictionInput)
        {
            int[] objPruneList =
            {
                8, 6, 7, 0, 0, 5, 2, 3, 4, 0, 0,
                1, 0, 0, 0, 0, 0
            };

            int[] objCutPredictorIndex =
            {
                2, 2, 2, 0, 0, 1, 2, 2,
                2, 0, 0, 2, 0, 0, 0, 0, 0
            };

            bool[] objNanCutPoints =
            {
                false, false, false, true,
                true, false, false, false, false, true, true, false, true, true, true, true,
                true
            };

            double[] objNodeMean =
            {
                1.499999999999992,
                0.42016806722688976, 2.3184713375796075, 0.0, 1.0, 1.885869565217392,
                2.930769230769235, 1.6122448979591826, 2.1976744186046524,
                2.6086956521739122, 3.0000000000000049, 1.3214285714285727, 2.0, 2.0,
                2.9999999999999982, 1.0, 1.4736842105263159
            };

            double[] objCutPoint =
            {
                6.5, 4.5, 11.5, 0.0, 0.0, 0.5, 12.5,
                8.5, 7.5, 0.0, 0.0, 7.5, 0.0, 0.0, 0.0, 0.0, 0.0
            };

            int[] objChildren =
            {
                2, 3, 4, 5, 6, 7, 0, 0, 0, 0, 8,
                9, 10, 11, 12, 13, 14, 15, 0, 0, 0, 0, 16, 17, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
            };

            var index = 0;

            while (!(objPruneList[index] <= 0 || double.IsNaN(predictionInput[objCutPredictorIndex[index] - 1]) ||
                     objNanCutPoints[index]))
                if (predictionInput[objCutPredictorIndex[index] - 1] < objCutPoint[index])
                    index = objChildren[index << 1] - 1;
                else
                    index = objChildren[(index << 1) + 1] - 1;

            return objNodeMean[index];
        }

        public double SetInitialtargets(double targetItemId, double bodyFatCategory, double bmiCategory,
            double bgStatus)
        {
            double target;
            int bIndex;
            var healthySnacking = 0;
            var unhealthySnacking = 0;
            var calorieDrinks = 0;
            var breakfast = 0;
            var lunch = 0;
            var dinner = 0;
            var fasting = 0;
            var exercise = 0;

            // if ((body_fat_category>=2 && body_fat_category<=3) || (BMI_category>=2 && BMI_category<=5) || (BGStatus>=2&&BGStatus<=3)) 
            if (bodyFatCategory >= 2.0 || bmiCategory >= 2.0 || bgStatus >= 2.0)
            {
                //  Healthy_Snacking = "light";
                //  Unhealthy_Snacking = "avoid";
                //  Calorie_drinks = "avoid";
                //  Breakfast = "light";
                //  Lunch = "light";
                //  Dinner = "light";
                //  Fasting = "heavy";
                //  Exercise = "heavy";
                healthySnacking = 25;
                unhealthySnacking = 5;
                breakfast = 25;
                lunch = 25;
                dinner = 25;
                fasting = 75;
                exercise = 75;
            }
            else if (bodyFatCategory >= 1.0 || bmiCategory >= 1.0 || bgStatus >=
                1.0)
            {
                //  Healthy_Snacking = "light";
                //  Unhealthy_Snacking = "avoid";
                //  Calorie_drinks = "light";
                //  Breakfast = "moderate";
                //  Lunch = "moderate";
                //  Dinner = "moderate";
                //  Fasting = "moderate";
                //  Exercise = "moderate";
                healthySnacking = 25;
                unhealthySnacking = 5;
                calorieDrinks = 25;
                breakfast = 50;
                lunch = 50;
                dinner = 50;
                fasting = 50;
                exercise = 50;
            }
            else
            {
                if (bodyFatCategory <= 1.0 || bmiCategory <= 1.0 || bgStatus <= 1.0)
                {
                    //  Healthy_Snacking = "moderate";
                    //  Unhealthy_Snacking = "avoid";
                    //  Calorie_drinks = "moderate";
                    //  Breakfast = "heavy";
                    //  Lunch = "heavy";
                    //  Dinner = "heavy";
                    //  Fasting = "light";
                    //  Exercise = "light";
                    healthySnacking = 50;
                    unhealthySnacking = 5;
                    calorieDrinks = 50;
                    breakfast = 75;
                    lunch = 75;
                    dinner = 75;
                    fasting = 25;
                    exercise = 25;
                }
            }

            switch (targetItemId)
            {
                //  targets_name = {"Healthy_Snacking", "Unhealthy_Snacking", "Calorie_drinks", "Breakfast", "Lunch", "Dinner", "Fasting", "Exercise"}; 
                case 1.0:
                    bIndex = 0;
                    break;
                case 2.0:
                    bIndex = 1;
                    break;
                case 3.0:
                    bIndex = 2;
                    break;
                case 4.0:
                    bIndex = 3;
                    break;
                case 5.0:
                    bIndex = 4;
                    break;
                case 6.0:
                    bIndex = 5;
                    break;
                case 7.0:
                    bIndex = 6;
                    break;
                case 8.0:
                    bIndex = 7;
                    break;
                default:
                    bIndex = -1;
                    break;
            }

            switch (bIndex)
            {
                case 0:
                    target = healthySnacking;
                    break;

                case 1:
                    target = unhealthySnacking;
                    break;

                case 2:
                    target = calorieDrinks;
                    break;

                case 3:
                    target = breakfast;
                    break;

                case 4:
                    target = lunch;
                    break;

                case 5:
                    target = dinner;
                    break;

                case 6:
                    target = fasting;
                    break;

                case 7:
                    target = exercise;
                    break;

                default:
                    target = 0.0;
                    break;
            }

            return target;
        }

        public double GetLifeStyle(double activityType, double activityDuration, double
            activityIntensity)
        {
            double[] bActivityType = { };

            bActivityType[0] = activityType;
            bActivityType[1] = activityDuration;
            bActivityType[2] = activityIntensity;

            return CompactRegressionTree_LifeStylePredict(bActivityType);
        }

        private double CompactRegressionTree_LifeStylePredict(double[] predictionInput)
        {
            int[] objPruneList = { 2, 1, 0, 0, 0 };

            int[] objCutPredictorIndex = { 2, 3, 0, 0, 0 };

            bool[] objNanCutPoints = { false, false, true, true, true };

            double[] objNodeMean = { 1.3636363636363573, 0.833333333333333, 2.0, 0.0, 1.0 };

            double[] objCutPoint = { 30.5, 0.5, 0.0, 0.0, 0.0 };

            int[] objChildren = { 2, 3, 4, 5, 0, 0, 0, 0, 0, 0 };

            var index = 0;

            while (!(objPruneList[index] <= 0 || double.IsNaN(predictionInput[objCutPredictorIndex[index] - 1]) ||
                     objNanCutPoints[index]))
                if (predictionInput[objCutPredictorIndex[index] - 1] < objCutPoint[index])
                    index = objChildren[index << 1] - 1;
                else
                    index = objChildren[(index << 1) + 1] - 1;

            return objNodeMean[index];
        }
        // Function Definitions

        //
        // val_Healthy_Snacking [0-100%]
        //  val_Unhealthy_Snacking  [0-100%]
        //  val_Water [cups] i.e. how much water the user consumed
        //  val_recWaterPerday [cups] total recommended for the user. this is the
        //  output from the WaterAdvisory_Model(weight)
        //  i.e 4cups
        //  val_Calorie_drinks  [0-100%]
        //  val_Breakfast [0-100%]
        //  val_Lunch [0-100%]
        //  val_Dinner  [0-100%]
        //  val_Fasting  [0-100%]
        //  val_Exercise  [0-100%]
        // Arguments    : double val_Healthy_Snacking
        //                double val_Unhealthy_Snacking
        //                double val_Water
        //                double val_recWaterPerday
        //                double val_Calorie_drinks
        //                double val_Breakfast
        //                double val_Lunch
        //                double val_Dinner
        //                double val_Fasting
        //                double val_Exercise
        // Return Type  : double
        //

        public double GetNutriStatus(double valHealthySnacking, double
                valUnhealthySnacking, double valWater, double valRecWaterPerday, double
                valCalorieDrinks, double valBreakfast, double valLunch, double valDinner,
            double valFasting, double valExercise)
        {
            double[] nutriStautsInput = { };
            int k;

            //  val_Healthy_Snacking  = 0;
            //  val_Unhealthy_Snacking  = 0;
            //  val_Calorie_drinks  = 0;
            //  val_Breakfast = 0;
            //  val_Lunch = 0;
            //  val_Dinner  = 0;
            //  val_Fasting  = 0;
            //  val_Exercise  = 0;
            //  val_Healthy_Snacking  = (val_Healthy_Snacking/100)*2;
            //  val_Unhealthy_Snacking  = (val_Unhealthy_Snacking/100)*2;
            //  val_Calorie_drinks  = (val_Calorie_drinks/100)*2;
            //  val_Breakfast = (val_Breakfast/100)*2;
            //  val_Lunch = (val_Lunch/100)*2;
            //  val_Dinner  = (val_Dinner/100)*2;
            //  val_Fasting  = (val_Fasting/100)*2;
            //  val_Exercise  = (val_Exercise/100)*2;
            nutriStautsInput[0] = valHealthySnacking;
            nutriStautsInput[1] = valUnhealthySnacking;
            nutriStautsInput[2] = valWater / valRecWaterPerday * 100.0;
            nutriStautsInput[3] = valCalorieDrinks;
            nutriStautsInput[4] = valBreakfast;
            nutriStautsInput[5] = valLunch;
            nutriStautsInput[6] = valDinner;
            nutriStautsInput[7] = valFasting;
            nutriStautsInput[8] = valExercise;
            var sumNutriScore = valHealthySnacking;

            for (k = 0; k < 8; k++) sumNutriScore += nutriStautsInput[k + 1];

            return CompactRegressionTree_NutriStatusPredict(sumNutriScore / 100.0 * 2.0 / 18.0 *
                                                            16.0);
        }

        private double CompactRegressionTree_NutriStatusPredict(double predictionInput)
        {
            int[] objPruneList = { 2, 1, 0, 0, 0 };

            bool[] objNanCutPoints = { false, false, true, true, true };

            double[] objNodeMean = { 1.0000000000000071, 0.50000000000000067, 2.0, 0.0, 1.0 };

            int[] objCutPoint = { 12, 8, 0, 0, 0 };

            int[] objChildren = { 2, 3, 4, 5, 0, 0, 0, 0, 0, 0 };

            var index = 0;

            while (!(objPruneList[index] <= 0 || double.IsNaN(predictionInput) || objNanCutPoints[index]))
                if (predictionInput < objCutPoint[index])
                    index = objChildren[index << 1] - 1;
                else
                    index = objChildren[(index << 1) + 1] - 1;

            return objNodeMean[index];
        }

        // Function Definitions

        //
        // weight (kg) - 0 - 150kg
        // Arguments    : double weight
        // Return Type  : double
        //

        public double GetWaterAdvisory(double weight)
        {
            return CompactRegressionTree_WaterAdvisoryPredict(weight);
        }

        private double CompactRegressionTree_WaterAdvisoryPredict(double predictionInput)
        {
            int[] objPruneList =
            {
                10, 8, 9, 6, 4, 7, 5, 2, 3, 3,
                0, 3, 4, 0, 0, 0, 1, 0, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0
            };

            bool[] objNanCutPoints =
            {
                false, false, false, false,
                false, false, false, false, false, false, true, false, false, true, true,
                true, false, true, true, true, true, true, true, true, false, true, true,
                true, true
            };

            double[] objNodeMean =
            {
                7.714285714285654, 3.9999999999999885,
                11.42857142857139, 2.4999999999999982, 6.0000000000000115,
                9.9999999999999964, 15.000000000000004, 1.500000000000002,
                3.5000000000000013, 5.499999999999992, 6.9999999999999938,
                8.5000000000000142, 11.000000000000046, 13.999999999999988, 16.0, 1.0,
                1.8620689655172413, 2.9999999999999942, 4.0, 4.9999999999999893,
                5.9999999999999885, 8.0, 8.9999999999999911, 9.9999999999999787,
                11.499999999999986, 1.5, 2.0, 10.99999999999998, 11.999999999999977
            };

            double[] objCutPoint =
            {
                63.0, 36.5, 108.5, 18.5, 54.5, 81.5,
                127.5, 13.5, 27.5, 45.5, 0.0, 72.5, 90.5, 0.0, 0.0, 0.0, 14.5, 0.0, 0.0, 0.0,
                0.0, 0.0, 0.0, 0.0, 100.0, 0.0, 0.0, 0.0, 0.0
            };

            int[] objChildren =
            {
                2, 3, 4, 5, 6, 7, 8, 9, 10, 11,
                12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 0, 0, 22, 23, 24, 25, 0, 0, 0, 0, 0,
                0, 26, 27, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 28, 29, 0, 0, 0, 0, 0,
                0, 0, 0
            };

            var index = 0;

            while (!(objPruneList[index] <= 0 || double.IsNaN(predictionInput) || objNanCutPoints[index]))
                if (predictionInput < objCutPoint[index])
                    index = objChildren[index << 1] - 1;
                else
                    index = objChildren[(index << 1) + 1] - 1;

            return objNodeMean[index];
        }

        public int GetAgeCategory(int age)
        {
            int ageCategory = 0;

            if (age >= 20 && age <= 40)
            {
                ageCategory = 0;
            }
            else if (age >= 41 && age <= 60)
            {
                ageCategory = 1;
            }
            else if (age >= 61 && age <= 79)
            {
                ageCategory = 2;
            }

            return ageCategory;
        }

        public BodyMassType GetBodyMassType(double bodyMassIndex)
        {
            BodyMassType bodyMassType = BodyMassType.Underweight;

            if (bodyMassIndex < 18.5)
            {
                bodyMassType = BodyMassType.Underweight;
            }
            else if (bodyMassIndex <= 24.9)
            {
                bodyMassType = BodyMassType.Normal;
            }
            else if (bodyMassIndex <= 29.9)
            {
                bodyMassType = BodyMassType.Overweight;
            }
            else if (bodyMassIndex <= 34.9)
            {
                bodyMassType = BodyMassType.ObesityClass1;
            }
            else if (bodyMassIndex <= 39.9)
            {
                bodyMassType = BodyMassType.ObesityClass2;
            }
            else if (bodyMassIndex >= 40)
            {
                bodyMassType = BodyMassType.ObesityClass3;
            }


            return bodyMassType;
        }

        public BloodGlucoseType GetBloodGlucoseType(int fastingStatus, double sugarLevelMmol)
        {
            BloodGlucoseType bloodGlucoseType = BloodGlucoseType.Low;

            switch (fastingStatus)
            {
                case 0:
                    if (sugarLevelMmol <= 4)
                    {
                        bloodGlucoseType = BloodGlucoseType.Low;
                    }
                    else if (sugarLevelMmol > 4 && sugarLevelMmol <= 6)
                    {
                        bloodGlucoseType = BloodGlucoseType.Normal;
                    }
                    else if (sugarLevelMmol > 6.2 && sugarLevelMmol <= 7)
                    {
                        bloodGlucoseType = BloodGlucoseType.PerDiabetes;
                    }
                    else if (sugarLevelMmol > 7 && sugarLevelMmol <= 21)
                    {
                        bloodGlucoseType = BloodGlucoseType.Diabetes;
                    }

                    break;
                case 1:
                    if (sugarLevelMmol <= 4)
                    {
                        bloodGlucoseType = BloodGlucoseType.Low;
                    }
                    else if (sugarLevelMmol > 4 && sugarLevelMmol <= 8)
                    {
                        bloodGlucoseType = BloodGlucoseType.Normal;
                    }
                    else if (sugarLevelMmol > 8 && sugarLevelMmol <= 11.5)
                    {
                        bloodGlucoseType = BloodGlucoseType.PerDiabetes;
                    }
                    else if (sugarLevelMmol > 11 && sugarLevelMmol <= 21)
                    {
                        bloodGlucoseType = BloodGlucoseType.Diabetes;
                    }

                    break;
            }

            return bloodGlucoseType;
        }

        public BodyFatType GetBodyFatType(int ageCategory, GenderBiologicalType genderCategory, double bodyFatPercentage)
        {
            var bodyFatType = BodyFatType.UnderFat;

            switch (genderCategory)
            {
                case GenderBiologicalType.Male:
                case GenderBiologicalType.NonBinary:
                case GenderBiologicalType.Undefined:
                    switch (ageCategory)
                    {
                        case 0:
                            if (bodyFatPercentage >= 0 && bodyFatPercentage <= 21)
                            {
                                bodyFatType = BodyFatType.UnderFat;
                            }
                            else if (bodyFatPercentage > 21 && bodyFatPercentage <= 33)
                            {
                                bodyFatType = BodyFatType.Healthy;
                            }
                            else if (bodyFatPercentage > 33 && bodyFatPercentage <= 39)
                            {
                                bodyFatType = BodyFatType.Overweight;
                            }
                            else if (bodyFatPercentage > 39 && bodyFatPercentage <= 100)
                            {
                                bodyFatType = BodyFatType.Obese;
                            }

                            break;
                        case 1:
                            if (bodyFatPercentage >= 0 && bodyFatPercentage <= 23)
                            {
                                bodyFatType = BodyFatType.UnderFat;
                            }
                            else if (bodyFatPercentage > 23 && bodyFatPercentage <= 35)
                            {
                                bodyFatType = BodyFatType.Healthy;
                            }
                            else if (bodyFatPercentage > 35 && bodyFatPercentage <= 40)
                            {
                                bodyFatType = BodyFatType.Overweight;
                            }
                            else if (bodyFatPercentage > 40 && bodyFatPercentage <= 100)
                            {
                                bodyFatType = BodyFatType.Obese;
                            }

                            break;
                        case 2:
                            if (bodyFatPercentage >= 0 && bodyFatPercentage <= 24)
                            {
                                bodyFatType = BodyFatType.UnderFat;
                            }
                            else if (bodyFatPercentage > 24 && bodyFatPercentage <= 36)
                            {
                                bodyFatType = BodyFatType.Healthy;
                            }
                            else if (bodyFatPercentage > 36 && bodyFatPercentage <= 42)
                            {
                                bodyFatType = BodyFatType.Overweight;
                            }
                            else if (bodyFatPercentage > 42 && bodyFatPercentage <= 100)
                            {
                                bodyFatType = BodyFatType.Obese;
                            }

                            break;
                    }

                    break;
                case GenderBiologicalType.Female:

                    switch (ageCategory)
                    {
                        case 0:
                            if (bodyFatPercentage >= 0 && bodyFatPercentage <= 8)
                            {
                                bodyFatType = BodyFatType.UnderFat;
                            }
                            else if (bodyFatPercentage > 8 && bodyFatPercentage <= 19)
                            {
                                bodyFatType = BodyFatType.Healthy;
                            }
                            else if (bodyFatPercentage > 19 && bodyFatPercentage <= 25)
                            {
                                bodyFatType = BodyFatType.Overweight;
                            }
                            else if (bodyFatPercentage > 25 && bodyFatPercentage <= 100)
                            {
                                bodyFatType = BodyFatType.Obese;
                            }

                            break;
                        case 1:
                            if (bodyFatPercentage >= 0 && bodyFatPercentage <= 11)
                            {
                                bodyFatType = BodyFatType.UnderFat;
                            }
                            else if (bodyFatPercentage > 11 && bodyFatPercentage <= 22)
                            {
                                bodyFatType = BodyFatType.Healthy;
                            }
                            else if (bodyFatPercentage > 22 && bodyFatPercentage <= 27)
                            {
                                bodyFatType = BodyFatType.Overweight;
                            }
                            else if (bodyFatPercentage > 27 && bodyFatPercentage <= 100)
                            {
                                bodyFatType = BodyFatType.Obese;
                            }

                            break;
                        case 2:
                            if (bodyFatPercentage >= 0 && bodyFatPercentage <= 13)
                            {
                                bodyFatType = BodyFatType.UnderFat;
                            }
                            else if (bodyFatPercentage > 13 && bodyFatPercentage <= 25)
                            {
                                bodyFatType = BodyFatType.Healthy;
                            }
                            else if (bodyFatPercentage > 25 && bodyFatPercentage <= 30)
                            {
                                bodyFatType = BodyFatType.Overweight;
                            }
                            else if (bodyFatPercentage > 30 && bodyFatPercentage <= 100)
                            {
                                bodyFatType = BodyFatType.Obese;
                            }

                            break;
                    }

                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(genderCategory), genderCategory, null);
            }

            return bodyFatType;
        }

        public FoodAmountType FoodAmountFromPercentage(double targetPercentage)
        {
            FoodAmountType foodAmountType = FoodAmountType.VeryLight;

            if (targetPercentage == 0)
            {
                foodAmountType = FoodAmountType.None;
            }
            else if (targetPercentage >= 0 && targetPercentage < 25)
            {
                foodAmountType = FoodAmountType.VeryLight;
            }
            else if (targetPercentage >= 25 && targetPercentage < 50)
            {
                foodAmountType = FoodAmountType.Light;
            }
            else if (targetPercentage >= 50 && targetPercentage < 75)
            {
                foodAmountType = FoodAmountType.Medium;
            }
            else if (targetPercentage >= 75 && targetPercentage < 100)
            {
                foodAmountType = FoodAmountType.Heavy;
            }
            else if (targetPercentage >= 100)
            {
                foodAmountType = FoodAmountType.VeryHeavy;
            }

            return foodAmountType;
        }

        public double CalorieDrinkAmountFromPercentage(double targetPercentage)
        {
            const double maxCupAmount = 4;

            var targetCups = (targetPercentage / 100) * maxCupAmount;

            //Convert from cups to liters
            return Math.Round(targetCups) * 250 / 1000;
        }

        public double ActivityAmountFromPercentage(double targetPercentage)
        {
            TimeSpan hoursTimeSpan = TimeSpan.FromMinutes(20);
            return hoursTimeSpan.TotalHours;

            //const double maxActivityTime = 120;

            //var activityTime = (targetPercentage / 100) * maxActivityTime;

            //var targetMinutes = Math.Round(activityTime);

            ////convert to hours
            //TimeSpan hoursTimeSpan = TimeSpan.FromMinutes(targetMinutes);

            //return hoursTimeSpan.TotalHours;
        }

        //
        // sex
        //  men = 1, women = 0;
        //  height is in cm
        //  child_group = 2:1:19;
        //  adult_group = 20:1:64;
        //  older_adult = 65:1:100;
        // Arguments    : double age
        //                double height
        //                double optimal_weight_data[] (removed)
        //                int optimal_weight_size[2] (removed)
        // Return Type  : double
        public double Optimal_Weight(double age, double height)
        {
            double[] optimal_weight_data = new double[24];
            int[] optimal_weight_size = new int[2];
            double y;
            int idx;
            int ii;
            bool[] x = new bool[21];
            bool[] b_x = new bool[24];
            bool exitg1;
            char[] ii_data = new char[21];
            char[] b_ii_data = new char[24];
            double[] dv =
            {
                18.5, 18.82, 19.14, 19.46, 19.78, 20.1,
                20.419999999999998, 20.74, 21.06, 21.38, 21.7, 22.02, 22.34, 22.66,
                22.979999999999997, 23.299999999999997, 23.619999999999997,
                23.939999999999998, 24.259999999999998, 24.58, 24.9
            };

            double[] dv1 =
            {
                24.9, 25.021739130434781, 25.143478260869564,
                25.265217391304347, 25.38695652173913, 25.508695652173913,
                25.630434782608695, 25.752173913043478, 25.873913043478261,
                25.995652173913044, 26.117391304347827, 26.239130434782609,
                26.360869565217392, 26.482608695652171, 26.604347826086954,
                26.726086956521737, 26.84782608695652, 26.969565217391303,
                27.091304347826085, 27.213043478260868, 27.334782608695651,
                27.456521739130434, 27.578260869565216, 27.7
            };

            //  child_group = 20;
            //  adult_group = 24.9;
            //  older_adult = 27.7;
            //  optimal_bmi = adult_group;
            //  if age <=19
            //      optimal_bmi = child_group;
            //  if age>= 20 && age <=64
            //      optimal_bmi = adult_group;
            //  elseif age >= 65
            //      optimal_bmi = older_adult ;
            //  else
            //  end
            height /= 100.0;

            //  age_group1 = linspace(20, 40, 21);
            //  age_group2 = linspace(41, 64, 24);
            //  bmi_group1 = linspace(18.5, 24.9, 21);  % for age 20 - 40 years
            //  bmi_group2 = linspace(24.9, 27.7, 24);  % for age 41 - 64 years
            //  adult_group = 24.9;
            y = 20.0;
            for (idx = 0; idx < 20; idx++)
            {
                y += (((idx) + 2.0) - 1.0) + 20.0;
            }

            optimal_weight_size[1] = 1;
            optimal_weight_data[0] = y / 21.0;

            //  height = height/100;
            //  if age <=19
            //      optimal_bmi = child_group;
            if ((age >= 20.0) && (age <= 40.0))
            {
                //      optimal_bmi = adult_group;
                for (ii = 0; ii < 21; ii++)
                {
                    x[ii] = ((ii) + 20.0 == age);
                }

                idx = 0;
                ii = 0;
                exitg1 = false;
                while ((!exitg1) && (ii < 21))
                {
                    if (x[ii])
                    {
                        idx++;
                        ii_data[idx - 1] = (char)((ii + 1));
                        if (idx >= 21)
                        {
                            exitg1 = true;
                        }
                        else
                        {
                            ii++;
                        }
                    }
                    else
                    {
                        ii++;
                    }
                }

                if (1 > idx)
                {
                    idx = 0;
                }

                optimal_weight_size[1] = idx;
                for (ii = 0; ii < idx; ii++)
                {
                    optimal_weight_data[ii] = dv[ii_data[ii] - 1];
                }
            }
            else if ((age >= 41.0) && (age <= 64.0))
            {
                //      optimal_bmi = adult_group;
                for (ii = 0; ii < 24; ii++)
                {
                    b_x[ii] = ((ii) + 41.0 == age);
                }

                idx = 0;
                ii = 0;
                exitg1 = false;
                while ((!exitg1) && (ii < 24))
                {
                    if (b_x[ii])
                    {
                        idx++;
                        b_ii_data[idx - 1] = (char)((ii + 1));
                        if (idx >= 24)
                        {
                            exitg1 = true;
                        }
                        else
                        {
                            ii++;
                        }
                    }
                    else
                    {
                        ii++;
                    }
                }

                if (1 > idx)
                {
                    idx = 0;
                }

                optimal_weight_size[1] = idx;
                for (ii = 0; ii < idx; ii++)
                {
                    optimal_weight_data[ii] = dv1[b_ii_data[ii] - 1];
                }
            }
            else
            {
                if (age >= 65.0)
                {
                    optimal_weight_size[1] = 1;
                    optimal_weight_data[0] = 27.7;
                }
            }

            optimal_weight_size[0] = 1;
            idx = optimal_weight_size[1] - 1;
            for (ii = 0; ii <= idx; ii++)
            {
                optimal_weight_data[ii] = optimal_weight_data[ii] * height * height;
                return optimal_weight_data[ii];
            }

            return optimal_weight_data[0];
        }

        // BMI is based on data from The Geriatric Dietitian
        //  link: https://www.thegeriatricdietitian.com/bmi-in-the-elderly/
        //  sex
        //  men = 1, women = 0;
        //  height is in cm
        // Arguments    : double age
        //                double optimal_bmi_data[] (removed)
        //                int optimal_bmi_size[2] (removed)
        // Return Type  : double
        public double Optimal_BMI(double age)
        {
            double[] optimal_bmi_data = new double[24];
            int[] optimal_bmi_size = new int[2];
            int idx;
            bool[] x = new bool[21];
            bool[] b_x = new bool[24];
            int ii;
            bool exitg1;
            char[] ii_data = new char[21];
            char[] b_ii_data = new char[24];

            double[] dv =
            {
                18.5, 18.82, 19.14, 19.46, 19.78, 20.1,
                20.419999999999998, 20.74, 21.06, 21.38, 21.7, 22.02, 22.34, 22.66,
                22.979999999999997, 23.299999999999997, 23.619999999999997,
                23.939999999999998, 24.259999999999998, 24.58, 24.9
            };

            double[] dv1 =
            {
                24.9, 25.021739130434781, 25.143478260869564,
                25.265217391304347, 25.38695652173913, 25.508695652173913,
                25.630434782608695, 25.752173913043478, 25.873913043478261,
                25.995652173913044, 26.117391304347827, 26.239130434782609,
                26.360869565217392, 26.482608695652171, 26.604347826086954,
                26.726086956521737, 26.84782608695652, 26.969565217391303,
                27.091304347826085, 27.213043478260868, 27.334782608695651,
                27.456521739130434, 27.578260869565216, 27.7
            };

            optimal_bmi_size[0] = 1;
            optimal_bmi_size[1] = 1;
            optimal_bmi_data[0] = 21.7;

            //  age_group1 = linspace(20, 40, 21);
            //  age_group2 = linspace(41, 64, 24);
            //  bmi_group1 = linspace(18.5, 24.9, 21);  % for age 20 - 40 years
            //  bmi_group2 = linspace(24.9, 27.7, 24);  % for age 41 - 64 years
            if ((age >= 20.0) && (age <= 40.0))
            {
                //      optimal_bmi = adult_group;
                for (idx = 0; idx < 21; idx++)
                {
                    x[idx] = ((idx) + 20.0 == age);
                }

                idx = 0;
                ii = 0;
                exitg1 = false;
                while ((!exitg1) && (ii < 21))
                {
                    if (x[ii])
                    {
                        idx++;
                        ii_data[idx - 1] = (char)(ii + 1);
                        if (idx >= 21)
                        {
                            exitg1 = true;
                        }
                        else
                        {
                            ii++;
                        }
                    }
                    else
                    {
                        ii++;
                    }
                }

                ii = 1 > idx ? 0 : idx;

                optimal_bmi_size[0] = 1;
                optimal_bmi_size[1] = ii;
                for (idx = 0; idx < ii; idx++)
                {
                    optimal_bmi_data[idx] = dv[ii_data[idx] - 1];
                    return optimal_bmi_data[idx];
                }
            }
            else if ((age >= 41.0) && (age <= 64.0))
            {
                //      optimal_bmi = adult_group;
                for (idx = 0; idx < 24; idx++)
                {
                    b_x[idx] = (idx + 41.0 == age);
                }

                idx = 0;
                ii = 0;
                exitg1 = false;
                while ((!exitg1) && (ii < 24))
                {
                    if (b_x[ii])
                    {
                        idx++;
                        b_ii_data[idx - 1] = (char)((ii + 1));
                        if (idx >= 24)
                        {
                            exitg1 = true;
                        }
                        else
                        {
                            ii++;
                        }
                    }
                    else
                    {
                        ii++;
                    }
                }

                ii = 1 > idx ? 0 : idx;

                optimal_bmi_size[0] = 1;
                optimal_bmi_size[1] = ii;
                for (idx = 0; idx < ii; idx++)
                {
                    optimal_bmi_data[idx] = dv1[b_ii_data[idx] - 1];
                    return optimal_bmi_data[idx];
                }
            }
            else
            {
                if (age >= 65.0)
                {
                    optimal_bmi_size[0] = 1;
                    optimal_bmi_size[1] = 1;
                    optimal_bmi_data[0] = 27.7;
                    return optimal_bmi_data[0];
                }
            }

            return optimal_bmi_data[0];
        }
    }
}
