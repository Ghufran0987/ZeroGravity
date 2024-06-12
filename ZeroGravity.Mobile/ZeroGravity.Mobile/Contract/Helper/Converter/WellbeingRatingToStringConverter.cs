using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class WellbeingRatingToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var wellbeingString = string.Empty;

            if (value != null)
            {
                var stringValue = value.ToString();

                if (Enum.TryParse(stringValue, out WellbeingType wellbeingType))
                    switch (wellbeingType)
                    {
                        case WellbeingType.VeryBad:
                            wellbeingString = AppResources.WellbeingType_VeryBad;
                            break;
                        case WellbeingType.Bad:
                            wellbeingString = AppResources.WellbeingType_Bad;
                            break;
                        case WellbeingType.NotSoGreat:
                            wellbeingString = AppResources.WellbeingType_NotSoGreat;
                            break;
                        case WellbeingType.Great:
                            wellbeingString = AppResources.WellbeingType_Great;
                            break;
                        case WellbeingType.Fantastic:
                            wellbeingString = AppResources.WellbeingType_Fantastic;
                            break;
                        default:
                            wellbeingString = AppResources.WellbeingType_VeryBad;
                            break;
                    }
            }

            return wellbeingString;
        }


        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}