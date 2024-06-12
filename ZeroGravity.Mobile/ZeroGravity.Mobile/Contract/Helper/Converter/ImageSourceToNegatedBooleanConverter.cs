using System;
using System.Globalization;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class ImageSourceToNegatedBooleanConverter :  IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
