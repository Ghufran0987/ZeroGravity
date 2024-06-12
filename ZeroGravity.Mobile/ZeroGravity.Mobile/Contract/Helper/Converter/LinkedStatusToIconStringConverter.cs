using System;
using System.Globalization;
using Xamarin.Forms;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class LinkedStatusToIconStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iconString = string.Empty;

            if (value != null)
            {
                var stringValue = value.ToString();

                if (bool.TryParse(stringValue, out var result))
                    switch (result)
                    {
                        case true:
                            iconString = "\uf0c1";
                            break;
                        case false:
                            iconString = "";
                            break;
                    }
            }

            return iconString;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}