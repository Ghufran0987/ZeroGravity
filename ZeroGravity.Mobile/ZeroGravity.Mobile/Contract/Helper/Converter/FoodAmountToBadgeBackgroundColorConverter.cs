using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resources.Fonts;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class FoodAmountToBadgeBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = CustomColors.Green;

            if (value == null)
            {
                return color;
            }

            var stringValue = value.ToString();

            if (Enum.TryParse(stringValue, out FoodAmountType amountType))
            {
                string mealType = (string)parameter;

                switch (amountType)
                {
                    case FoodAmountType.Undefined:
                        // noch kein Eintrag vorhanden
                        color = CustomColors.Yellow;
                        break;
                    case FoodAmountType.None:
                        // Mahlzeit übersprungen
                        color = CustomColors.Red;
                        break;
                    default:
                        // alle anderen
                        color = CustomColors.Green;
                        break;
                }
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
