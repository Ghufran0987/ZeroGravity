using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class FoodAmountToIconStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var amountIconString = string.Empty;

            if (value != null)
            {
                var stringValue = value.ToString();

                if (Enum.TryParse(stringValue, out FoodAmountType amountType))
                {
                    string mealType = (string)parameter;

                    switch (amountType)
                    {
                        case FoodAmountType.Undefined:
                            // Zero (old "Sync/Replay Icon")
                            amountIconString = "0";
                            break;
                        case FoodAmountType.None:
                            if (mealType == null)
                            {
                                // "Doppelpfeil Icon"
                                amountIconString = "\uf04e";
                            }
                            else
                            {
                                if (mealType == AppResources.MealsSnacksHealthySnack_Title ||
                                    mealType == AppResources.MealsSnacksUnhealthySnack_Title)
                                {
                                    // "VeryLight Blatt Icon"
                                    amountIconString = "\uf06c";
                                }
                                else
                                {
                                    // "X Icon"
                                    amountIconString = "\uf00d";
                                }
                            }
                            break;
                        case FoodAmountType.VeryLight:
                            amountIconString = "\uf06c";
                            break;
                        case FoodAmountType.Light:
                            amountIconString = "\uf06c";
                            break;
                        case FoodAmountType.Medium:
                            amountIconString = "\uf24e";
                            break;
                        case FoodAmountType.Heavy:
                            amountIconString = "\uf5cd";
                            break;
                        case FoodAmountType.VeryHeavy:
                            amountIconString = "\uf5cd";
                            break;
                        default:
                            amountIconString = "\uf06c";
                            break;
                    }
                }

            }

            return amountIconString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}