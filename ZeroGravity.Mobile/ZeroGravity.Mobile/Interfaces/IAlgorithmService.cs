using System;
using System.Collections.Generic;
using System.Text;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Interfaces
{
    public interface IAlgorithmService
    {
        double CalculateBmi(double height, double weight);
        double CalculateBodyFatPercentage(GenderBiologicalType sex, double waist, double hip, double neck, double height);
        double GetBloodGlucose(double sugarlevel, double fasting);

        double SetInitialtargets(double targetItemId, double bodyFatCategory, double bmiCategory,
            double bgStatus);

        double GetLifeStyle(double activityType, double activityDuration, double
            activityIntensity);

        double GetNutriStatus(double valHealthySnacking, double
                valUnhealthySnacking, double valWater, double valRecWaterPerday, double
                valCalorieDrinks, double valBreakfast, double valLunch, double valDinner,
            double valFasting, double valExercise);

        double GetWaterAdvisory(double weight);
        BodyMassType GetBodyMassType(double bodyMassIndex);
        BloodGlucoseType GetBloodGlucoseType(int fastingStatus, double sugarLevelMmol);
        BodyFatType GetBodyFatType(int ageCategory, GenderBiologicalType genderCategory, double bodyFatPercentage);
        int GetAgeCategory(int age);
        FoodAmountType FoodAmountFromPercentage(double targetPercentage);
        double CalorieDrinkAmountFromPercentage(double targetPercentage);
        double ActivityAmountFromPercentage(double targetPercentage);
    }
}
