using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class IntegrationTypeToIconStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iconString = string.Empty;

            if (value != null)
            {
                var stringValue = value.ToString();

                if (Enum.TryParse(stringValue, out IntegrationType integrationType))
                    switch (integrationType)
                    {
                        case IntegrationType.Device:
                            iconString = "\uf63e";
                            break;
                        case IntegrationType.Service:
                            iconString = "\uf40e";
                            break;
                        default:
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