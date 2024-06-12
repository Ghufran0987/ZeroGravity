using System;
using System.Globalization;
using Xamarin.Forms;
using ZeroGravity.Mobile.Resources.Fonts;

namespace ZeroGravity.Mobile.Contract.Helper.Converter
{
    public class RSSIToColorConverter : IValueConverter
    {
        public object Convert(object value,  Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return CustomColors.LightGray;
            }

            var rssi =-1 * (int)value;
            //var place = Convert.ToInt( c);

            if(rssi==0)
                return CustomColors.LightGray;

            switch (parameter)
            {
                case "1":
                    if (rssi <= 60)
                    {
                        return CustomColors.Green;
                    }
                    else if(rssi>60 && rssi<=70)
                    {
                        return CustomColors.Green;
                    }
                    else if (rssi > 70 && rssi <= 80)
                    {
                        return CustomColors.Green;
                    }
                    else if (rssi > 80 && rssi <= 90)
                    {
                        return CustomColors.Green;
                    }
                    else if (rssi > 90)
                    {
                        return CustomColors.LightGray;
                    }
                    break;
                case "2":
                    if (rssi <= 60)
                    {
                        return CustomColors.Green;
                    }
                    else if (rssi > 60 && rssi <= 70)
                    {
                        return CustomColors.Green;
                    }
                    else if (rssi > 70 && rssi <= 80)
                    {
                        return CustomColors.Green;
                    }
                    else if (rssi > 80 && rssi <= 90)
                    {
                        return CustomColors.LightGray;
                    }
                    else if (rssi > 90)
                    {
                        return CustomColors.LightGray;
                    }
                    break;
                case "3":
                    if (rssi <= 60)
                    {
                        return CustomColors.Green;
                    }
                    else if (rssi > 60 && rssi <= 70)
                    {
                        return CustomColors.Green;
                    }
                    else if (rssi > 70 && rssi <= 80)
                    {
                        return CustomColors.LightGray;
                    }
                    else if (rssi > 80 && rssi <= 90)
                    {
                        return CustomColors.LightGray;
                    }
                    else if (rssi > 90)
                    {
                        return CustomColors.LightGray;
                    }
                    break;
                case "4":
                    if (rssi <= 60)
                    {
                        return CustomColors.Green;
                    }
                    else if (rssi > 60 && rssi <= 70)
                    {
                        return CustomColors.LightGray;
                    }
                    else if (rssi > 70 && rssi <= 80)
                    {
                        return CustomColors.LightGray;
                    }
                    else if (rssi > 80 && rssi <= 90)
                    {
                        return CustomColors.DarkGray;
                    }
                    else if (rssi > 90 )
                    {
                        return CustomColors.LightGray;
                    }
                    break;
            }
            return CustomColors.LightGray;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
