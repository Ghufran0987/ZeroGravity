using System;
using System.Globalization;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class VideoUploadProgressToFixedStringLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }

            var stringValue = value.ToString();

            if (stringValue.Length == 1)
            {
                stringValue = stringValue.Insert(0, "  ");
            }
            else if (stringValue.Length == 2)
            {
                stringValue = stringValue.Insert(0, " ");
            }

            stringValue = stringValue + "%";
            return stringValue;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
