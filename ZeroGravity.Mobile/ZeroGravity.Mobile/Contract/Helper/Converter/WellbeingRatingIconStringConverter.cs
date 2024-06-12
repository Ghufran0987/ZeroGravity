using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class WellbeingRatingIconStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var ratingIconString = string.Empty;

            if (value != null)
            {
                var stringValue = value.ToString();

                if (Enum.TryParse(stringValue, out WellbeingType wellbeingType))
                    switch (wellbeingType)
                    {
                        case WellbeingType.VeryBad:
                            ratingIconString = "\uf5b4";
                            break;
                        case WellbeingType.Bad:
                            ratingIconString = "\uf119";;
                            break;
                        case WellbeingType.NotSoGreat:
                            ratingIconString = "\uf11a";;
                            break;
                        case WellbeingType.Great:
                            ratingIconString = "\uf118";;
                            break;
                        case WellbeingType.Fantastic:
                            ratingIconString = "\uf59a";;
                            break;
                        default:
                            ratingIconString = "\uf5b4";;
                            break;
                    }
            }

            return ratingIconString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
