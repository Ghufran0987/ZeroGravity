using System;
using System.Globalization;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class TextToSolidBrushConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string textColor)
            {
                return new SolidColorBrush(Color.FromHex(textColor));
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}