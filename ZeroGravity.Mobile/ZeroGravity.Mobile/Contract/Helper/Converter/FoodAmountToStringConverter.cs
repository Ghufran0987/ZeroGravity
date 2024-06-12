using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class FoodAmountToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string foodAmountString = string.Empty;
            
            if(value != null)
            {
                var stringValue = value.ToString();

                if(Enum.TryParse(stringValue, out FoodAmountType amountType))
                {
                    string mealType = (string) parameter;

                    switch (amountType)
                    {
                        case FoodAmountType.None:
                            if (!string.IsNullOrEmpty(mealType))
                            {
                                if (mealType == AppResources.MealsSnacksBreakfast_Title)
                                {
                                    foodAmountString = AppResources.FastingSetting_SkipBreakfast;
                                }
                                else if (mealType == AppResources.MealsSnacksLunch_Title)
                                {
                                    foodAmountString = AppResources.FastingSetting_SkipLunch;
                                }
                                else if (mealType == AppResources.MealsSnacksDinner_Title)
                                {
                                    foodAmountString = AppResources.FastingSetting_SkipDinner;
                                }
                                else if (mealType == AppResources.Feedback_Title)
                                {
                                    foodAmountString = AppResources.FoodAmount_NotAvailable;
                                }
                            }
                            else
                            {
                                foodAmountString = AppResources.FoodAmount_VeryLight;
                            }
                            break;
                        case FoodAmountType.VeryLight:
                            foodAmountString = AppResources.FoodAmount_VeryLight;
                            break;
                        case FoodAmountType.Light:
                            foodAmountString = AppResources.FoodAmount_Light;
                            break;
                        case FoodAmountType.Medium:
                            foodAmountString = AppResources.FoodAmount_Medium;
                            break;
                        case FoodAmountType.Heavy:
                            foodAmountString = AppResources.FoodAmount_Heavy;
                            break;
                        case FoodAmountType.VeryHeavy:
                            foodAmountString = AppResources.FoodAmount_VeryHeavy;
                            break;
                        case FoodAmountType.Undefined:
                            foodAmountString = AppResources.FoodAmount_NotAvailable;
                            break;
                        default:
                            foodAmountString = AppResources.FoodAmount_VeryLight;
                            break;
                    }
                }
            }

            return foodAmountString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}