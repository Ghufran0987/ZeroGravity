using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZeroGravity.Mobile.Services;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class MetricWeightToPoundsConverter : IMarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), out var valueDouble))
            {
                var displayPrefences = DisplayConversionService.GetDisplayPrefences();

               if(displayPrefences.UnitDisplayType == UnitDisplayType.Imperial)
               {
                   return DisplayConversionService.ConvertWeight(valueDouble, UnitDisplayType.Imperial);
               }
            }

            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), out var valueDouble))
            {
                var displayPrefences = DisplayConversionService.GetDisplayPrefences();

                if(displayPrefences.UnitDisplayType == UnitDisplayType.Imperial)
                {
                    return DisplayConversionService.ConvertWeight(valueDouble, UnitDisplayType.Metric);
                }
            }

            return value;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return new MetricWeightToPoundsConverter();
        }
    }
}