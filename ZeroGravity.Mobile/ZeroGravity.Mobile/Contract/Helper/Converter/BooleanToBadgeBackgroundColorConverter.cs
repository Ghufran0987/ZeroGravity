using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resources.Fonts;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class BooleanToBadgeBackgroundColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var color = CustomColors.Yellow;

            if (value == null)
            {
                return color;
            }

            var booleanValue = (bool)value;

            if (booleanValue)
            {
                color = CustomColors.Green;
                return color;
            }

            return color;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = (int)value;

            if (booleanValue == 0)
            {
                return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IntToBooleanNegateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var booleanValue = (int)value;

            if (booleanValue == 0)
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