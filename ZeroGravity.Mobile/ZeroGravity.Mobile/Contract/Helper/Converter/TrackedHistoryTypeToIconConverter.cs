using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Shared.Enums;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class TrackedHistoryTypeToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var iconString = string.Empty;

            if (value != null)
            {
                var stringValue = value.ToString();

                if (Enum.TryParse(stringValue, out HistoryItemType historyItemType))
                    switch (historyItemType)
                    {
                        case HistoryItemType.Activity:
                            iconString = "\uf554";
                            break;

                        case HistoryItemType.Breakfast:
                            iconString = "\uf7f6";
                            break;

                        case HistoryItemType.Lunch:
                            iconString = "\uf81e";
                            break;

                        case HistoryItemType.Dinner:
                            iconString = "\uf817";
                            break;

                        case HistoryItemType.HealthySnack:
                            iconString = "\uf5d1";
                            break;

                        case HistoryItemType.UnhealthySnack:
                            iconString = "\uf563";
                            break;

                        case HistoryItemType.CalorieDrinkAlcohol:
                            iconString = "\uf869";
                            break;

                        case HistoryItemType.WaterIntake:
                            iconString = "\uf804";
                            break;

                        case HistoryItemType.Wellbeing:
                            iconString = "\uf5bb";
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