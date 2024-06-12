using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Shared.Constants;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class RangeThresholdColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), out double valueDouble) && double.TryParse(parameter.ToString(), out double paramDouble))
            {
                return paramDouble >= valueDouble ? Color.Red : Color.LimeGreen;
            }

            return Color.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}