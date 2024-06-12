using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resx;
using ZeroGravity.Shared.Constants;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class LiquidAmountToCupsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (double.TryParse(value.ToString(), out var valueDouble))
            {
                var numberOfCups = (int)(valueDouble / LiquidIntakeConstants.CupAmountMl);

                return numberOfCups.ToString();
                // return string.Format(AppResources.LiquidIntake_Cups, numberOfCups);
            }

            return 0.ToString();
            // return string.Format(AppResources.LiquidIntake_Cups, 0);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}