using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class DebugConverter : IMarkupExtension, IValueConverter
    {
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return new DebugConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
