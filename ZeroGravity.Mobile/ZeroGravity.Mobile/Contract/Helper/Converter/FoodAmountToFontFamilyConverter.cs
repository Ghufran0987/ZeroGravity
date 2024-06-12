using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resources.Fonts;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class FoodAmountToFontFamilyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string fontFamily = CustomFontName.OpenSanRegular;

            if (value != null)
            {
                var stringValue = value.ToString();

                if (Enum.TryParse(stringValue, out FoodAmountType amountType))
                {
                    switch (amountType)
                    {
                        case FoodAmountType.Undefined:
                            // Zero (old "Sync/Replay Icon") => use Text FontFamily
                            fontFamily = CustomFontName.OpenSanRegular;
                            break;
                        
                        default:
                            //=> use Icon FontFamily
                            fontFamily = CustomFontName.FaLight300;
                            break;
                    }
                }

            }

            return fontFamily;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
