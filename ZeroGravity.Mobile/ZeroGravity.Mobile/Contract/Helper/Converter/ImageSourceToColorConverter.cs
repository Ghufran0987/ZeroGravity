using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resources.Fonts;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class ImageSourceToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return CustomColors.DarkGray;
            }

            return CustomColors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
