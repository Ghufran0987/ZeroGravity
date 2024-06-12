using System;
using System.Globalization;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class ZeroToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int inputValue)
            {
                if (inputValue == 0)
                    return false;
                else
                    return true;
            }
            else if (value is string inputValue1)
            {
                if (inputValue1 == string.Empty || string.IsNullOrEmpty(inputValue1) || string.IsNullOrWhiteSpace(inputValue1) || inputValue1 == "0")
                    return false;
                else
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