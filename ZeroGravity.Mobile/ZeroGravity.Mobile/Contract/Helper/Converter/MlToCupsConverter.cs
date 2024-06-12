using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Services;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    //public class MlToCupsConverter : IMarkupExtension, IValueConverter
    //{
    //    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        try
    //        {
    //            if (double.TryParse(value.ToString(), out var valueDouble))
    //            {
    //                return DisplayConversionService.ConvertMlToCups(valueDouble);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return value;
    //        }
    //        return value;
    //    }

    //    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    //    {
    //        try
    //        { 
    //            if (double.TryParse(value.ToString(), out var valueDouble))
    //            {
    //                return DisplayConversionService.ConvertCupsToMl(valueDouble);
    //            }
    //        }
    //        catch (Exception ex)
    //        {
    //            return value;
    //        }
    //        return value;
    //    }

    //    public object ProvideValue(IServiceProvider serviceProvider)
    //    {
    //        return new MetricLiquidToFluidOunceConverter();
    //    }
    //}

}