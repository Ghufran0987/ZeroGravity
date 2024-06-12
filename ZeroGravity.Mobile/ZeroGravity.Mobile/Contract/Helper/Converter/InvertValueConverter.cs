using System;
using System.Globalization;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class InvertValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var d = (double) value;
                return -d;
            }
            catch
            {
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Binding.DoNothing;
        }
    }
}
