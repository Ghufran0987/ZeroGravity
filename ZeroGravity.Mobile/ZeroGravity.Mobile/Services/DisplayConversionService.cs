using System;
using ZeroGravity.Mobile.Interfaces;
using ZeroGravity.Shared.Enums;
using ZeroGravity.Shared.Models;

namespace ZeroGravity.Mobile.Services
{
    public static class DisplayConversionService
    {
        private const double InchConstant = 2.54;
        private const double PoundConstant = 2.205;
        private const double FluidOuncesConstant = 33.814;
        private const double FluidGallonConstant = 3.785;

        private static readonly DisplayPrefences DisplayPrefences = new DisplayPrefences();

        public static void SetDisplayPrefences(DateTimeDisplayType dateTimeDisplayType, UnitDisplayType unitDisplayType)
        {
            DisplayPrefences.DateTimeDisplayType = dateTimeDisplayType;
            DisplayPrefences.UnitDisplayType = unitDisplayType;
        }

        public static object ConvertFluidGal(double fluid, UnitDisplayType unitDisplayType)
        {
            double targetWeightValue;

            switch (unitDisplayType)
            {
                case UnitDisplayType.Imperial:

                    //liter in flgal
                    targetWeightValue = Math.Round(fluid / FluidGallonConstant, 2);
                    break;
                case UnitDisplayType.Metric:

                    //flgal in liter
                    targetWeightValue = Math.Round(fluid * FluidGallonConstant, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unitDisplayType), unitDisplayType, null);
            }

            return targetWeightValue;
        }

        public static DisplayPrefences GetDisplayPrefences()
        {
            return DisplayPrefences;
        }

        /// <summary>
        ///     Converts the height value from cm to inches and back
        /// </summary>
        /// <param name="length"></param>
        /// <param name="unitDisplayType"></param>
        /// <returns></returns>
        public static double ConvertLength(double length, UnitDisplayType unitDisplayType)
        {
            double targetHeightValue;

            switch (unitDisplayType)
            {
                case UnitDisplayType.Imperial:

                    //cm in inches
                    targetHeightValue = Math.Round(length / InchConstant, 2);
                    break;
                case UnitDisplayType.Metric:
                    //inches in cm
                    targetHeightValue = Math.Round(length * InchConstant, 2);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unitDisplayType), unitDisplayType, null);
            }

            return targetHeightValue;
        }

        public static double ConvertWeight(double weight, UnitDisplayType unitDisplayType)
        {
            double targetWeightValue;

            switch (unitDisplayType)
            {
                case UnitDisplayType.Imperial:

                    //kg in pounds
                    targetWeightValue = Math.Round(weight * PoundConstant, 2);
                    break;
                case UnitDisplayType.Metric:

                    //pounds in kg
                    targetWeightValue = Math.Round(weight / PoundConstant, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unitDisplayType), unitDisplayType, null);
            }

            return targetWeightValue;
        }

        internal static double ConvertCupsToLtr(double fluidInCups)
        {
            double cups = 0;
            if (fluidInCups > 0)
            {
                cups = (fluidInCups * 250) / 1000;
            }
            return cups;
        }

        public static double ConvertFluidOz(double fluid, UnitDisplayType unitDisplayType)
        {
            double targetWeightValue;

            switch (unitDisplayType)
            {
                case UnitDisplayType.Imperial:

                    //liter in floz
                    targetWeightValue = Math.Round(fluid * FluidOuncesConstant, 2);
                    break;
                case UnitDisplayType.Metric:

                    //floz in liter
                    targetWeightValue = Math.Round(fluid / FluidOuncesConstant, 0);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(unitDisplayType), unitDisplayType, null);
            }

            return targetWeightValue;
        }

        public static double ConvertLtrToCups(double fluidInMl)
        {
            double cups = 0;
            if (fluidInMl > 0)
            {
                cups = (fluidInMl * 1000) / 250;
            }
            return cups;
        }

        public static string GetTimeDisplayFormat(DateTimeDisplayType dateTimeDisplayType)
        {
            string targetFormat;

            switch (dateTimeDisplayType)
            {
                case DateTimeDisplayType.Show12HourDay:
                    targetFormat = "h:mm tt";
                    break;
                case DateTimeDisplayType.Show24HourDay:
                    targetFormat = "HH:mm";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(dateTimeDisplayType), dateTimeDisplayType, null);
            }

            return targetFormat;
        }

        public static string GetTimeDisplayFormat()
        {
            string targetFormat;

            switch (DisplayPrefences.DateTimeDisplayType)
            {
                case DateTimeDisplayType.Show12HourDay:
                    targetFormat = "h:mm tt";
                    break;
                case DateTimeDisplayType.Show24HourDay:
                    targetFormat = "HH:mm";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(DisplayPrefences.DateTimeDisplayType), DisplayPrefences.DateTimeDisplayType, null);
            }

            return targetFormat;
        }

        public static double ConvertOZToMl(double fluidinOz)
        {
            double retvalue;

            // 1ml = 0.034 oz
            retvalue = fluidinOz * 29.57;

            return retvalue;
        }


        public static double ConvertKgToStone(double kg)
        {
            var stone = kg * 0.157473;
            return stone;
        }

        public static double ConvertKgToPound(double kg)
        {
            var pound = kg * 2.2047;
            return pound;
        }

        public static double ConvertStoneToKg(double stone)
        {
            var kg = stone * 6.351;
            return kg;
        }
        public static double ConvertPoundToKg(double pound)
        {
            var kg = pound * 0.4536;
            return kg;
        }



    }
}