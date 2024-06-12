using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resources.Fonts;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class BooleanToBadgeTextColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = CustomColors.Black;

            if (value == null)
            {
                return color;
            }

            var booleanValue = (bool)value;

            if (booleanValue)
            {
                color = CustomColors.White;
                return color;
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
