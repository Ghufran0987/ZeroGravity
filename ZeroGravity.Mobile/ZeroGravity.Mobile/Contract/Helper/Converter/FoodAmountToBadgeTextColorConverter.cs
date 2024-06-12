using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resources.Fonts;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class FoodAmountToBadgeTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = CustomColors.White;

            if (value == null)
            {
                return color;
            }

            var stringValue = value.ToString();

            if (Enum.TryParse(stringValue, out FoodAmountType amountType))
            {
                switch (amountType)
                {
                    case FoodAmountType.Undefined:
                        // noch kein Eintrag vorhanden
                        color = CustomColors.Black;
                        break;
                    default:
                        // Mahlzeit übersprungen + alle anderen
                        color = CustomColors.White;
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
