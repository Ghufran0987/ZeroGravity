using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Services;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class TimeDisplayTypeConverter : IMarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DisplayConversionService.GetTimeDisplayFormat();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return new TimeDisplayTypeConverter();
        }
    }
}