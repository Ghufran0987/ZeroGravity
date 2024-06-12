using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class MetricLiquidMilliliterToFluidOunceConverter : IMarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value?.ToString(), out var valueDouble))
            {
                //change Ml to l
                valueDouble = valueDouble / 1000;

                var displayPrefences = DisplayConversionService.GetDisplayPrefences();

                if (displayPrefences.UnitDisplayType == UnitDisplayType.Imperial)
                    return DisplayConversionService.ConvertFluidOz(valueDouble, UnitDisplayType.Imperial);
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), out var valueDouble))
            {
                var displayPrefences = DisplayConversionService.GetDisplayPrefences();

                if (displayPrefences.UnitDisplayType == UnitDisplayType.Imperial)
                    return DisplayConversionService.ConvertFluidOz(valueDouble, UnitDisplayType.Metric);
            }

            return value;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return new MetricLiquidMilliliterToFluidOunceConverter();
        }
    }
}